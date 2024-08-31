using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using System;

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
            var ntc = new S2CQuestPlayStartTimerNtc()
            {
                PlayEndDateTime = (ulong)(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + 120000)
            };
            client.Party.SendToAll(ntc);



            return new S2CQuestPlayStartTimerRes();
        }
    }
}
