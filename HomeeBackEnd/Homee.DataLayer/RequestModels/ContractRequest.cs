using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.DataLayer.RequestModels
{
    public class ContractRequest
    {
        public int RoomId { get; set; }

        public long Duration { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool? Confirmed { get; set; }
    }
}
