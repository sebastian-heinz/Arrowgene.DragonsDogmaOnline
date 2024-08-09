using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillRegisterPresetAbilityHandler : GameRequestPacketHandler<C2SSkillRegisterPresetAbilityReq, S2CSkillRegisterPresetAbilityRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillRegisterPresetAbilityHandler));

        private DdonGameServer _server;

        public SkillRegisterPresetAbilityHandler(DdonGameServer server) : base(server)
        {
            _server = server;
        }

        public override S2CSkillRegisterPresetAbilityRes Handle(GameClient client, C2SSkillRegisterPresetAbilityReq packet)
        {
            Logger.Info($"Registering Preset for ID {packet.PawnId}, slot {packet.PresetNo}");

            string newName = $"Preset {packet.PresetNo}";

            var foo = JobManager.ToPresetAbilityParam(client.Character, client.Character.EquippedAbilitiesDictionary[client.Character.Job], packet.PresetNo, newName);
            client.Character.AbilityPresets.Add(foo);

            return new S2CSkillRegisterPresetAbilityRes();
        }
    }
}
