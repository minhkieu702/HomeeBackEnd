using Homee.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.DataLayer.ResponseModels
{
    public class CategoryPlaceResponse
    {
        public int CategoryPlaceId { get; set; }

        public int CategoryId { get; set; }

        public int PlaceId { get; set; }

        public virtual CategoryResponse Category { get; set; } = null!;

        public virtual PlaceResponse Place { get; set; } = null!;
    }
}
