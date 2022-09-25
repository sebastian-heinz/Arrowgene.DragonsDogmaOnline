using System;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetLearnedAbilityListHandler : StructurePacketHandler<GameClient, C2SSkillGetLearnedAbilityListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetLearnedAbilityListHandler));

        public SkillGetLearnedAbilityListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillGetLearnedAbilityListReq> packet)
        {
            client.Send(new S2CSkillGetLearnedAbilityListRes()
            {
                SetAcquierementParam = SkillGetAcquirableAbilityListHandler.AllAbilities
                    .Select(ability => new CDataLearnedSetAcquirementParam() {
                        Job = ability.Job,
                        Type = ability.Type,
                        AcquirementNo = ability.AbilityNo,
                        AcquirementLv = 6
                    }).ToList()
            });            
        }
    }
}