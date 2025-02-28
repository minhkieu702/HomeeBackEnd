﻿using Homee.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.DataLayer.ResponseModels
{
    public class OrderResponse
    {
        public long OrderId { get; set; }

        public int SubscriptionId { get; set; }

        public int OwnerId { get; set; }

        public DateTime ExpiredAt { get; set; }

        public DateTime SubscribedAt { get; set; }

        public virtual AccountResponse Owner { get; set; } = null!;

        public virtual SubscriptionResponse Subscription { get; set; } = null!;
    }
}
