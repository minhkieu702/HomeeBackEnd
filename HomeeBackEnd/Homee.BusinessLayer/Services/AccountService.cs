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
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Security.Principal;

namespace Homee.BusinessLayer.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;
        private readonly IAccountRepository _repo;

        public AccountService(IMapper mapper, IAccountRepository accountRepository, IMailService mailService)
        {
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
        public async Task<IHomeeResult> ConfirmEmaiToRegister(string email, HttpContext context)
        {
            try
            {
                var accounts = _repo.GetAll();
                if (accounts.Any(c => c.Email.Equals(email)))
                {
                    return new HomeeResult(Const.FAIL_CREATE_CODE, "This email is already used.");
                }
                var result = await SendOtp(email, context);
                return result;
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
                    SupportingFeature.SetValueToSession("email", email, context);
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
                if(!SupportingFeature.GetValueFromSession("email", out string email, context) || email.IsNullOrEmpty())
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
                if (!SupportingFeature.GetValueFromSession("email", out string email, context))
                {
                    return new HomeeResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                }
                model.Email = email;
                model.Role = 0;
                var account = _mapper.Map<Account>(model);
                account.CreatedAt = DateTime.Now;
                account.UpdatedAt = DateTime.Now;
                _repo.Insert(account);
                var check = await _repo.SaveChangesAsync();
                return check > 0 ? new HomeeResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG) : new HomeeResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
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

        public async Task<IHomeeResult> Login(string email, string password, HttpContext context)
        {
            try
            {
                var result = _repo.GetAll();
                if (result.Count() <= 0)
                {
                    return new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
                var account = result.FirstOrDefault(c => c.Email.Equals(email) && c.Password.Equals(password));
                if(account == null) return new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                SupportingFeature.SetValueToSession("user", account, context);
                return new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, _mapper.Map<AccountResponse>(account));
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
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
