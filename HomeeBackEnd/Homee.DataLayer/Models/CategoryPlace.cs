using System;
using System.Collections.Generic;

namespace Homee.DataLayer.Models;

public partial class CategoryPlace
{
    public int CategoryPlaceId { get; set; }

    public int CategoryId { get; set; }

    public int PlaceId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Place Place { get; set; } = null!;
}
