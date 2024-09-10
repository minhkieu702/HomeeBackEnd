using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.DataLayer.RequestModels
{
    public class ImageRequest
    {
        public string ImageUrl { get; set; } = null!;

        public int PostId { get; set; }
    }
}
