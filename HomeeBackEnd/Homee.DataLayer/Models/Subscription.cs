using System;
using System.Collections.Generic;

namespace Homee.DataLayer.Models;

public partial class Subscription
{
    public int SubscriptionId { get; set; }

    public double Price { get; set; }

    public string SubscriptionName { get; set; } = null!;

    public string? Description { get; set; }

    public long? Duration { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
