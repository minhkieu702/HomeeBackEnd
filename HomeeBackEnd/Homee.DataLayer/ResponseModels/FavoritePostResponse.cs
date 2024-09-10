using Homee.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.DataLayer.ResponseModels
{
    public class FavoritePostResponse
    {
        public int AccountId { get; set; }

        public int PostId { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual AccountResponse Account { get; set; } = null!;

        public virtual PostResponse Post { get; set; } = null!;
    }
}
