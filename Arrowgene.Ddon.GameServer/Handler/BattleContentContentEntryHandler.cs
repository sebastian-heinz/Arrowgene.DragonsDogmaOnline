using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Shared.Entity;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BattleContentContentEntryHandler : GameRequestPacketHandler<C2SBattleContentContentEntryReq, S2CBattleContentContentEntryRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BattleContentContentEntryHandler));

        public BattleContentContentEntryHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBattleContentContentEntryRes Handle(GameClient client, C2SBattleContentContentEntryReq request)
        {
            EntitySerializer<S2CBattleContentInfoListRes> serializer = EntitySerializer.Get<S2CBattleContentInfoListRes>();
            S2CBattleContentInfoListRes pcap = serializer.Read(InGameDump.Dump_93.AsBuffer());

            S2CBattleContentContentEntryNtc ntc = new S2CBattleContentContentEntryNtc()
            {
                StageId = request.StageId,
                Unk0 = pcap.Unk0
            };
            client.Send(ntc);

            return new S2CBattleContentContentEntryRes();
        }
    }
}
