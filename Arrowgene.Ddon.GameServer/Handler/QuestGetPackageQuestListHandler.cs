using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetPackageQuestListHandler : GameRequestPacketHandler<C2SQuestGetPackageQuestListReq, S2CQuestGetPackageQuestListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetPackageQuestListHandler));

        public QuestGetPackageQuestListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestGetPackageQuestListRes Handle(GameClient client, C2SQuestGetPackageQuestListReq request)
        {
            //var res = EntitySerializer.Get<S2CQuestGetPackageQuestListRes>().Read(GameFull.Dump_159.AsBuffer());
            
            return new()
            {
                Unk0 = request.Unk0
            };
        }
    }
}
