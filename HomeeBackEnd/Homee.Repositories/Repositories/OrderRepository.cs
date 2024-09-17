using Homee.BusinessLayer.Helpers;
using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using Homee.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Net.payOS;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.Repositories.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly IConfiguration _config;
        private readonly HomeeDbContext _context;

        public OrderRepository()
        {
            
        }
        public OrderRepository(HomeeDbContext context, IConfiguration configuration)
        {
            _config = configuration;
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

        public async Task<bool> CanInsert(int sId)
        {
            return true;
        }

        public async Task<string> CreateOrderTemp(int subId, HttpContext httpContext)
        {
            try
            {
                PayOS payOS = new(_config["PAYOS_CLIENT_ID"], _config["PAYOS_API_KEY"], _config["PAYOS_CHECKSUM_KEY"]);
                
                var subscription = _context.Subscriptions.FirstOrDefault(c => c.SubscriptionId == subId);
                
                if (subscription == null) return null;
                
                var item = new ItemData(subscription.SubscriptionName, 1, (int)subscription.Price);

                var accId = 1;
                if (SupportingFeature.GetValueFromSession("user", out Account user, httpContext))
                {
                    accId = user.AccountId;
                }

                var expiredAt = DateTime.Now.AddDays((double)subscription.Duration);

                var order = new Order
                {
                    SubscriptionId = subId,
                    ExpiredAt = expiredAt,
                    OwnerId = accId,
                    SubscribedAt = DateTime.Now,
                };

                await _context.Orders.AddAsync(order);
                
                var check = await _context.SaveChangesAsync();
                
                List<ItemData> items = [item];

                SupportingFeature.SetValueToSession("orderId", order.OrderId, httpContext);

                var paymentData = new PaymentData(order.OrderId, item.price, "Dang ky goi "+subscription.SubscriptionName, items, _config["LocalHostUrl"], _config["LocalHostUrl"]);

                CreatePaymentResult createPayment = await payOS.createPaymentLink(paymentData);
                
                return createPayment.checkoutUrl;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
