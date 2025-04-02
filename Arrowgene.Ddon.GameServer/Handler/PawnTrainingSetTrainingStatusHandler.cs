using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnTrainingSetTrainingStatusHandler : GameRequestPacketHandler<C2SPawnTrainingSetTrainingStatusReq, S2CPawnTrainingSetTrainingStatusRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnTrainingSetTrainingStatusHandler));
        
        public PawnTrainingSetTrainingStatusHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnTrainingSetTrainingStatusRes Handle(GameClient client, C2SPawnTrainingSetTrainingStatusReq request)
        {
            Pawn pawn = client.Character.PawnById(request.PawnId, PawnType.Main);
            pawn.TrainingStatus[request.Job] = request.TrainingStatus;
            pawn.TrainingPoints -= request.SpentTrainingPoints;

            PacketQueue queue = new();
            Server.Database.ExecuteInTransaction(connection =>
            {
                Server.Database.ReplacePawnTrainingStatus(pawn.PawnId, request.Job, request.TrainingStatus, connection);
                Server.Database.UpdatePawnBaseInfo(pawn, connection);
                queue.AddRange(Server.AchievementManager.HandlePawnTraining(client, connection));
            });

            queue.Send();
            return new();
        }
    }
}
