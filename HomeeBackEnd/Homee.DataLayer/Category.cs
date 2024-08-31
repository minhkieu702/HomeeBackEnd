﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Homee.DataLayer;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;
    
    public virtual ICollection<Place> Places { get; set; } = new List<Place>();
}
