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

    public int OwnerId { get; set; }

    public bool IsBlock { get; set; }

    public virtual Account Owner { get; set; } = null!;

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
