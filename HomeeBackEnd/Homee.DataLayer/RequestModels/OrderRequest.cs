using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.DataLayer.RequestModels
{
    public class OrderRequest
    {
        public int SubscriptionId { get; set; }

        public DateTime ExpiredAt { get; set; }

        public DateTime SubscribedAt { get; set; }
    }
}
