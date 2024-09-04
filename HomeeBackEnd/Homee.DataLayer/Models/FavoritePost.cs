using System;
using System.Collections.Generic;

namespace Homee.DataLayer.Models;

public partial class FavoritePost
{
    public int AccountId { get; set; }

    public int PostId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Post Post { get; set; } = null!;
}
