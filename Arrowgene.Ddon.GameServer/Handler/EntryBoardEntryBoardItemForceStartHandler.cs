using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp.Consumer.BlockingQueueConsumption;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EntryBoardEntryBoardItemForceStartHandler : GameRequestPacketHandler<C2SEntryBoardEntryBoardItemForceStartReq, S2CEntryBoardEntryBoardItemForceStartRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EntryBoardEntryBoardItemForceStartHandler));

        public EntryBoardEntryBoardItemForceStartHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CEntryBoardEntryBoardItemForceStartRes Handle(GameClient client, C2SEntryBoardEntryBoardItemForceStartReq request)
        {
            // var pcap = new S2CEntryBoardEntryBoardItemForceStartRes.Serializer().Read(GameFull.Dump_711.AsBuffer());

            var data = Server.BoardManager.GetGroupDataForCharacter(client.Character);

            foreach (var characterId in data.Members)
            {
                var memberClient = Server.ClientLookup.GetClientByCharacterId(characterId);
                // Allows the menu to transition
                var ntc = new S2CEntryBoardEntryBoardItemReadyNtc()
                {
                    MaxMember = data.EntryItem.Param.MaxEntryNum,
                    TimeOut = 120
                };
                memberClient.Send(ntc);
            }

            // TODO: Start a timer for 120 seconds

            return new S2CEntryBoardEntryBoardItemForceStartRes();
        }
    }
}
