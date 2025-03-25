using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnTrainingGetTrainingStatusHandler : GameRequestPacketHandler<C2SPawnTrainingGetTrainingStatusReq, S2CPawnTrainingGetTrainingStatusRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnTrainingGetTrainingStatusHandler));
        
        public PawnTrainingGetTrainingStatusHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnTrainingGetTrainingStatusRes Handle(GameClient client, C2SPawnTrainingGetTrainingStatusReq request)
        {
            Pawn pawn = client.Character.PawnById(request.PawnId, PawnType.Main);
            S2CPawnTrainingGetTrainingStatusRes res = new S2CPawnTrainingGetTrainingStatusRes()
            {
                TrainingPoints = pawn.TrainingPoints,
                TrainingStatus = pawn.TrainingStatus.GetValueOrDefault(request.Job, new byte[64]),
                AvailableTraining = pawn.AvailableTraining
            };

            return res;
        }
    }
}
