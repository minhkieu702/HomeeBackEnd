using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.DataLayer.RequestModels
{
    public class FavoritePostRequest
    {
        public int PostId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
