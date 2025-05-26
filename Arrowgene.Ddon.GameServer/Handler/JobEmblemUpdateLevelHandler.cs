using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobEmblemUpdateLevelHandler : GameRequestPacketQueueHandler<C2SJobEmblemUpdateLevelReq, S2CJobEmblemUpdateLevelRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobEmblemUpdateLevelHandler));

        public JobEmblemUpdateLevelHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SJobEmblemUpdateLevelReq request)
        {
            var packets = new PacketQueue();

            var emblemData = client.Character.JobEmblems[request.JobId];
            emblemData.EmblemLevel = request.Level;

            Server.Database.ExecuteInTransaction(connection =>
            {
                S2CJobUpdatePlayPointNtc playPointNtc = Server.PPManager.RemovePlayPoint2(client, request.JobId, Server.JobEmblemManager.LevelUpPPCost(request.Level), connectionIn: connection);
                client.Enqueue(playPointNtc, packets);

                Server.Database.UpsertJobEmblemData(client.Character.CharacterId, emblemData, connection);
            });

            client.Enqueue(new S2CJobEmblemUpdateLevelRes()
            {
                JobId = request.JobId,
                Level = request.Level,
                EmblemPoints = new()
                {
                    JobId = request.JobId,
                    Amount = Server.JobEmblemManager.GetAvailableEmblemPoints(emblemData),
                    MaxAmount = Server.JobEmblemManager.MaxEmblemPointsForLevel(emblemData)
                },
            }, packets);

            return packets;
        }
    }
}
