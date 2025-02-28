﻿using System;
using System.Collections.Generic;

namespace Homee.DataLayer.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public string? RoomName { get; set; }

    public int? Direction { get; set; }

    public decimal? Area { get; set; }

    public int? InteriorStatus { get; set; }

    public bool? IsRented { get; set; }

    public decimal? RentAmount { get; set; }

    public decimal? WaterAmount { get; set; }

    public decimal? ElectricAmount { get; set; }

    public decimal? ServiceAmount { get; set; }

    public int? PlaceId { get; set; }

    public int? Type { get; set; }

    public bool? IsBlock { get; set; }

    public int? RestRoom { get; set; }

    public int? BedRoom { get; set; }

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual Place? Place { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
