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

        public double Area { get; set; }

        public int Direction { get; set; }

        public int NumberOfToilet { get; set; }

        public int NumberOfBedroom { get; set; }

        public double Rent { get; set; }

        public List<int> Categories { get; set; } = [];

        public DateTime PostedDate { get; set; }

        public string? Note { get; set; }

        public int PlaceId { get; set; }

        public int Status { get; set; }

        public List<string> ImageUrls { get; set; } = [];
    }
}
