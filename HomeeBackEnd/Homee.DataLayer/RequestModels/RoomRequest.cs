using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.DataLayer.RequestModels
{
    public class RoomRequest
    {
        public string? RoomName { get; set; }

        public int? Direction { get; set; }

        public decimal? Area { get; set; }

        public int? InteriorStatus { get; set; }
        public int Type { get; set; }
        public bool? IsRented { get; set; }

        public decimal? RentAmount { get; set; }

        public decimal? WaterAmount { get; set; }

        public decimal? ElectricAmount { get; set; }

        public decimal? ServiceAmount { get; set; }

        public int PlaceId { get; set; }
    }
}
