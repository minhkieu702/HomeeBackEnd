using Homee.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.DataLayer.ResponseModels
{
    public class InteriorResponse
    {
        public int InteriorId { get; set; }

        public string InteriorName { get; set; } = null!;

        public int Status { get; set; }

        public string? Description { get; set; }

        public int PlaceId { get; set; }

        public virtual Place Place { get; set; } = null!;
    }
}
