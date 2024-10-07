using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Homee.DataLayer.Models;

public partial class Image
{
    [JsonIgnore]
    public int ImageId { get; set; }

    public string ImageUrl { get; set; } = null!;
    [JsonIgnore]
    public int PostId { get; set; }

    public int? No { get; set; }
    [JsonIgnore]
    public virtual Post Post { get; set; } = null!;
}
