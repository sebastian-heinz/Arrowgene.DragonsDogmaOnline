using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillSetPresetAbilityListHandler : GameStructurePacketHandler<C2SSkillSetPresetAbilityListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetPresetAbilityListHandler));

        public SkillSetPresetAbilityListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillSetPresetAbilityListReq> packet)
        {
            // The preset only works properly if the response happens before the notices,
            // but we need to validate the preset first if we want to use the response to throw a user-facing error.
            // Since we only have control over the order as a StructurePacketHandler, we have to re-implement the error handling.

            CDataPresetAbilityParam preset;
            CharacterCommon targetCharacter;
            S2CSkillSetPresetAbilityListRes response;
            try
            {
                preset = client.Character.AbilityPresets
                .Where(x => x.PresetNo == packet.Structure.PresetNo)
                .FirstOrDefault()
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_SKILL_INVALID_PRESET_NO);

                targetCharacter = client.Character;
                if (packet.Structure.PawnId > 0)
                {
                    targetCharacter = client.Character.Pawns
                        .Where(x => x.PawnId == packet.Structure.PawnId)
                        .FirstOrDefault()
                        ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_INVALID_SLOT_NO);
                }

                Server.JobManager.CheckPreset(Server.Database, client, targetCharacter, preset);

                response = new S2CSkillSetPresetAbilityListRes()
                {
                    PawnId = packet.Structure.PawnId,
                    SetAcquirementParamList = preset.AbilityList
                };
            } 
            catch (ResponseErrorException ex)
            {
                response = new S2CSkillSetPresetAbilityListRes();
                response.Error = (uint)ex.ErrorCode;
                client.Send(response);
                return;
            }
            catch (Exception)
            {
                response = new S2CSkillSetPresetAbilityListRes();
                response.Error = (uint)ErrorCode.ERROR_CODE_FAIL;
                throw;
            }

            client.Send(response);

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
        }
    }
}
