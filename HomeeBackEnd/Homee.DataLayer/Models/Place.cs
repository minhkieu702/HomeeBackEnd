using System;
using System.Collections.Generic;

namespace Homee.DataLayer.Models;

public partial class Place
{
    public int PlaceId { get; set; }

    public string Province { get; set; } = null!;

    public string Distinct { get; set; } = null!;

    public string Ward { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string Number { get; set; } = null!;

    public double Area { get; set; }

    public int Direction { get; set; }

    public int NumberOfToilet { get; set; }

    public int NumberOfBedroom { get; set; }

    public double Rent { get; set; }

    public int OwnerId { get; set; }

    public bool IsBlock { get; set; }

    public virtual ICollection<CategoryPlace> CategoryPlaces { get; set; } = new List<CategoryPlace>();

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual ICollection<Interior> Interiors { get; set; } = new List<Interior>();

    public virtual Account Owner { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
