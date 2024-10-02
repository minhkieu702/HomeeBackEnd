using System;
using System.Collections.Generic;

namespace Homee.DataLayer.Models;

public partial class Box
{
    public int BoxId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
