using System;
using System.Collections.Generic;

namespace Homee.DataLayer.Models;

public partial class Image
{
    public int ImageId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public int PostId { get; set; }

    public int? No { get; set; }

    public virtual Post Post { get; set; } = null!;
}
