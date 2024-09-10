using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class Instance_13_46_16_Handler : GameStructurePacketHandler<C2SInstance_13_46_16_Ntc>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(Instance_13_46_16_Handler));

        public Instance_13_46_16_Handler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstance_13_46_16_Ntc> packet)
        {
            Logger.Debug($"{packet.Structure.Unk0}.{packet.Structure.Unk1}.{packet.Structure.Unk2}.{packet.Structure.Unk3}.{packet.Structure.Unk4}.{packet.Structure.Unk5}");
            // TODO: Identify this packet.
        }
    }
}
