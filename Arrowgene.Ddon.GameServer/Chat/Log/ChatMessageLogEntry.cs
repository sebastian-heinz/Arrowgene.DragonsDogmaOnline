using System;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Chat.Log;

public class ChatMessageLogEntry
{
    // For the JSON deserializer
    public ChatMessageLogEntry()
    {
    }

    public ChatMessageLogEntry(uint characterId, string firstName, string lastName, ChatMessage chatMessage)
    {
        DateTime = DateTime.UtcNow;
        FirstName = firstName;
        LastName = lastName;
        CharacterId = characterId;
        ChatMessage = chatMessage;
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
    public long UnixTimeMillis { get => ((DateTimeOffset) DateTime.SpecifyKind(this.DateTime, DateTimeKind.Utc)).ToUnixTimeMilliseconds(); }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public uint CharacterId { get; set; }
    public ChatMessage ChatMessage { get; set; }
}
