using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EntryBoardEntryItemInfoChangeHandler : GameRequestPacketHandler<C2SEntryBoardEntryBoardItemInfoChangeReq, S2CEntryBoardEntryBoardItemInfoChangeRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EntryBoardEntryItemInfoChangeHandler));


        public EntryBoardEntryItemInfoChangeHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CEntryBoardEntryBoardItemInfoChangeRes Handle(GameClient client, C2SEntryBoardEntryBoardItemInfoChangeReq request)
        {
            var data = Server.ExmManager.GetEntryItemDataForCharacter(client.Character);
            data.Param = request.Param;
            // TODO: How to save password?
            // request.Password

            var ntc = new S2CEntryBoardEntryBoardItemInfoChangeNtc()
            {
                BoardId = Server.ExmManager.GetContentIdForCharacter(client.Character),
                EntryItemData = data
            };
            // TODO: Does this need to be sent to everyone in the server?
            client.Party.SendToAllExcept(ntc, client);

            return new S2CEntryBoardEntryBoardItemInfoChangeRes()
            {
                EntryItemData = data
            };
        }
    }
}
