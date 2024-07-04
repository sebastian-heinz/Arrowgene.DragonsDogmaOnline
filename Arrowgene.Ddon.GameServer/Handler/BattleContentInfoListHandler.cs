using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BattleContentInfoListHandler : GameRequestPacketHandler<C2SBattleContentInfoListReq, S2CBattleContentInfoListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BattleContentInfoListHandler));

        public BattleContentInfoListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBattleContentInfoListRes Handle(GameClient client, C2SBattleContentInfoListReq request)
        {
            // client.Send(InGameDump.Dump_93);

            EntitySerializer<S2CBattleContentInfoListRes> serializer = EntitySerializer.Get<S2CBattleContentInfoListRes>();
            S2CBattleContentInfoListRes pcap = serializer.Read(InGameDump.Dump_93.AsBuffer());
            pcap.Unk0[0].Unk0.Unk9 = 4875391515;

            return pcap;
        }
    }
}
