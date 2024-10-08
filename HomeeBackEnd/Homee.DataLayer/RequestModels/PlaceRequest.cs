using Homee.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Homee.DataLayer.RequestModels
{
    public class PlaceRequest
    {
        public string Province { get; set; } = null!;

        public string Distinct { get; set; } = null!;

        public string Ward { get; set; } = null!;

        public string Street { get; set; } = null!;

        public string Number { get; set; } = null!;
    }
}
