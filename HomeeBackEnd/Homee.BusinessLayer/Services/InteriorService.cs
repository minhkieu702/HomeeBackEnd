using AutoMapper;
using Homee.BusinessLayer.Commons;
using Homee.BusinessLayer.IServices;
using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using Homee.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.BusinessLayer.Services
{
    public class InteriorService : IInteriorService
    {
        private readonly IInteriorRepository _repo;
        private readonly IMapper _mapper;

        public InteriorService(IMapper mapper, IInteriorRepository interiorRepository)
        {
            _repo = interiorRepository;
            _mapper = mapper;
        }
        public async Task<IHomeeResult> Create(InteriorRequest model)
        {
            try
            {
                await _repo.InsertAsync(_mapper.Map<Interior>(model));
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

        public async Task<IHomeeResult> GetAll()
        {
            try
            {
                var result = _repo.GetAll();
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
                var result = await _repo.GetById(id);
                return result == null ? new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG) : new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IHomeeResult> Update(int id, InteriorRequest model)
        {
            try
            {
                var result = await _repo.GetById(id);
                if (result == null)
                {
                    return new HomeeResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                }
                
                result.Status = model.Status;
                result.InteriorName = model.InteriorName;
                result.Description = model.Description;

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
    }
}
