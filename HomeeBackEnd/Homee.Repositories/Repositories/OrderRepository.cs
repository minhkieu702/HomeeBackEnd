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
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Homee.Repositories.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly IConfiguration _config;
        private readonly HomeedbContext _context;

        public OrderRepository()
        {
            
        }
        public OrderRepository(HomeedbContext context, IConfiguration configuration)
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
            return _context.Orders.IncludeAll(_context).FirstOrDefault(c => c.OrderId == id);
        }
        public List<Order> GetAllOrders()
        {
            var orders = _context.Orders.IncludeAll(_context).ToList();
            return orders;
        }

        public async Task<int> CanInsert(PAYOS_RETURN_URLRequest request)
        {
            try
            {
                PayOS payOS = new(_config["PAYOS_CLIENT_ID"], _config["PAYOS_API_KEY"], _config["PAYOS_CHECKSUM_KEY"]);

                var paymentLinkInformation = await payOS.getPaymentLinkInformation(request.OrderCode);

                if (paymentLinkInformation.status != "PAID") return -1;

                if (_context.Orders.FirstOrDefault(c => request.PaymentId.Equals(c.PaymentId)) != null) return 0;

                return 1;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<string> CreatePaymentUrl()
        {
            try
            {
                PayOS payOS = new(_config["PAYOS_CLIENT_ID"], _config["PAYOS_API_KEY"], _config["PAYOS_CHECKSUM_KEY"]);

                var item = new ItemData("post", 1, 20_000);

                List<ItemData> items = [item];
                int expired = int.Parse(DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeSeconds().ToString());

                var paymentData = new PaymentData(SupportingFeature.Instance.GenerateUniqueCode(), item.price, "one post", items, _config["PAYOS_RETURN_URL"], _config["PAYOS_RETURN_URL"], expiredAt: expired);

                CreatePaymentResult createPayment = await payOS.createPaymentLink(paymentData);

                return createPayment.checkoutUrl;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<string> CreatePaymentUrl(int subId)
        {
            try
            {
                PayOS payOS = new(_config["PAYOS_CLIENT_ID"], _config["PAYOS_API_KEY"], _config["PAYOS_CHECKSUM_KEY"]);
                
                var subscription = _context.Subscriptions.FirstOrDefault(c => c.SubscriptionId == subId);
                
                if (subscription == null) return null;
                
                var item = new ItemData(subscription.SubscriptionName, 1, (int)subscription.Price);
                                
                List<ItemData> items = [item];
                int expired = int.Parse(DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeSeconds().ToString());

                var paymentData = new PaymentData(SupportingFeature.Instance.GetOrderCode(subId), item.price, subscription.SubscriptionName, items, _config["PAYOS_RETURN_URL"], _config["PAYOS_RETURN_URL"], expiredAt: expired);

                CreatePaymentResult createPayment = await payOS.createPaymentLink(paymentData);
                
                return createPayment.checkoutUrl;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> InsertOrder(PAYOS_RETURN_URLRequest payment, ClaimsPrincipal user)
        {
            try
            {
                var subscription = _context.Subscriptions.FirstOrDefault(c => c.SubscriptionId == payment.SubId);
                var accId = 1;
                var order = new Order();
                if (subscription == null)
                {
                    order = new Order
                    {
                        SubscribedAt = DateTime.Now,
                        OwnerId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier).ToString()),
                        PaymentId = payment.PaymentId,
                    };
                }
                else
                {
                    order = new Order
                    {
                        SubscriptionId = payment.SubId,
                        SubscribedAt = DateTime.Now,
                        ExpiredAt = DateTime.Now.AddDays((double)subscription.Duration),
                        PaymentId = payment.PaymentId,
                        OwnerId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier).ToString())
                    };
                }
                await _context.Orders.AddAsync(order);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
