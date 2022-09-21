using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceSetOmInstantKeyValueHandler : StructurePacketHandler<GameClient, C2SInstanceSetOmInstantKeyValueReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceSetOmInstantKeyValueHandler));


        public InstanceSetOmInstantKeyValueHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceSetOmInstantKeyValueReq> req)
        {
            S2CInstanceSetOmInstantKeyValueRes res = new S2CInstanceSetOmInstantKeyValueRes(req.Structure);
            res.StageId = client.Character.Stage.Id;
            client.Send(res);

            S2CInstance_13_20_16_Ntc ntc = new S2CInstance_13_20_16_Ntc();
            ntc.StageId = client.Character.Stage.Id;
            ntc.Data0 = req.Structure.Data0;
            ntc.Data1 = req.Structure.Data1;
            client.Send(ntc);
        }
    }
}
