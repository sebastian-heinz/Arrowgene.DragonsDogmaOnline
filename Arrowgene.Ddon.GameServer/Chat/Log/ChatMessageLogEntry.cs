using System;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Chat.Log;

public class ChatMessageLogEntry
{
    // For the JSON deserializer
    public ChatMessageLogEntry()
    {
    }

    public ChatMessageLogEntry(Character character, ChatMessage chatMessage)
    {
        DateTime = DateTime.UtcNow;
        FirstName = character.FirstName;
        LastName = character.LastName;
        CharacterId = character.CharacterId;
        ChatMessage = chatMessage;
    }

    public DateTime DateTime { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public uint CharacterId { get; set; }
    public ChatMessage ChatMessage { get; set; }
}
