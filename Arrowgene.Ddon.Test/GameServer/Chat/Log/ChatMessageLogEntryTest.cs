using System.Text.Json;
using Arrowgene.Ddon.GameServer.Chat;
using Arrowgene.Ddon.GameServer.Chat.Log;
using Arrowgene.Ddon.Shared.Model;
using Xunit;

namespace Arrowgene.Ddon.Test.GameServer.Chat.Log;

public class ChatMessageLogEntryTest
{
    [Fact]
    public void TestJsonSerialize()
    {
        ChatMessageLogEntry obj = new ChatMessageLogEntry();
        obj.ChatMessage = new ChatMessage();
        obj.ChatMessage.Type = LobbyChatMsgType.Party;
        string json = JsonSerializer.Serialize(obj);
        ChatMessageLogEntry res = JsonSerializer.Deserialize<ChatMessageLogEntry>(json);
        Assert.NotNull(res);
        Assert.Equal(obj.ChatMessage.Type, res.ChatMessage.Type);
    }
}
