using System;
using System.Collections.Generic;

namespace Homee.DataLayer.Models;

public partial class Order
{
    public long OrderId { get; set; }

    public int? SubscriptionId { get; set; }

    public int? OwnerId { get; set; }

    public DateTime? ExpiredAt { get; set; }

    public DateTime? SubscribedAt { get; set; }

    public string? PaymentId { get; set; }

    public virtual Account? Owner { get; set; }

    public virtual Subscription? Subscription { get; set; }
}
