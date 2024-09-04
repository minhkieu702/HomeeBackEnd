using System;
using System.Collections.Generic;

namespace Homee.DataLayer.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<CategoryPlace> CategoryPlaces { get; set; } = new List<CategoryPlace>();
}
