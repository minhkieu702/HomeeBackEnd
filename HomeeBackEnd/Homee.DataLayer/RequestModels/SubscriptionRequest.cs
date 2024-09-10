using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.DataLayer.RequestModels
{
    public class SubscriptionRequest
    {
        public double Price { get; set; }

        public string SubscriptionName { get; set; } = null!;

        public string? Description { get; set; }

        public long? Duration { get; set; }
    }
}
