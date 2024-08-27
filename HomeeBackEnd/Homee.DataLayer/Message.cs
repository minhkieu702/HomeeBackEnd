using System;
using System.Collections.Generic;

namespace Homee.DataLayer;

public partial class Message
{
    public int MessageId { get; set; }

    public int ConversationId { get; set; }

    public int SenderId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public virtual Conversation Conversation { get; set; } = null!;

    public virtual Account Sender { get; set; } = null!;
}
