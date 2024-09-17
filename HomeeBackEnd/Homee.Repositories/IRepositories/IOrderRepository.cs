using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.Repositories.IRepositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<bool> CanInsert(int model);
        List<Order> GetAllOrders();
        Order GetOrder(int id);
        Task<string> CreateOrderTemp(int subId, HttpContext httpContext);
        Task<int> UpdatePlace(Order result, OrderRequest model);
    }
}
