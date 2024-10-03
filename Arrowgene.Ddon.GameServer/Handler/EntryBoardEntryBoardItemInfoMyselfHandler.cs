using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EntryBoardEntryBoardItemInfoMyselfHandler : GameRequestPacketHandler<C2SEntryBoardEntryBoardItemInfoMyselfReq, S2CEntryBoardEntryBoardItemInfoMyselfRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EntryBoardEntryBoardItemInfoMyselfHandler));

        public EntryBoardEntryBoardItemInfoMyselfHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CEntryBoardEntryBoardItemInfoMyselfRes Handle(GameClient client, C2SEntryBoardEntryBoardItemInfoMyselfReq request)
        {
            // var pcap = new S2CEntryBoardEntryBoardItemInfoMyselfRes.Serializer().Read(GameFull.Dump_712.AsBuffer());

            var data = Server.BoardManager.GetGroupDataForCharacter(client.Character);
            var result = new S2CEntryBoardEntryBoardItemInfoMyselfRes()
            {
                BoardId = data.BoardId,
                EntryItem = data.EntryItem
            };

            return result;
        }
    }
}
