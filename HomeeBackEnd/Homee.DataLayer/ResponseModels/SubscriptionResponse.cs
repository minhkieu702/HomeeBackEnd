using Homee.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.DataLayer.ResponseModels
{
    public class SubscriptionResponse
    {
        public int SubscriptionId { get; set; }

        public double Price { get; set; }

        public string SubscriptionName { get; set; } = null!;

        public string? Description { get; set; }

        public long? Duration { get; set; }

        public virtual ICollection<OrderResponse> Orders { get; set; } = [];
    }
}
