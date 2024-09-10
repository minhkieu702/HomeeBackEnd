using Homee.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.DataLayer.ResponseModels
{
    public class PlaceResponse
    {
        public int PlaceId { get; set; }

        public string Province { get; set; } = null!;

        public string Distinct { get; set; } = null!;

        public string Ward { get; set; } = null!;

        public string Street { get; set; } = null!;

        public string Number { get; set; } = null!;

        public double Area { get; set; }

        public string  Direction { get; set; }

        public int NumberOfToilet { get; set; }

        public int NumberOfBedroom { get; set; }

        public double Rent { get; set; }

        public int OwnerId { get; set; }

        public bool? IsBlock { get; set; }

        public virtual ICollection<InteriorResponse> Interiors { get; set; } = new List<InteriorResponse>();

        public virtual AccountResponse Owner { get; set; } = null!;

        public virtual ICollection<CategoryPlaceResponse> CategoryPlaces { get; set; } = new List<CategoryPlaceResponse>();
    }
}
