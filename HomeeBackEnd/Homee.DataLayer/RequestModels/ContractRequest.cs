using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.DataLayer.RequestModels
{
    public class ContractRequest
    {
        public int RenderId { get; set; }

        public int PlaceId { get; set; }

        public long Duration { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
