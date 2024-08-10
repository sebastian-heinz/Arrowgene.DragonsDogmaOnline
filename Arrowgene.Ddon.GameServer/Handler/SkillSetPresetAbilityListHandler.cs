using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillSetPresetAbilityListHandler : GameRequestPacketHandler<C2SSkillSetPresetAbilityListReq, S2CSkillSetPresetAbilityListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetPresetAbilityListHandler));

        public SkillSetPresetAbilityListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSkillSetPresetAbilityListRes Handle(GameClient client, C2SSkillSetPresetAbilityListReq packet)
        {
            CDataPresetAbilityParam preset = client.Character.AbilityPresets.Where(x => x.PresetNo == packet.PresetNo).FirstOrDefault();

            if (preset is null)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_SKILL_INVALID_PRESET_NO);
            }

            CharacterCommon targetCharacter = client.Character;
            if (packet.PawnId > 0)
            {
                targetCharacter = client.Character.Pawns.Where(x => x.PawnId == packet.PawnId).FirstOrDefault();

                if (targetCharacter is null)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_INVALID_SLOT_NO);
                }
            }

            Server.JobManager.SetAbilityPreset(Server.Database, client, targetCharacter, preset);

            if (targetCharacter is Character character)
            {
                client.Send(new S2CSkillSetPresetAbilityNtc()
                {
                    CharacterId = character.CharacterId,
                    AbilityDataList = character.EquippedAbilitiesDictionary[character.Job]
                        .Where(x => x != null)
                        .Select((x, i) => x.AsCDataContextAcquirementData((byte)(i + 1)))
                        .ToList()
                });
            }    
            else if (targetCharacter is Pawn pawn)
            {
                client.Send(new S2CSkillSetPresetPawnAbilityNtc()
                {
                    PawnId = pawn.PawnId,
                    AbilityDataList = pawn.EquippedAbilitiesDictionary[pawn.Job]
                        .Where(x => x != null)
                        .Select((x, i) => x.AsCDataContextAcquirementData((byte)(i + 1)))
                        .ToList()
                });
            }

            return new S2CSkillSetPresetAbilityListRes()
            {
                PawnId = packet.PawnId,
                SetAcquirementParamList = preset.AbilityList
            };
        }
    }
}
