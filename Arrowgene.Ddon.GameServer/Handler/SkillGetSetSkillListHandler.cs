using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetSetSkillListHandler : GameRequestPacketHandler<C2SSkillGetSetSkillListReq, S2CSkillGetSetSkillListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetSetSkillListHandler));

        public SkillGetSetSkillListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSkillGetSetSkillListRes Handle(GameClient client, C2SSkillGetSetSkillListReq request)
        {
            return new S2CSkillGetSetSkillListRes() {
                SetAcquierementParam = client.Character.EquippedCustomSkillsDictionary[client.Character.Job]
                    .Select((x, index) => x?.AsCDataSetAcquirementParam((byte)(index+1)))
                    .Where(x => x != null)
                    .ToList()
            };
        }
    }
}
