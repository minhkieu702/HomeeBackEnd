using System;
using System.Collections.Generic;

namespace Homee.DataLayer.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int AccountId { get; set; }

    public int Type { get; set; }

    public string Content { get; set; } = null!;

    public string? UrlPath { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Account Account { get; set; } = null!;
}
