using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.DataLayer.RequestModels
{
    public class PlacePostRequest
    {
        public string Province { get; set; } = null!;

        public string Distinct { get; set; } = null!;

        public string Ward { get; set; } = null!;

        public string Street { get; set; } = null!;

        public string Number { get; set; } = null!;

        public string? RoomName { get; set; }

        public int? Direction { get; set; }

        public decimal? Area { get; set; }

        public int? InteriorStatus { get; set; }

        public bool? RentalStatus { get; set; }

        public decimal? RentAmount { get; set; }

        public decimal? WaterAmount { get; set; }

        public decimal? ElectricAmount { get; set; }

        public decimal? ServiceAmount { get; set; }

        public DateTime PostedDate { get; set; }

        public string? Note { get; set; }

        public string? Title { get; set; }

        public List<int> Categories { get; set; } = [];

        public int Status { get; set; }

        public List<ImageRequest> ImageUrls { get; set; } = [];
    }
}
