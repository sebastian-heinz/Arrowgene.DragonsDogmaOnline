using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class Context_35_5_16_Handler : GameStructurePacketHandler<C2SContext_35_5_16_Ntc>
    {
        public Context_35_5_16_Handler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SContext_35_5_16_Ntc> packet)
        {
            // This notice is sent when the player falls in brine-infested water (red aura/tendrils that warp you back to shore).
            // If you jump off a ledge that has no water, this message does not get sent.
            // It also gets sent when mudman debuff you and your lantern turns off.
            // If you fall in and past the trigger plane, you usually only get one notice.
            // If it's a walkoff like a beach, you can stand in the water and send like one NTC a frame.

            Server.ItemManager.StopLantern(client, true).Send();
        }
    }
}
