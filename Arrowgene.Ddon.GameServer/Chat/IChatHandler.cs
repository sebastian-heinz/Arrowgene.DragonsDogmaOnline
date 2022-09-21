using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Chat
{
    public interface IChatHandler
    {
        void Handle(IPartyMember client, ChatMessage message, List<ChatResponse> responses);
    }
}
