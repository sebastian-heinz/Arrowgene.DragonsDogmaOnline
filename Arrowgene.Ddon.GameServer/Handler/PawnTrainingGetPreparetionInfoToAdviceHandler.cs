using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnTrainingGetPreparetionInfoToAdviceHandler : StructurePacketHandler<GameClient, C2SPawnTrainingGetPreparetionInfoToAdviceReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnTrainingGetPreparetionInfoToAdviceHandler));

        public PawnTrainingGetPreparetionInfoToAdviceHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPawnTrainingGetPreparetionInfoToAdviceReq> req)
        {
            S2CPawnTrainingGetPreparetionInfoToAdviceRes res = new S2CPawnTrainingGetPreparetionInfoToAdviceRes();
            client.Send(res);
        }
    }
}
