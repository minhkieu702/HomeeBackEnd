using Homee.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.DataLayer.ResponseModels
{
    public class PostResponse
    {
        public int PostId { get; set; }

        public DateTime PostedDate { get; set; }

        public string? Note { get; set; }

        public int PlaceId { get; set; }

        public int Status { get; set; }

        public bool IsBlock { get; set; }

        public virtual ICollection<FavoritePostResponse> FavoritePosts { get; set; } = [];

        public virtual ICollection<ImageResponse> Images { get; set; } = [];

        public virtual PlaceResponse Place { get; set; } = null!;
    }
}
