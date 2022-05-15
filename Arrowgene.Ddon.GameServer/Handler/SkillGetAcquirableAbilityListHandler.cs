using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetAcquirableAbilityListHandler : StructurePacketHandler<GameClient, C2SSkillGetAcquirableAbilityListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetAcquirableAbilityListHandler));

        public SkillGetAcquirableAbilityListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillGetAcquirableAbilityListReq> packet)
        {
            client.Send(new S2CSkillGetAcquirableAbilityListRes()
            {
                AbilityParamList = new List<CDataAbilityParam>()
                {
                    new CDataAbilityParam()
                    {
                        AbilityNo = 1,
                        Job = 1,
                        Type = 1,
                        Params = new List<CDataAbilityLevelParam>()
                        {
                            new CDataAbilityLevelParam()
                            {
                                Lv = 1,
                                RequireJobLevel = 1,
                                RequireJobPoint = 1,
                                IsRelease = true
                            }
                        }
                    }
                }
            });
        }
    }
}