using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
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
            CDataSetAcquirementParam skillSlot = client.Character.CustomSkills
                .Where(skill => skill.Job == packet.Structure.Job && skill.SlotNo == packet.Structure.SlotNo)
                .FirstOrDefault();
            
            if(skillSlot == null)
            {
                skillSlot = new CDataSetAcquirementParam()
                {
                    Job = packet.Structure.Job,
                    SlotNo = packet.Structure.SlotNo
                };
                client.Character.CustomSkills.Add(skillSlot);
            }

            skillSlot.AcquirementNo = packet.Structure.SkillId;
            skillSlot.AcquirementLv = packet.Structure.SkillLv;

            Database.UpdateCharacter(client.Character);

            client.Send(new S2CSkillSetSkillRes() {
                Job = packet.Structure.Job,
                SlotNo = packet.Structure.SlotNo,
                SkillId = packet.Structure.SkillId,
                SkillLv = packet.Structure.SkillLv
            });

            // Inform party members of the change
            // There's probably a different, smaller packet precisely for this purpose (S2C_CUSTOM_SKILL_SET_NTC?)
            S2CContextGetPartyPlayerContextNtc partyPlayerContextNtc = new S2CContextGetPartyPlayerContextNtc(client.Character);
            partyPlayerContextNtc.Context.Base.MemberIndex = client.Party.Members.IndexOf(client);
            client.Party.SendToAll(partyPlayerContextNtc);
        }
    }
}