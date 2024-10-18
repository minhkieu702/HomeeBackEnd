using Homee.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.DataLayer.ResponseModels
{
    public class RoomResponse
    {
        public int RoomId { get; set; }

        public string? RoomName { get; set; }

        public string? Direction { get; set; }

        public decimal? Area { get; set; }

        public string? InteriorStatus { get; set; }

        public bool? IsRented { get; set; }

        public decimal? RentAmount { get; set; }

        public decimal? WaterAmount { get; set; }

        public decimal? ElectricAmount { get; set; }

        public decimal? ServiceAmount { get; set; }

        public int? PlaceId { get; set; }

        public string? Type { get; set; }

        public bool? IsBlock { get; set; }

        public int? RestRoom { get; set; }

        public int? BedRoom { get; set; }

        public virtual PlaceResponse? Place { get; set; }

        public virtual ICollection<PostResponse> Posts { get; set; } = [];
    }
}
