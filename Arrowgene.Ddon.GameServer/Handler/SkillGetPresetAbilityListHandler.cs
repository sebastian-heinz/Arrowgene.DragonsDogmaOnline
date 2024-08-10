using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetPresetAbilityListHandler : GameRequestPacketHandler<C2SSkillGetPresetAbilityListReq, S2CSkillGetPresetAbilityListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetPresetAbilityListHandler));

        public SkillGetPresetAbilityListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSkillGetPresetAbilityListRes Handle(GameClient client, C2SSkillGetPresetAbilityListReq packet)
        {
            return new S2CSkillGetPresetAbilityListRes()
            {
                PresetAbilityParamList = client.Character.AbilityPresets
            };
        }
    }
}
