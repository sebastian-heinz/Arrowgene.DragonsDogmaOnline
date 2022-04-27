using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
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

            // We believe it may be telling the client to load a persistent context.
            // If it's not sent, it will load a new context.
            // Sending S2CInstance_13_42_16_Ntc resets it (Like its done in StageAreaChangeHandler)
            S2CContextSetContextBaseNtc baseNtc = new S2CContextSetContextBaseNtc();
            baseNtc.ContextId = packet.Structure.ContextId;
            baseNtc.UniqueId = packet.Structure.UniqueId;
            baseNtc.StageNo = packet.Structure.StageNo;
            baseNtc.EncountArea = packet.Structure.EncountArea;
            baseNtc.MasterIndex = packet.Structure.MasterIndex;
            baseNtc.Unk0 = packet.Structure.Unk0;
            client.Send(baseNtc);
        }
    }
}
