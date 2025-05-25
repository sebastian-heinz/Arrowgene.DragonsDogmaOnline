#nullable enable
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftSkillUpHandler : GameRequestPacketHandler<C2SCraftSkillUpReq, S2CCraftSkillUpRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftSkillUpHandler));

        public CraftSkillUpHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCraftSkillUpRes Handle(GameClient client, C2SCraftSkillUpReq request)
        {
            S2CCraftSkillUpRes craftSkillUpRes = new S2CCraftSkillUpRes
            {
                PawnID = request.PawnID,
                SkillType = request.SkillType,
                SkillLevel = request.SkillLevel
            };

            Pawn pawn = client.Character.Pawns.Find(p => p.PawnId == request.PawnID)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_NOT_FOUNDED,
                $"Couldn't find pawn ID {request.PawnID}.");

            CDataPawnCraftSkill pawnCraftSkill = pawn.CraftData.PawnCraftSkillList.Find(skill => skill.Type == request.SkillType)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_CRAFT_INVALID_CRAFT_SKILL_TYPE);
            uint previousCraftSkillLevel = pawnCraftSkill.Level;
            pawnCraftSkill.Level = request.SkillLevel;

            craftSkillUpRes.UseCraftPoint = pawnCraftSkill.Level - previousCraftSkillLevel;

            pawn.CraftData.CraftPoint -= craftSkillUpRes.UseCraftPoint;
            craftSkillUpRes.RemainCraftPoint = pawn.CraftData.CraftPoint;

            Server.Database.UpdatePawnBaseInfo(pawn);

            return craftSkillUpRes;
        }
    }
}
