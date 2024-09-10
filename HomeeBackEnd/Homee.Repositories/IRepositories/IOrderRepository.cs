using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.Repositories.IRepositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        List<Order> GetAllOrders();
        Order GetOrder(int id);
        Task<int> UpdatePlace(Order result, OrderRequest model);
    }
}
