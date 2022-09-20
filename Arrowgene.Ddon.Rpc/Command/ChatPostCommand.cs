using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using static Arrowgene.Ddon.GameServer.Chat.ChatManager;

namespace Arrowgene.Ddon.Rpc.Command
{
    public class ChatPostCommand : IRpcCommand
    {
        private static readonly string DEFAULT_FIRST_NAME = "DDOn";
        private static readonly string DEFAULT_LAST_NAME = "Tools";

        public string Name => "ChatPostCommand";

        public ChatPostCommand(ChatMessageLogEntry entry)
        {
            _entry = entry;
        }

        private ChatMessageLogEntry _entry { get; set; }

        public RpcCommandResult Execute(DdonGameServer gameServer)
        {
            gameServer.ChatManager.Handle(new MockClient(_entry), _entry.ChatMessage);
            return new RpcCommandResult(this, true);
        }

        private class MockClient : IPartyMember
        {
            public MockClient(ChatMessageLogEntry entry)
            {
                Character = new Character();
                Character.Id = entry.CharacterId;
                Character.FirstName = entry.FirstName ?? DEFAULT_FIRST_NAME;
                Character.LastName = entry.LastName ?? DEFAULT_LAST_NAME;
            }

            public Character Character { get; set; }
            public Party Party { get; set; }

            public CDataPartyMember AsCDataPartyMember()
            {
                throw new System.NotImplementedException();
            }

            public Packet AsContextPacket()
            {
                throw new System.NotImplementedException();
            }

            public void Send(Packet packet)
            {
                // Do nothing
            }

            void IPartyMember.Send<TResStruct>(TResStruct res)
            {
                // Do nothing
            }
        }
    }
}