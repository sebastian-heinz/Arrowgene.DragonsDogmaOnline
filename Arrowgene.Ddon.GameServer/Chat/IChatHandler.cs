using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Chat
{
    public interface IChatHandler
    {
        void Handle(GameClient client, ChatMessage message, List<ChatResponse> responses);
    }
}
