using AutoMapper;
using Homee.BusinessLayer.Commons;
using Homee.BusinessLayer.IServices;
using Homee.DataLayer.RequestModels;
using Homee.DataLayer.Models;
using Homee.Repositories.IRepositories;
using Homee.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Homee.DataLayer.ResponseModels;
using System.Security.Claims;
using Homee.BusinessLayer.Helpers;

namespace Homee.BusinessLayer.Services
{
    public class PlaceService : IPlaceService
    {
        private readonly IMapper _mapper;
        private readonly IPlaceRepository _repo;

        public PlaceService(IMapper mapper, IPlaceRepository repository)
        {
            _mapper = mapper;
            _repo = repository;
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

        public async Task<IHomeeResult> GetAll()
        {
            try
            {
                var result = _repo.GetPlaces();
                return result.Count() <= 0 ? new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG) : new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result.Select(_mapper.Map<PlaceResponse>));
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IHomeeResult> GetByCurrentUser(ClaimsPrincipal claims)
        {
            try
            {
                var userId = claims.FindFirst(ClaimTypes.NameIdentifier);
                if (userId == null || !int. TryParse(userId.Value, out int uid))
                {
                    return new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
                var result = _repo.GetAll(c => c.OwnerId == uid).IncludeAll();
                return result.Count() <= 0 ? new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG) : new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result.Select(_mapper.Map<PlaceResponse>));
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
                var result = _repo.GetPlace(id);
                return result == null ? new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG) : new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, _mapper.Map<PlaceResponse>(result));
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IHomeeResult> Insert(PlaceRequest model, ClaimsPrincipal user)
        {
            try
            {
                bool result = await _repo.CanInsert(model);
                if (!result)
                {
                    return new HomeeResult(Const.FAIL_CREATE_CODE, "This address is already registered.");
                }
                //await _repo.InsertPlace(_mapper.Map<Place>(model));
                var check = await _repo.InsertPlace(model, user);
                return check <= 0 ?
                    new HomeeResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG) :
                    new HomeeResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IHomeeResult> Update(int id, PlaceRequest model, ClaimsPrincipal user)
        {
            try
            {
                var result = await _repo.GetById(id);
                if (result == null)
                {
                    return new HomeeResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                }
                var check = await _repo.UpdatePlace(result, model, user);
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
