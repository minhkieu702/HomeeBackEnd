using Homee.BusinessLayer.Helpers;
using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using Homee.DataLayer.ResponseModels;
using Homee.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.Repositories.Repositories
{
    public class SubscriptionRepository : BaseRepository<Subscription>, ISubscriptionRepository
    {
        private readonly HomeedbContext _context;

        public SubscriptionRepository(HomeedbContext context)
        {
            _context = context;
        }
        public async Task<bool> CanInsert(SubscriptionRequest model)
        {
            var subscription = _context.Subscriptions.Where(c => c.Price == model.Price && c.Duration == model.Duration).FirstOrDefault();
            return subscription != null;
        }

        public Subscription GetSubscription(int id)
        {
            return _context.Subscriptions.IncludeAll(_context).FirstOrDefault(c => c.SubscriptionId == id);

        }

        public List<Subscription> GetSubscriptions()
        {
            return _context.Subscriptions.IncludeAll(_context).ToList();
        }

        public int CanDelete(int id)
        {
            var subscription = _context.Subscriptions.FirstOrDefault(c => c.SubscriptionId == id);
            if (subscription == null)
            {
                return 0;
            }
            var orders = _context.Orders.Where(c => c.SubscriptionId == id);
            if (orders.Count() >= 0)
            {
                _context.Orders.RemoveRange(orders);
            }
            _context.Subscriptions.Remove(subscription);
            return _context.SaveChanges();
        }
    }
}
