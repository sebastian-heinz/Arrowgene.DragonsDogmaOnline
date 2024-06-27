using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Shared.Entity;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BattleContentInstantClearInfoHandler : GameRequestPacketHandler<C2SBattleContentInstantClearInfoReq, S2CBattleContentInstantClearInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BattleContentInstantClearInfoHandler));

        public BattleContentInstantClearInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBattleContentInstantClearInfoRes Handle(GameClient client, C2SBattleContentInstantClearInfoReq request)
        {
            // EntitySerializer<S2CBattleContentInfoListRes> serializer = EntitySerializer.Get<S2CBattleContentInfoListRes>();
            // S2CBattleContentInfoListRes pcap = serializer.Read(InGameDump.Dump_93.AsBuffer());

            return new S2CBattleContentInstantClearInfoRes()
            {
            };
        }
    }
}

