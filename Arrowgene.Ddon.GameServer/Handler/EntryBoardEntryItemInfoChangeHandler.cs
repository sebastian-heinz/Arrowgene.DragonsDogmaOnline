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
            var data = Server.BoardManager.GetGroupDataForCharacter(client.Character);
            data.Password = request.Password;
            data.EntryItem.Param = request.Param;

            var ntc = new S2CEntryBoardEntryBoardItemInfoChangeNtc()
            {
                BoardId = data.BoardId,
                EntryItemData = data.EntryItem
            };

            foreach (var characterId in data.Members)
            {
                var memberClient = Server.ClientLookup.GetClientByCharacterId(characterId);
                memberClient.Send(ntc);
            }
            
            return new S2CEntryBoardEntryBoardItemInfoChangeRes()
            {
                EntryItemData = data.EntryItem
            };
        }
    }
}
