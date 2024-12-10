using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetQuestScheduleInfoHandler : GameRequestPacketHandler<C2SQuestGetQuestScheduleInfoReq, S2CQuestGetQuestScheduleInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetQuestScheduleInfoHandler));

        public QuestGetQuestScheduleInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestGetQuestScheduleInfoRes Handle(GameClient client, C2SQuestGetQuestScheduleInfoReq packet)
        {
            // TODO: Convert QuestScheduleId to QuestId
            return new S2CQuestGetQuestScheduleInfoRes()
            {
                QuestId = packet.QuestScheduleId
            };
        }
    }
}
