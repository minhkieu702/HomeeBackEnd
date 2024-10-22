using AutoMapper;
using Homee.BusinessLayer.Commons;
using Homee.BusinessLayer.IServices;
using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using Homee.DataLayer.ResponseModels;
using Homee.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Homee.BusinessLayer.Services
{
    public class ContractService : IContractService
    {
        private readonly IMapper _mapper;
        private readonly IContractRepository _repo;
        private readonly IAccountRepository _accRepo;
        public ContractService(IMapper mapper, IContractRepository repo, IAccountRepository accRepo)
        {
            _accRepo = accRepo;
            _mapper = mapper;
            _repo = repo;
        }
        public async Task<IHomeeResult> Delete(int id)
        {
            try
            {
                var result = await _repo.GetById(id);
                if (result == null) return new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);

                _repo.Delete(result);
                var check = await _repo.SaveChangesAsync();

                return check <= 0 ? new HomeeResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG) : new HomeeResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IHomeeResult> Create(ContractRequest model, ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                var claim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
                if (claim == null || string.IsNullOrEmpty(claim.Value) || !int.TryParse(claim.Value, out int uId))
                    return new HomeeResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);

                var user = await _accRepo.GetById(uId);
                if (user == null)
                    return new HomeeResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);

                bool result = _repo.CanCreate(model);
                if (!result)
                    return new HomeeResult(Const.FAIL_CREATE_CODE, "This address is already registered.");
                var contract = _mapper.Map<Contract>(model);
                contract.RenterId = uId;
                contract.CreateAt = DateTime.Now;
                await _repo.InsertAsync(contract);
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
                var result = _repo.GetContracts();
                return result.Count() <= 0 ? new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG) : new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result.Select(_mapper.Map<ContractResponse>));
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
                var result = _repo.GetContract(id);
                return result == null ? new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG) : new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, _mapper.Map<ContractResponse>(result));
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IHomeeResult> Update(int id, long model)
        {
            try
            {
                var result = await _repo.GetById(id);
                if (result == null)
                {
                    return new HomeeResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                }
                result.Duration = model;

                _repo.Update(result);
                var check = await _repo.SaveChangesAsync();

                return check > 0 ?
                    new HomeeResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG) :
                    new HomeeResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IHomeeResult> GetByCurrentUser(ClaimsPrincipal user)
        {
            try
            {
                var result = _repo.GetContractByCurrentUser(user);
                return result == null || result.Count <= 0 ? new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG) : new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result.Select(_mapper.Map<ContractResponse>));
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IHomeeResult> Confirm(int contractId)
        {
            try
            {
                var result = await _repo.GetById(contractId);
                if (result == null) return new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                result.Confirmed = true;
                _repo.Update(result);
                return _repo.SaveChanges() > 0 ? new HomeeResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG) : new HomeeResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
    }
}
