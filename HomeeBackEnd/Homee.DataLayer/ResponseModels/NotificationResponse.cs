using Homee.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.DataLayer.ResponseModels
{
    public class NotificationResponse
    {
        public int AccountId { get; set; }

        public int Type { get; set; }

        public string Content { get; set; } = null!;

        public string? UrlPath { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual AccountResponse Account { get; set; } = null!;
    }
}
