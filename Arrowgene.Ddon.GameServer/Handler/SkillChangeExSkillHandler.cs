using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillChangeExSkillHandler : GameRequestPacketHandler<C2SSkillChangeExSkillReq, S2CSkillChangeExSkillRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillChangeExSkillHandler));

        private readonly JobManager jobManager;

        public SkillChangeExSkillHandler(DdonGameServer server) : base(server)
        {
            jobManager = server.JobManager;
        }

        public override S2CSkillChangeExSkillRes Handle(GameClient client, C2SSkillChangeExSkillReq request)
        {
            CharacterCommon character;
            if(request.PawnId == 0)
            {
                character = client.Character;
            }
            else
            {
                Pawn pawn = client.Character.PawnById(request.PawnId, PawnType.Main);
                character = pawn;
            }

            IEnumerable<byte> skillSlots = jobManager.ChangeExSkill(Server.Database, client, character, request.Job, request.SkillId);

            return new S2CSkillChangeExSkillRes() {
                Job = request.Job,
                SkillId = request.SkillId,
                SkillLv = 1, // Must be 1 otherwise they do 0 damage
                PawnId = request.PawnId,
                SlotsToUpdate = skillSlots.Select(slotNo => new CDataCommonU8(slotNo)).ToList()
            };
        }
    }
}
