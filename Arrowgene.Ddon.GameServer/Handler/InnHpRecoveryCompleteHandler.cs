using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InnHpRecoveryCompleteHandler : GameStructurePacketHandler<C2SInnHpRecoveryCompleteNtc>
    {
        public InnHpRecoveryCompleteHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInnHpRecoveryCompleteNtc> packet)
        {
            client.Character.WhiteHp = uint.MaxValue;
            client.Character.GreenHp = uint.MaxValue;
        }
    }
}
