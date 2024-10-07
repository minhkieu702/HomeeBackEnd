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
    public class OrderService : IOrderService
    {
        private readonly IMapper _mappper;
        private readonly IOrderRepository _repo;

        public OrderService(IMapper mapper, IOrderRepository orderRepository)
        {
            _mappper = mapper;
            _repo = orderRepository;
        }
        public async Task<IHomeeResult> Create(int subId)
        {
            try
            {
                //bool result = await _repo.CanInsert(subId);
                //if (!result)
                //{
                //    return new HomeeResult(Const.FAIL_CREATE_CODE, "This address is already registered.");
                //}
                string check = await _repo.CreatePaymentUrl(subId);
                return check == null || check.Length <= 0 ?
                    new HomeeResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG) :
                    new HomeeResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, check);
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
        
        public async Task<IHomeeResult> ExecutePayment(PAYOS_RETURN_URLRequest payment, ClaimsPrincipal user)
        {
            try
            {
                var result = await _repo.CanInsert(payment);
                if (result == -1) return new HomeeResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                if (result == 0) return new HomeeResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
                var check = await _repo.InsertOrder(payment, user);
                return check > 0 ? new HomeeResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG) : new HomeeResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
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
                return result.Count() == 0 ? new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG) : new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result.Select(_mappper.Map<OrderResponse>));
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
                var aid = user.FindFirst(ClaimTypes.NameIdentifier).Value;
                var orders = _repo.GetAll(c => c.OwnerId == int.Parse(aid));
                return orders.Count() <= 0 ? new HomeeResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG) : new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, orders.Select(_mappper.Map<OrderResponse>));
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
                return result == null ? new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG) : new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, _mappper.Map<OrderResponse>(_mappper.Map < OrderResponse > (result)));
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
