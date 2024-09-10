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

        public int RenderId { get; set; }

        public int PlaceId { get; set; }

        public DateTime CreatedAt { get; set; }

        public long? Duration { get; set; }

        public virtual PlaceResponse Place { get; set; } = null!;

        public virtual AccountResponse Render { get; set; } = null!;
    }
}
