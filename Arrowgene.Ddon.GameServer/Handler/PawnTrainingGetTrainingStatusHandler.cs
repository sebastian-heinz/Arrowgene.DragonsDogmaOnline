using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnTrainingGetTrainingStatusHandler : GameStructurePacketHandler<C2SPawnTrainingGetTrainingStatusReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnTrainingGetTrainingStatusHandler));
        
        public PawnTrainingGetTrainingStatusHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPawnTrainingGetTrainingStatusReq> packet)
        {
            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == packet.Structure.PawnId).Single();
            S2CPawnTrainingGetTrainingStatusRes res = new S2CPawnTrainingGetTrainingStatusRes();
            res.TrainingStatus = pawn.TrainingStatus.GetValueOrDefault(packet.Structure.Job, new byte[64]);
            res.TrainingPoints = pawn.TrainingPoints;
            res.AvailableTraining = pawn.AvailableTraining;

            client.Send(res);
        }
    }
}