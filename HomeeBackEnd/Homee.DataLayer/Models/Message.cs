using System;
using System.Collections.Generic;

namespace Homee.DataLayer.Models;

public partial class Message
{
    public int MessageId { get; set; }

    public int? BoxId { get; set; }

    public int? AccountId { get; set; }

    public string? Content { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Box? Box { get; set; }
}
