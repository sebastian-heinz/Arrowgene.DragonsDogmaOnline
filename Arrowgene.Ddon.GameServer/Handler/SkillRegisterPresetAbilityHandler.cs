using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillRegisterPresetAbilityHandler : GameRequestPacketHandler<C2SSkillRegisterPresetAbilityReq, S2CSkillRegisterPresetAbilityRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillRegisterPresetAbilityHandler));

        public SkillRegisterPresetAbilityHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSkillRegisterPresetAbilityRes Handle(GameClient client, C2SSkillRegisterPresetAbilityReq packet)
        {
            CharacterCommon sourceCharacter = client.Character;
            if (packet.PawnId > 0)
            {
                sourceCharacter = client.Character.Pawns.Where(x => x.PawnId == packet.PawnId).FirstOrDefault();
            }

            if (sourceCharacter is null)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_INVALID);
            }

            string newName = $"Preset {packet.PresetNo}";
            var existingPreset = client.Character.AbilityPresets.Where(x => x.PresetNo == packet.PresetNo).FirstOrDefault();
            if (existingPreset is not null)
            {
                newName = existingPreset.PresetName;
            }

            var newPreset = JobManager.MakePresetAbilityParam(sourceCharacter, sourceCharacter.EquippedAbilitiesDictionary[sourceCharacter.Job], packet.PresetNo, newName);
            client.Character.AbilityPresets.RemoveAll(x => x.PresetNo == packet.PresetNo);
            client.Character.AbilityPresets.Add(newPreset);

            Server.Database.ReplaceAbilityPreset(client.Character.CharacterId, newPreset);

            return new S2CSkillRegisterPresetAbilityRes();
        }
    }
}
