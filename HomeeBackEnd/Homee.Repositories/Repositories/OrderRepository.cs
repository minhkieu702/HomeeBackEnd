using Azure.Core;
using Homee.BusinessLayer.Helpers;
using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using Homee.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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

        private Account? GetAccount(ClaimsPrincipal? user, string? email)
        {
            if (user == null && !email.IsNullOrEmpty())
            {
                return _context.Accounts.FirstOrDefault(c => c.Email.Equals(email));
            }
            var accountId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return _context.Accounts.FirstOrDefault(c => c.AccountId.Equals(accountId));
        }

        public async Task<string> CreatePaymentUrl(ClaimsPrincipal user)
        {
            try
            {
                // Initialize PayOS client
                var payOS = new PayOS(_config["PAYOS_CLIENT_ID"], _config["PAYOS_API_KEY"], _config["PAYOS_CHECKSUM_KEY"]);

                // Retrieve the account from the database
                var account = GetAccount(user, null);

                if (account == null)
                    throw new Exception("Account not found.");

                var orderCode = SupportingFeature.Instance.GetOrderCode(0);

                var order = new Order
                {
                    OrderId = orderCode,
                    SubscribedAt = DateTime.Now,
                    OwnerId = account.AccountId,
                };

                // Create payment item
                var item = new ItemData("post", 1, 20_000);
                var items = new List<ItemData> { item };

                // Set expiration time
                int expired = (int)DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeSeconds();

                // Build payment data
                var paymentData = new PaymentData(
                    SupportingFeature.Instance.GetOrderCode(0),
                    item.price,
                    "one post",
                    items,
                    _config["PAYOS_RETURN_URL"],
                    _config["PAYOS_RETURN_URL"],
                    expiredAt: expired,
                    buyerEmail: account.Email
                );

                // Create payment link
                var createPayment = await payOS.createPaymentLink(paymentData);
                return createPayment.checkoutUrl;
            }
            catch (Exception ex)
            {
                // Handle exceptions and log if needed
                throw new Exception("Error while creating payment URL", ex);
            }
        }

        public async Task<string> CreatePaymentUrl(int subId, ClaimsPrincipal user)
        {
            try
            {
                // Initialize PayOS client
                var payOS = new PayOS(_config["PAYOS_CLIENT_ID"], _config["PAYOS_API_KEY"], _config["PAYOS_CHECKSUM_KEY"]);

                // Retrieve the account from the database
                var account = GetAccount(user, null);

                if (account == null)
                    throw new Exception("Account not found.");

                // Retrieve the subscription from the database
                var subscription = _context.Subscriptions.FirstOrDefault(s => s.SubscriptionId == subId);
                if (subscription == null)
                    throw new Exception("Subscription not found.");

                var order = new Order
                {
                    SubscriptionId = subId,
                    SubscribedAt = DateTime.Now,
                    ExpiredAt = DateTime.Now.AddDays((double)subscription.Duration),
                    OwnerId = account.AccountId,
                };

                _context.Orders.Add(order);
                var check = await _context.SaveChangesAsync();
                if (check == 0)
                    throw new Exception("Cannot create");

                // Create payment item based on subscription
                var item = new ItemData(subscription.SubscriptionName, 1, (int)subscription.Price);
                var items = new List<ItemData> { item };

                // Set expiration time
                int expired = (int)DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeSeconds();

                // Build payment data
                var paymentData = new PaymentData(
                    SupportingFeature.Instance.GetOrderCode(subId),
                    item.price,
                    subscription.SubscriptionName,
                    items,
                    _config["PAYOS_RETURN_URL"],
                    _config["PAYOS_RETURN_URL"],
                    expiredAt: expired,
                    buyerEmail: account.Email
                );

                // Create payment link
                var createPayment = await payOS.createPaymentLink(paymentData);
                return createPayment.checkoutUrl;
            }
            catch (Exception ex)
            {
                // Handle exceptions and log if needed
                throw new Exception("Error while creating payment URL", ex);
            }
        }


        public async Task<int> ConfirmOrder(PAYOS_RETURN_URLRequest payment)
        {
            try
            {
                var payOS = new PayOS(_config["PAYOS_CLIENT_ID"], _config["PAYOS_API_KEY"], _config["PAYOS_CHECKSUM_KEY"]);

                var subscription = _context.Subscriptions.FirstOrDefault(c => c.SubscriptionId == payment.SubId);

                var order = _context.Orders.FirstOrDefault(c => c.OrderId == payment.OrderCode);

                order.PaymentId = payment.PaymentId;

                _context.Orders.Update(order);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
