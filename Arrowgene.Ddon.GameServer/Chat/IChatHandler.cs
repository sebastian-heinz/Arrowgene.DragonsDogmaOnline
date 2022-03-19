using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Chat
{
    public interface IChatHandler
    {
        void Handle(GameClient client, ChatMessage message, List<ChatResponse> responses);
    }
}
