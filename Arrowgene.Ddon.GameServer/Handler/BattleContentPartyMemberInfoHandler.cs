using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BattleContentPartyMemberInfoHandler : GameRequestPacketHandler<C2SBattleContentPartyMemberInfoReq, S2CBattleContentPartyMemberInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BattleContentPartyMemberInfoHandler));

        public BattleContentPartyMemberInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBattleContentPartyMemberInfoRes Handle(GameClient client, C2SBattleContentPartyMemberInfoReq request)
        {

            EntitySerializer<S2CBattleContentInfoListRes> serializer = EntitySerializer.Get<S2CBattleContentInfoListRes>();
            S2CBattleContentInfoListRes pcap = serializer.Read(InGameDump.Dump_93.AsBuffer());

            return new S2CBattleContentPartyMemberInfoRes()
            {
                Unk0 = new List<CDataBattleContentUnk6>
                {
                    new CDataBattleContentUnk6()
                    {
                        Unk2 = pcap.Unk0[0].Unk0
                    }
                },
                Unk1 = false
            };

            return new S2CBattleContentPartyMemberInfoRes();
        }
    }
}

