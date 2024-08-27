using System;
using System.Collections.Generic;

namespace Homee.DataLayer;

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

    public int Status { get; set; }

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual ICollection<Interior> Interiors { get; set; } = new List<Interior>();

    public virtual Account Owner { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
