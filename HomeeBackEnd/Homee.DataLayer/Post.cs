using System;
using System.Collections.Generic;

namespace Homee.DataLayer;

public partial class Post
{
    public int PostId { get; set; }

    public DateTime PostedDate { get; set; }

    public string? Note { get; set; }

    public int PlaceId { get; set; }

    public int Status { get; set; }

    public bool IsBlock { get; set; }

    public int? StaffId { get; set; }

    public virtual ICollection<FavoritePost> FavoritePosts { get; set; } = new List<FavoritePost>();

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual Place Place { get; set; } = null!;

    public virtual Account? Staff { get; set; }
}
