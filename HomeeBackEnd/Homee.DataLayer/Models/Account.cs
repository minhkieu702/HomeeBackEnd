using System;
using System.Collections.Generic;

namespace Homee.DataLayer.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string CitizenId { get; set; } = null!;

    public int Role { get; set; }

    public DateOnly? BirthDay { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string? GoogleUserId { get; set; }

    public bool IsGoogleAuthenticated { get; set; }

    public bool IsBlock { get; set; }

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual ICollection<ConversationParticipant> ConversationParticipants { get; set; } = new List<ConversationParticipant>();

    public virtual ICollection<FavoritePost> FavoritePosts { get; set; } = new List<FavoritePost>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Place> Places { get; set; } = new List<Place>();
}
