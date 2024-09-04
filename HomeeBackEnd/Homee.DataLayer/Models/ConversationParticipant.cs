using System;
using System.Collections.Generic;

namespace Homee.DataLayer.Models;

public partial class ConversationParticipant
{
    public int ConversationParticipantId { get; set; }

    public int AccountId { get; set; }

    public int ConversationId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Conversation Conversation { get; set; } = null!;
}
