using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillSetSkillHandler : StructurePacketHandler<GameClient, C2SSkillSetSkillReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillSetSkillHandler));

        public SkillSetSkillHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillSetSkillReq> packet)
        {
            // TODO: Check in DB if the skill is unlocked and it's leveled up to what the packet says
            
            CustomSkill skillSlot = client.Character.CustomSkills
                .Where(skill => skill.Job == packet.Structure.Job && skill.SlotNo == packet.Structure.SlotNo)
                .FirstOrDefault();
            
            if(skillSlot == null)
            {
                skillSlot = new CustomSkill()
                {
                    Job = packet.Structure.Job,
                    SlotNo = packet.Structure.SlotNo,
                    SkillId = packet.Structure.SkillId,
                    SkillLv = packet.Structure.SkillLv
                };
                client.Character.CustomSkills.Add(skillSlot);
            }
            else
            {
                skillSlot.SkillId = packet.Structure.SkillId;
                skillSlot.SkillLv = packet.Structure.SkillLv;
            }

            Database.ReplaceEquippedCustomSkill(client.Character.CommonId, skillSlot);

            client.Send(new S2CSkillSetSkillRes() {
                Job = skillSlot.Job,
                SlotNo = skillSlot.SlotNo,
                SkillId = skillSlot.SkillId,
                SkillLv = skillSlot.SkillLv
            });

            // Inform party members of the change
            if(packet.Structure.Job == client.Character.Job)
            {
                client.Party.SendToAll(new S2CSkillCustomSkillSetNtc()
                {
                    CharacterId = client.Character.CharacterId,
                    ContextAcquirementData = new CDataContextAcquirementData()
                    {
                        SlotNo = skillSlot.SlotNo,
                        AcquirementNo = skillSlot.SkillId,
                        AcquirementLv = skillSlot.SkillLv
                    }
                });
            }
        }
    }
}