using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using Homee.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.Repositories.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly HomeeDbContext _context;

        public OrderRepository()
        {
            
        }
        public OrderRepository(HomeeDbContext context)
        {
            _context = context;
        }
        public async Task<int> UpdatePlace(Order result, OrderRequest model)
        {
            result.ExpiredAt = model.ExpiredAt;
            result.SubscribedAt = model.SubscribedAt;
            result.SubscriptionId = model.SubscriptionId;
            _context.Orders.Update(result);
            return await _context.SaveChangesAsync();
        }
        public Order GetOrder(int id)
        {
            var order = _context.Orders.Include(c => c.Owner).Include(c => c.Subscription).FirstOrDefault(c => c.OrderId == id);
            if (order != null)
            {
                order.Owner.Orders = null;
                order.Subscription.Orders = null;
            }
            return order;
        }
        public List<Order> GetAllOrders()
        {
             var orders = _context.Orders.Include(c => c.Owner).Include(c => c.Subscription).ToList();
            orders.ForEach(o =>
            {
                o.Owner.Orders = null;
                o.Subscription.Orders = null;
            }); 
            return orders;
        }
    }
}
