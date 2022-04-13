using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceExchangeOmInstantKeyValueHandler : StructurePacketHandler<GameClient, C2SInstanceExchangeOmInstantKeyValueReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceExchangeOmInstantKeyValueHandler));


        public InstanceExchangeOmInstantKeyValueHandler(DdonGameServer server) : base(server)
        {
        }


        public override void Handle(GameClient client, StructurePacket<C2SInstanceExchangeOmInstantKeyValueReq> req)
        {
            S2CInstanceExchangeOmInstantKeyValueRes res = new S2CInstanceExchangeOmInstantKeyValueRes();
            res.StageId = client.Stage.Id;
            res.Data0 = req.Structure.Data0;
            res.Data1 = req.Structure.Data1;
            client.Send(res);

            S2CInstance_13_23_16_Ntc ntc = new S2CInstance_13_23_16_Ntc();
            ntc.StageId = client.Stage.Id;
            ntc.Data0 = req.Structure.Data0;
            ntc.Data1 = req.Structure.Data1;
            client.Send(ntc);
        }
    }
}
