using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ContextGetSetContextHandler : StructurePacketHandler<GameClient, C2SContextGetSetContextReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ContextGetSetContextHandler));


        public ContextGetSetContextHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SContextGetSetContextReq> packet)
        {
            S2CContextGetSetContextRes res = new S2CContextGetSetContextRes();
            client.Send(res);

            CData_35_14_16 ntcData = new CData_35_14_16();
            ntcData.UniqueId = packet.Structure.UniqueId;
            ntcData.Unk0 = 0;

            S2CContext_35_14_16_Ntc ntc = new S2CContext_35_14_16_Ntc();
            ntc.Unk0.Add(ntcData);

            client.Send(ntc);
        }
    }
}
