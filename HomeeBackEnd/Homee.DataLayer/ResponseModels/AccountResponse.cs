using Homee.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.DataLayer.ResponseModels
{
    public class AccountResponse
    {
    public int AccountId { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string CitizenId { get; set; } = null!;

        public int Role { get; set; }

        public DateTime? BirthDay { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string? GoogleUserId { get; set; }

        public bool IsGoogleAuthenticated { get; set; }

        public bool IsBlock { get; set; }
        
        public OrderResponse? LastOrder { get; set; }

        public virtual ICollection<NotificationResponse> Notifications { get; set; } = [];

        public virtual ICollection<OrderResponse> Orders { get; set; } = [];
    }
}
