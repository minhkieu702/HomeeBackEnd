using System;
using System.Collections.Generic;

namespace Homee.DataLayer;

public partial class Conversation
{
    public int ConversationId { get; set; }

    public DateTime CreatedAt { get; set; }

    public int LastMessageId { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
