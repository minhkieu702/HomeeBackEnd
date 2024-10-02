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

        public int? Type { get; set; }

        public bool? IsBlock { get; set; }

        public virtual Place? Place { get; set; }

        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
