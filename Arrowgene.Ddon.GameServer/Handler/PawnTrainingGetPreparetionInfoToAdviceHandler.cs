using System.Linq;
using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;

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
            // TODO: Proper implementation
            S2CPawnTrainingGetPreparetionInfoToAdviceRes res = new S2CPawnTrainingGetPreparetionInfoToAdviceRes
            {
                // TODO: Figure out
                Unk0 = 0,
                PawnTrainingPreparationInfoToAdvices = client.Character.Pawns
                    .Select(pawn => new CDataPawnTrainingPreparationInfoToAdvice()
                    {
                        PawnId = pawn.PawnId,
                        // TODO: Figure out. Training lv? Training xp?
                        Unk0 = 0,
                        Unk1 = 0
                    })
                    .ToList()
            };
            client.Send(res);
        }

        // PCAP:
        // 0x0, 0x0, 0x0, 0x0,
        // 0x0, 0x0, 0x0, 0x0, 
        //
        // 0x0, 0x0, 0x0, 0x6, 
        // 
        // 0x0, 0x0, 0x0, 0x3,
        // 1:
        //  0x0, 0xDA, 0x5D, 0x4E, 
        //  0x0, 0x0, 0x0, 0x3, 
        //  0x0, 0x0, 0x0, 0x0,
        // 2:
        //  0x0, 0xDA, 0x66, 0x8D,
        //  0x0, 0x0, 0x0, 0x3, 
        //  0x0, 0x0, 0x0, 0x1, 
        // 3:
        //  0x0, 0xDA, 0xB2, 0xF3, 
        //  0x0, 0x0, 0x0, 0x0,
        //  0x0, 0x0, 0x0, 0x6, 
        //
        // padding: 0x41, 0x6, 0x6A
    }
}
