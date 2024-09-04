using System;
using System.Collections.Generic;

namespace Homee.DataLayer.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int SubscriptionId { get; set; }

    public int OwnerId { get; set; }

    public DateTime ExpiredAt { get; set; }

    public DateTime SubscribedAt { get; set; }

    public virtual Account Owner { get; set; } = null!;

    public virtual Subscription Subscription { get; set; } = null!;
}
