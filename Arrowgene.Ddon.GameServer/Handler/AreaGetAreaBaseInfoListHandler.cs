using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class AreaGetAreaBaseInfoListHandler : GameRequestPacketHandler<C2SAreaGetAreaBaseInfoListReq, S2CAreaGetAreaBaseInfoListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(AreaGetAreaBaseInfoListHandler));


        public AreaGetAreaBaseInfoListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CAreaGetAreaBaseInfoListRes Handle(GameClient client, C2SAreaGetAreaBaseInfoListReq request)
        {
            // client.Send(InGameDump.Dump_58);
            S2CAreaGetAreaBaseInfoListRes pcap = EntitySerializer.Get<S2CAreaGetAreaBaseInfoListRes>().Read(InGameDump.data_Dump_58);
            foreach (var areaBaseInfo in pcap.AreaBaseInfoList)
            {
                areaBaseInfo.Rank = 15;
                areaBaseInfo.CanRankUp = false;
                areaBaseInfo.ClanAreaPoint = 0;
            }
            
            return pcap;
        }
    }
}
