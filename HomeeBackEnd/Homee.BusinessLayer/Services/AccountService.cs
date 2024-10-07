using AutoMapper;
using Homee.BusinessLayer.Commons;
using Homee.BusinessLayer.Helpers;
using Homee.BusinessLayer.IServices;
using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using Homee.DataLayer.ResponseModels;
using Homee.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Homee.BusinessLayer.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        private readonly IConfiguration _config;
        private readonly IAccountRepository _repo;


        public AccountService(IAccountRepository accountRepository, IConfiguration configuration, IMailService mailService, IMapper mapper)
        {
            _mapper = mapper;
            _mailService = mailService;
            _config = configuration;
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
        //public async Task<IHomeeResult> ConfirmOtpToRegister(string otp, HttpContext context)
        //{
        //    try
        //    {
        //        if (!SupportingFeature.GetValueFromSession("user", out Account user, context) ||
        //            !SupportingFeature.GetValueFromSession("otp", out string storedOtp, context) ||
        //            user == null ||
        //            string.IsNullOrEmpty(storedOtp) ||
        //            !otp.Equals(storedOtp)
        //            )
        //        {
        //            return new HomeeResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
        //        }

        //        await _repo.InsertAsync(user);
        //        var check = await _repo.SaveChangesAsync();

        //        return check > 0 ? new HomeeResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG) : new HomeeResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
        //    }
        //}
        public async Task<IHomeeResult> ConfirmEmaiToGetNewPassword(string email, HttpContext context)
        {
            try
            {
                var accounts = _repo.GetAll(c => c.Email.Equals(email)).FirstOrDefault();
                if (accounts == null)
                {
                    return new HomeeResult(Const.FAIL_CREATE_CODE, "This email does already not exist.");
                }
                var token = Guid.NewGuid().ToString() + DateTime.Now.Ticks;
                
                accounts.VerificationToken = token;
                accounts.IsVerified = false;
                _repo.Update(accounts);
                _repo.SaveChanges();
                var result = await SendOtp(email, context, token);
                return result;
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
        private async Task<IHomeeResult> SendOtp(string email, HttpContext context, string otp)
        {
            try
            {
                var content = $"<a href='{_config["Verify_Email_URL"]}?token={otp}'>Click here to verify</a>";
                var result = await _mailService.SendMail(email, "Verify your email", content);
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
                return new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, accounts.Select(_mapper.Map<AccountResponse>));
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
                if (!SupportingFeature.GetValueFromSession("email", out string email, context) || string.IsNullOrEmpty(email))
                {
                    return new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
                var account = _repo.GetAccounts().FirstOrDefault(c => c.Email.Equals(email));
                if (account == null)
                {
                    return new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
                account.Password = password;
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
                var token = Guid.NewGuid().ToString() + DateTime.Now.Ticks;
                var account = new Account
                {
                    Email = model.Email,
                    Password = model.Password,
                    ImageUrl = model.ImageUrl ?? "https://genslerzudansdentistry.com/wp-content/uploads/2015/11/anonymous-user.png",
                    Name = model.Email.Substring(0, model.Email.IndexOf("@")),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Role = 0,
                    IsBlock = false,
                    VerificationToken = token,
                };

                _repo.Insert(account);

                _repo.SaveChanges();

                var result = await SendOtp(model.Email, context, token);

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
                return new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, _mapper.Map < AccountResponse > (account));
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IHomeeResult> GetProfile(ClaimsPrincipal user)
        {
            try
            {
                if (!int.TryParse(user.FindFirst(ClaimTypes.NameIdentifier).Value, out int userId))
                {
                    return new HomeeResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }

                var result = _repo.GetAccount(userId);

                return result == null ? new HomeeResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG) : new HomeeResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, result);
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IHomeeResult> Login(string email, string password)
        {
            try
            {
                var account = _repo.GetAll(c => c.Email.Equals(email) && c.Password.Equals(password)).FirstOrDefault();

                if (account == null) return new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);

                var token = GenerateJwtToken(account);

                return new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, token);
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        private string GenerateJwtToken(Account account)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
                new Claim(ClaimTypes.Role, account.Role.ToString())
            };
                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                    _config["Jwt:Issuer"],
                    claims,
                    expires: DateTime.Now.AddHours(120),
                    signingCredentials: credentials);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception)
            {
                throw;
            }
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
                
                int role = result.Role;
                result = _mapper.Map<Account>(model);
                result.Role = role;
                result.AccountId = id;

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
