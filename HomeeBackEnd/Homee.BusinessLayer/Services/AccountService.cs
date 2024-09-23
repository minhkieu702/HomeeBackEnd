using AutoMapper;
using Homee.BusinessLayer.Commons;
using Homee.BusinessLayer.Helpers;
using Homee.BusinessLayer.IServices;
using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using Homee.DataLayer.ResponseModels;
using Homee.Repositories.IRepositories;
using Homee.Repositories.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Homee.BusinessLayer.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _config;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;
        private readonly IAccountRepository _repo;

        public AccountService(IMapper mapper, IAccountRepository accountRepository, IMailService mailService, IConfiguration configuration, IMemoryCache memoryCache)
        {
            _cache = memoryCache;
            _config = configuration;
            _mailService = mailService;
            _mapper = mapper;
            _repo = accountRepository;
        }
        public async Task<IHomeeResult> Block(int id)
        {
            try
            {
                var result = await _repo.GetById(id);
                if (result == null) return new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);

                result.IsBlock = true;
                _repo.Update(result);
                var check = await _repo.SaveChangesAsync();

                return check <= 0 ? new HomeeResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG) : new HomeeResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IHomeeResult> Create(AccountRequest model)
        {
            try
            {
                bool result = _repo.CanInsert(model);
                if (result)
                {
                    return new HomeeResult(Const.FAIL_CREATE_CODE, "This email is already used.");
                }
                var account = _mapper.Map<Account>(model);
                account.CreatedAt = account.UpdatedAt = DateTime.Now;
                await _repo.InsertAsync(account);
                var check = await _repo.SaveChangesAsync();
                return check <= 0 ?
                    new HomeeResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG) :
                    new HomeeResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
        public async Task<IHomeeResult> ConfirmOtpToRegister(string otp, HttpContext context)
        {
            try
            {
                if (!SupportingFeature.GetValueFromSession("user", out Account user, context) ||
                    !SupportingFeature.GetValueFromSession("otp", out string storedOtp, context) || 
                    user == null ||
                    string.IsNullOrEmpty(storedOtp) ||
                    !otp.Equals(storedOtp)
                    )
                {
                    return new HomeeResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                }
                
                await _repo.InsertAsync(user);
                var check = await _repo.SaveChangesAsync();
                
                return check > 0 ? new HomeeResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG) : new HomeeResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
        public async Task<IHomeeResult> ConfirmEmaiToGetNewPassword(string email, HttpContext context)
        {
            try
            {
                var accounts = _repo.GetAll();
                if (!accounts.Any(c => c.Email.Equals(email)))
                {
                    return new HomeeResult(Const.FAIL_CREATE_CODE, "This email does already not exist.");
                }
                var result = await SendOtp(email, context);
                return result;
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
        private async Task<IHomeeResult> SendOtp(string email, HttpContext context)
        {
            try
            {
                var otp = SupportingFeature.Instance.GenerateOTP();

                var result = await _mailService.SendMail(email, "CONFIRMING CODE", otp);
                if (result.Status > 0)
                {
                    SupportingFeature.SetValueToSession("otp", otp, context);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
        public async Task<IHomeeResult> GetAll()
        {
            try
            {
                List<Account> result = _repo.GetAccounts();
                if (result.Count() <= 0)
                {
                    new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
                var accounts = result.Select(_mapper.Map<AccountResponse>);
                foreach (var account in accounts)
                {
                    account.LastOrder = account.Orders.Where(c => c.OwnerId == account.AccountId && c.ExpiredAt > DateTime.Now).LastOrDefault();
                }
                return new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, accounts);
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
        public async Task<IHomeeResult> ResetPassword(string password, HttpContext context)
        {
            try
            {
                if(!SupportingFeature.GetValueFromSession("email", out string email, context) || string.IsNullOrEmpty(email))
                {
                    return new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
                var account = _repo.GetAccounts().FirstOrDefault(c => c.Email.Equals(email));
                if (account == null)
                {
                    return new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
                account.Password= password;
                _repo.Update(account);
                var check = await _repo.SaveChangesAsync();
                if (check <= 0)
                {
                    return new HomeeResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                }
                return new HomeeResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IHomeeResult> Register(AccountRequest model, HttpContext context)
        {
            try
            {
                var accounts = await _repo.FirstOrDefaultAsync(c => c.Email.Equals(model.Email));
                if (accounts != null)
                {
                    return new HomeeResult(Const.FAIL_CREATE_CODE, "This email is already used.");
                }
                var account = new Account
                {
                    Email = model.Email,
                    Password = model.Password,
                    ImageUrl = model.ImageUrl ?? "https://genslerzudansdentistry.com/wp-content/uploads/2015/11/anonymous-user.png",
                    Name = model.Email.Substring(0, model.Email.IndexOf("@")),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Role = 0,
                    IsBlock = false
                };
                SupportingFeature.SetValueToSession("user", account, context);

                var result = await SendOtp(model.Email, context);

                return result.Status > 0 ? new HomeeResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG) : new HomeeResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
        public async Task<IHomeeResult> GetById(int id)
        {
            try
            {
                var result = _repo.GetAccount(id);
                if (result == null)
                {
                    new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
                var account = _mapper.Map<AccountResponse>(result);
                account.LastOrder = account.Orders.Where(c => c.OwnerId == account.AccountId && c.ExpiredAt > DateTime.Now).LastOrDefault();
                return new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, account);
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        private string GenerateJwtToken(string email, int role, int id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            var tokenDescriptior = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, role.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:Time"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Issuer"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptior);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public async Task<IHomeeResult> Login(string email, string password, HttpContext context)
        {
            try
            {
                var account = _repo.GetAll(c => c.Email.Equals(email) && c.Password.Equals(password)).FirstOrDefault();

                if(account == null) return new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);

                var token = GenerateJwtToken(account.Email, account.Role, account.AccountId);
                var refreshToken = GenerateRefreshToken();

                _cache.Set(account.AccountId, refreshToken, TimeSpan.FromMinutes(119));
                
                return new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, new {Token = token, RefreshToken = refreshToken });
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public IHomeeResult RefreshToken(RefreshTokenModel model)
        {
            if (_cache.TryGetValue(model.AccountId, out string storedToken) 
                && !string.IsNullOrEmpty(storedToken) 
                && storedToken.Equals(model.RefreshToken))
            {
                var newRefreshToken = GenerateRefreshToken();
                _cache.Set(model.AccountId, newRefreshToken, TimeSpan.FromMinutes(119));

                return new HomeeResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, new {RefreshToken = newRefreshToken});
            }
            return new HomeeResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
        }

        public async Task<IHomeeResult> Update(int id, AccountRequest model)
        {
            try
            {
                var result = await _repo.GetById(id);
                if (result == null)
                {
                    return new HomeeResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                }
                
                result.UpdatedAt = DateTime.Now;
                result.Password = model.Password;
                result.ImageUrl = model.ImageUrl;
                result.Name = model.Name;
                result.Phone = model.Phone;
                result.CitizenId = model.CitizenId;
                result.BirthDay = model.BirthDay;

                _repo.Update(result);
                var check = await _repo.SaveChangesAsync();
                return check <= 0 ?
                    new HomeeResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG) :
                new HomeeResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
    }
}
