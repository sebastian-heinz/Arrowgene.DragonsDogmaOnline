using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetSetAbilityListHandler : GameRequestPacketHandler<C2SSkillGetSetAbilityListReq, S2CSkillGetSetAbilityListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetSetAbilityListHandler));

        public SkillGetSetAbilityListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSkillGetSetAbilityListRes Handle(GameClient client, C2SSkillGetSetAbilityListReq request)
        {
            return new S2CSkillGetSetAbilityListRes() {
                SetAcquierementParam = client.Character.EquippedAbilitiesDictionary[client.Character.Job]
                    .Select((x, index) => x?.AsCDataSetAcquirementParam((byte)(index+1)))
                    .Where(x => x != null)
                    .ToList()
            };
        }
    }
}
