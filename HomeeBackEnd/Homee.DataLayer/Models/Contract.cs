using System;
using System.Collections.Generic;

namespace Homee.DataLayer.Models;

public partial class Contract
{
    public int ContractId { get; set; }

    public int RenderId { get; set; }

    public int PlaceId { get; set; }

    public DateTime CreatedAt { get; set; }

    public long? Duration { get; set; }

    public virtual Place Place { get; set; } = null!;

    public virtual Account Render { get; set; } = null!;
}
