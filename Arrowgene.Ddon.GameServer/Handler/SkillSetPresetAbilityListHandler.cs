using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillSetPresetAbilityListHandler : GameRequestPacketQueueHandler<C2SSkillSetPresetAbilityListReq, S2CSkillSetPresetAbilityListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetPresetAbilityListHandler));

        public SkillSetPresetAbilityListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SSkillSetPresetAbilityListReq request)
        {
            // The preset only works properly if the response happens before the notices.

            PacketQueue queue = new();

            CDataPresetAbilityParam preset;
            CharacterCommon targetCharacter;
            S2CSkillSetPresetAbilityListRes response;
            preset = client.Character.AbilityPresets
                .Where(x => x.PresetNo == request.PresetNo)
                .FirstOrDefault()
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_SKILL_INVALID_PRESET_NO);

            targetCharacter = client.Character;
            if (request.PawnId > 0)
            {
                targetCharacter = client.Character.Pawns
                    .Where(x => x.PawnId == request.PawnId)
                    .FirstOrDefault()
                    ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_INVALID_SLOT_NO);
            }

            Server.JobManager.CheckPreset(Server.Database, client, targetCharacter, preset);

            response = new S2CSkillSetPresetAbilityListRes()
            {
                PawnId = request.PawnId,
                SetAcquirementParamList = preset.AbilityList
            };

            client.Enqueue(response, queue);

            Server.JobManager.SetAbilityPreset(Server.Database, client, targetCharacter, preset);

            if (targetCharacter is Character character)
            {
                client.Enqueue(new S2CSkillSetPresetAbilityNtc()
                {
                    CharacterId = character.CharacterId,
                    AbilityDataList = character.EquippedAbilitiesDictionary[character.Job]
                        .Where(x => x != null)
                        .Select((x, i) => x.AsCDataContextAcquirementData((byte)(i + 1)))
                        .ToList()
                }, queue);
            }
            else if (targetCharacter is Pawn pawn)
            {
                client.Enqueue(new S2CSkillSetPresetPawnAbilityNtc()
                {
                    PawnId = pawn.PawnId,
                    AbilityDataList = pawn.EquippedAbilitiesDictionary[pawn.Job]
                        .Where(x => x != null)
                        .Select((x, i) => x.AsCDataContextAcquirementData((byte)(i + 1)))
                        .ToList()
                }, queue);
            }

            return queue;
        }
    }
}
