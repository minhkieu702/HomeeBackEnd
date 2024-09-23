using System;
using System.Collections.Generic;

namespace Homee.DataLayer.Models;

public partial class Post
{
    public int PostId { get; set; }

    public DateTime PostedDate { get; set; }

    public string? Note { get; set; }

    public int Status { get; set; }

    public bool IsBlock { get; set; }

    public int? RoomId { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<FavoritePost> FavoritePosts { get; set; } = new List<FavoritePost>();

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual Room? Room { get; set; }
}
