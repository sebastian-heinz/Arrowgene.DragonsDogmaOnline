using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillSetPresetAbilityNameHandler : GameRequestPacketHandler<C2SSkillSetPresetAbilityNameReq, S2CSkillSetPresetAbilityNameRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillRegisterPresetAbilityHandler));

        public SkillSetPresetAbilityNameHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSkillSetPresetAbilityNameRes Handle(GameClient client, C2SSkillSetPresetAbilityNameReq packet)
        {
            var preset = client.Character.AbilityPresets.Where(x => x.PresetNo == packet.PresetNo).FirstOrDefault();
            if (preset is null)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_SKILL_INVALID_PRESET_NO);
            }
            preset.PresetName = packet.PresetName;

            Server.Database.UpdateAbilityPreset(client.Character.CharacterId, preset);

            return new S2CSkillSetPresetAbilityNameRes();
        }
    }
}
