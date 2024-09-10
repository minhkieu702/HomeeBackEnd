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
using System.Text;
using System.Threading.Tasks;

namespace Homee.BusinessLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mappper;
        private readonly IOrderRepository _repo;

        public OrderService(IMapper mapper, IOrderRepository orderRepository)
        {
            _mappper = mapper;
            _repo = orderRepository;
        }
        public async Task<IHomeeResult> Create(OrderRequest model)
        {
            try
            {
                //bool result = await _repo.CanInsert(model);
                //if (!result)
                //{
                //    return new HomeeResult(Const.FAIL_CREATE_CODE, "This address is already registered.");
                //}
                //await _repo.InsertPlace(_mapper.Map<Place>(model));
                _repo.Insert(_mappper.Map<Order>(model));
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
                var result = _repo.GetAllOrders();
                return result == null ? new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG) : new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result.Select(_mappper.Map<ContractResponse>));
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
                var result = _repo.GetOrder(id);
                return result == null ? new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG) : new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, _mappper.Map<OrderResponse>(result));
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IHomeeResult> Update(int id, OrderRequest model)
        {
            try
            {
                var result = await _repo.GetById(id);
                if (result == null)
                {
                    return new HomeeResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                }
                var check = await _repo.UpdatePlace(result, model);
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
