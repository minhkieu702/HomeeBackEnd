using System;
using System.Collections.Generic;

namespace Homee.DataLayer;

public partial class Subscription
{
    public int SubscriptionId { get; set; }

    public double Price { get; set; }

    public TimeOnly Duration { get; set; }

    public string SubscriptionName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
