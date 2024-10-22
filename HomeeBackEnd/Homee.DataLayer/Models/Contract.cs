using System;
using System.Collections.Generic;

namespace Homee.DataLayer.Models;

public partial class Contract
{
    public int ContractId { get; set; }

    public int? RoomId { get; set; }

    public int? RenterId { get; set; }

    public DateTime? CreateAt { get; set; }

    public long? Duration { get; set; }

    public bool? Confirmed { get; set; }

    public virtual Account? Renter { get; set; }

    public virtual Room? Room { get; set; }
}
