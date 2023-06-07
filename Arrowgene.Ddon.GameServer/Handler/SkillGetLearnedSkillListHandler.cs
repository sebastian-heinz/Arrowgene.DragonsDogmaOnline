using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetLearnedSkillListHandler : StructurePacketHandler<GameClient, C2SSkillGetLearnedSkillListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetLearnedSkillListHandler));

        public SkillGetLearnedSkillListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillGetLearnedSkillListReq> packet)
        {
            // TODO: Move this to DB
            client.Send(new S2CSkillGetLearnedSkillListRes() {
                SetAcquierementParam = SkillGetAcquirableSkillListHandler.AllSkills
                    .Select(skill => new CDataLearnedSetAcquirementParam() {
                        Job = skill.Job,
                        Type = skill.Type,
                        AcquirementNo = skill.SkillNo,
                        AcquirementLv = (byte) (IsSkillEX(skill.SkillNo) ? 1 : 10) // EX skills must be Lv 1 to work, otherwise use Lv 10 (Max level)
                    }).ToList()
            });
        }

        public static bool IsSkillEX(uint skillNo)
        {
            return skillNo >= 100;
        }
    }
}