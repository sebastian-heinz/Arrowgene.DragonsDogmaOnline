using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class AreaGetAreaBaseInfoListHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(AreaGetAreaBaseInfoListHandler));


        public AreaGetAreaBaseInfoListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_AREA_GET_AREA_BASE_INFO_LIST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            // client.Send(InGameDump.Dump_58);
            S2CAreaGetAreaBaseInfoListRes pcap = EntitySerializer.Get<S2CAreaGetAreaBaseInfoListRes>().Read(InGameDump.data_Dump_58);
            foreach (var areaBaseInfo in pcap.AreaBaseInfoList)
            {
                areaBaseInfo.Rank = 15;
                areaBaseInfo.CanRankUp = false;
                areaBaseInfo.ClanAreaPoint = 0;
            }
            client.Send(pcap);
        }
    }
}
