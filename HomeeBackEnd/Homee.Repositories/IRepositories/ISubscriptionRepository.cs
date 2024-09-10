using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using Homee.DataLayer.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.Repositories.IRepositories
{
    public interface ISubscriptionRepository : IBaseRepository<Subscription>
    {
        int CanDelete(int id);
        Task<bool> CanInsert(SubscriptionRequest model);
        Subscription GetSubscription(int id);
        List<Subscription> GetSubscriptions();
    }
}
