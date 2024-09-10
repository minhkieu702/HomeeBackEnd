using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.DataLayer.RequestModels
{
    public class PostRequest
    {

        public DateTime PostedDate { get; set; }

        public string? Note { get; set; }

        public int PlaceId { get; set; }

        public int Status { get; set; }

        public bool IsBlock { get; set; }
        public List<string> ImageUrls { get; set; } = [];
    }
}
