﻿using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Homee.Repositories.IRepositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<int> CanInsert(PAYOS_RETURN_URLRequest model);
        List<Order> GetAllOrders();
        Order GetOrder(long id);
        Task<string> CreatePaymentUrl(int subId, ClaimsPrincipal user);
        Task<string> CreatePaymentUrl(ClaimsPrincipal user);
        Task<int> UpdatePlace(Order result, OrderRequest model);
        Task<int> ConfirmOrder(PAYOS_RETURN_URLRequest result);
    }
}
