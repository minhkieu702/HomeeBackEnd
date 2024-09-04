using System;
using System.Collections.Generic;

namespace Homee.DataLayer.Models;

public partial class Interior
{
    public int InteriorId { get; set; }

    public string InteriorName { get; set; } = null!;

    public int Status { get; set; }

    public string? Description { get; set; }

    public int PlaceId { get; set; }

    public virtual Place Place { get; set; } = null!;
}
