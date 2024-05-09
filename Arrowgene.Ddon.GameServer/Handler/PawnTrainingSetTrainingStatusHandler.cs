using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnTrainingSetTrainingStatusHandler : GameStructurePacketHandler<C2SPawnTrainingSetTrainingStatusReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnTrainingSetTrainingStatusHandler));
        
        public PawnTrainingSetTrainingStatusHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPawnTrainingSetTrainingStatusReq> packet)
        {
            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == packet.Structure.PawnId).Single();
            pawn.TrainingStatus[packet.Structure.Job] = packet.Structure.TrainingStatus;
            pawn.TrainingPoints -= packet.Structure.SpentTrainingPoints;

            
            S2CPawnTrainingSetTrainingStatusRes res = new S2CPawnTrainingSetTrainingStatusRes();
            client.Send(res);
        }
    }
}