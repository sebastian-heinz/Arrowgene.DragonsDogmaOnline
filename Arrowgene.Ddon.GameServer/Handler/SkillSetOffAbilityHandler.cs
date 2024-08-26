using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillSetOffAbilityHandler : GameRequestPacketHandler<C2SSkillSetOffAbilityReq, S2CSkillSetOffAbilityRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillSetOffAbilityHandler));

        private readonly JobManager jobManager;

        public SkillSetOffAbilityHandler(DdonGameServer server) : base(server)
        {
            jobManager = server.JobManager;
        }

        public override S2CSkillSetOffAbilityRes Handle(GameClient client, C2SSkillSetOffAbilityReq packet)
        {
            jobManager.RemoveAbility(Server.Database, client.Character, packet.SlotNo);

            client.Send(new S2CSkillSetPresetAbilityNtc()
            {
                CharacterId = client.Character.CharacterId,
                AbilityDataList = client.Character.EquippedAbilitiesDictionary[client.Character.Job]
                .Where(x => x != null)
                .Select((x, i) => x.AsCDataContextAcquirementData((byte)(i + 1)))
                .ToList()
            });

            return new S2CSkillSetOffAbilityRes()
            {
                SlotNo = (byte)(client.Character.EquippedAbilitiesDictionary[client.Character.Job]
                .Where(x => x != null)
                .Count()+1)
            };
        }
    }
}
