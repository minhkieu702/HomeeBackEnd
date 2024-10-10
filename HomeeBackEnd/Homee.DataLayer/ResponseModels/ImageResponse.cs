using Homee.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.DataLayer.ResponseModels
{
    public class ImageResponse
    {
        public int ImageId { get; set; }

        public string ImageUrl { get; set; } = null!;

        public int PostId { get; set; }
    }
}
