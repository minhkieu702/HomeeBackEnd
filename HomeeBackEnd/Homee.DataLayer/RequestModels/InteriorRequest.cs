using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.DataLayer.RequestModels
{
    public class InteriorRequest
    {
        public string InteriorName { get; set; } = null!;

        public int Status { get; set; }

        public string? Description { get; set; }

        public int PlaceId { get; set; }
    }
}
