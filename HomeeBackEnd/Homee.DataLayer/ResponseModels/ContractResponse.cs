using Homee.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.DataLayer.ResponseModels
{
    public class ContractResponse
    {
        public int ContractId { get; set; }

        public int? RoomId { get; set; }

        public int? RenterId { get; set; }

        public DateTime CreatedAt { get; set; }

        public long? Duration { get; set; }

        public bool? Confirmed { get; set; }

        public virtual RoomResponse Room { get; set; } = null!;

        public virtual AccountResponse Renter { get; set; } = null!;
    }
}
