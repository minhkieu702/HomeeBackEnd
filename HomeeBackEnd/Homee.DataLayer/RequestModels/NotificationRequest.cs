using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.DataLayer.RequestModels
{
    public class NotificationRequest
    {
        public int Type { get; set; }

        public string Content { get; set; } = null!;

        public string? UrlPath { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
