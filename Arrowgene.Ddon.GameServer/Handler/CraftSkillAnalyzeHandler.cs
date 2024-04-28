using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftSkillAnalyzeHandler : GameStructurePacketHandler<C2SCraftSkillAnalyzeReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftSkillAnalyzeHandler));


        public CraftSkillAnalyzeHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCraftSkillAnalyzeReq> packet)
        {
            S2CCraftSkillAnalyzeRes resp = new S2CCraftSkillAnalyzeRes()
            {
                Result = 0
            };

            client.Send(resp);
        }
    }
}
