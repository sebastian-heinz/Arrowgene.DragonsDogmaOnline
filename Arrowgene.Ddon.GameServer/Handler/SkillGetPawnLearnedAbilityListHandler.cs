using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetPawnLearnedAbilityListHandler : GameStructurePacketHandler<C2SSkillGetPawnLearnedAbilityListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetPawnLearnedAbilityListHandler));
        
        public SkillGetPawnLearnedAbilityListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillGetPawnLearnedAbilityListReq> packet)
        {
            // TODO: Move this to DB
            client.Send(new S2CSkillGetPawnLearnedAbilityListRes()
            {
                PawnId = packet.Structure.PawnId,
                SetAcquierementParam = SkillGetLearnedAbilityListHandler.AllAbilities
            });
        }
    }
}