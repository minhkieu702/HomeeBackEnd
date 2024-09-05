using AutoMapper;
using Homee.BusinessLayer.Commons;
using Homee.BusinessLayer.IServices;
using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using Homee.Repositories.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.BusinessLayer.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly AccountRepository _repo;

        public AccountService(IMapper mapper, AccountRepository accountRepository)
        {
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
                if (!result)
                {
                    return new HomeeResult(Const.FAIL_CREATE_CODE, "This email is already used.");
                }
                await _repo.InsertAsync(_mapper.Map<Account>(model));
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

        public async Task<IHomeeResult> GetAll()
        {
            try
            {
                var result = _repo.GetAccounts();
                return result.Count() <= 0 ? new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG) : new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
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
                return result != null ? new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG) : new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
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
                var result = _repo.GetAll();
                if (result.Count() <= 0)
                {
                    return new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
                var account = result.FirstOrDefault(c => c.Email.Equals(email) && c.Password.Equals(password));
                
                return new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, account);
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public Task<IHomeeResult> Update(int id, AccountRequest model)
        {
            throw new NotImplementedException();
        }
    }
}
