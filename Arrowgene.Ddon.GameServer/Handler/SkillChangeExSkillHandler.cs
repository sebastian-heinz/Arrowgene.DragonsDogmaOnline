using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Linq;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.GameServer.Characters;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillChangeExSkillHandler : StructurePacketHandler<GameClient, C2SSkillChangeExSkillReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillChangeExSkillHandler));

        private readonly JobManager jobManager;

        public SkillChangeExSkillHandler(DdonGameServer server) : base(server)
        {
            jobManager = server.JobManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillChangeExSkillReq> packet)
        {            
            CharacterCommon character;
            if(packet.Structure.PawnId == 0)
            {
                character = client.Character;
            }
            else
            {
                character = client.Character.Pawns.Where(pawn => pawn.PawnId == packet.Structure.PawnId).Single();
            }

            IEnumerable<CustomSkill> skillSlots = jobManager.ChangeExSkill(Server.Database, client, character, packet.Structure.Job, packet.Structure.SkillId);

            client.Send(new S2CSkillChangeExSkillRes() {
                Job = packet.Structure.Job,
                SkillId = packet.Structure.SkillId,
                SkillLv = 1, // Must be 1 otherwise they do 0 damage
                PawnId = packet.Structure.PawnId,
                SlotsToUpdate = skillSlots.Select(skill => new CDataCommonU8(skill.SlotNo)).ToList()
            });
        }
    }
}