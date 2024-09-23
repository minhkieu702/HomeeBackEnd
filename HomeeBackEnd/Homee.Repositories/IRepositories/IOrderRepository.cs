using Homee.DataLayer.Models;
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
        Task<int> CanInsert(ReturnUrlRequest model);
        List<Order> GetAllOrders();
        Order GetOrder(int id);
        Task<string> CreatePaymentUrl(int subId);
        Task<int> UpdatePlace(Order result, OrderRequest model);
        Task<int> InsertOrder(ReturnUrlRequest result, ClaimsPrincipal user);
    }
}
