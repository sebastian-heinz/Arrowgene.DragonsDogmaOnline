using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BattleContentPartyMemberInfoUpdateHandler : GameRequestPacketHandler<C2SBattleContentPartyMemberInfoUpdateReq, S2CBattleContentPartyMemberInfoUpdateRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BattleContentPartyMemberInfoUpdateHandler));

        public BattleContentPartyMemberInfoUpdateHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBattleContentPartyMemberInfoUpdateRes Handle(GameClient client, C2SBattleContentPartyMemberInfoUpdateReq request)
        {
            EntitySerializer<S2CBattleContentInfoListRes> serializer = EntitySerializer.Get<S2CBattleContentInfoListRes>();
            S2CBattleContentInfoListRes pcap = serializer.Read(InGameDump.Dump_93.AsBuffer());


            S2CBattleContentPartyMemberInfoUpdateNtc ntc = new S2CBattleContentPartyMemberInfoUpdateNtc()
            {
                CharacterId = client.Character.CharacterId,
                Pos = 0,
                BattleContentSituationData = new CDataBattleContentSituationData()
                {
                    
                },
                // Progress = item.Unk0,
                // Unk0 = item.Unk1,
                Status = true
            };
            client.Send(ntc);

            return new S2CBattleContentPartyMemberInfoUpdateRes();
        }
    }
}

