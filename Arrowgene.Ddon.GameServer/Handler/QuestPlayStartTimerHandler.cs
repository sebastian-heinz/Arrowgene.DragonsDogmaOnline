using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestPlayStartTimerHandler : GameRequestPacketHandler<C2SQuestPlayStartTimerReq, S2CQuestPlayStartTimerRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestPlayStartTimerHandler));

        public QuestPlayStartTimerHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestPlayStartTimerRes Handle(GameClient client, C2SQuestPlayStartTimerReq request)
        {
            var quest = QuestManager.GetQuestByBoardId(client.Party.ContentId);
            var ntc = new S2CQuestPlayStartTimerNtc()
            {
                PlayEndDateTime = (ulong)(DateTimeOffset.UtcNow.ToUnixTimeSeconds() + quest.MissionParams.PlaytimeInSeconds) 
            };
            client.Party.SendToAll(ntc);

            Server.ContentManager.StartTimer(client.Party.Id, client, quest.MissionParams.PlaytimeInSeconds);

            return new S2CQuestPlayStartTimerRes();
        }
    }
}
