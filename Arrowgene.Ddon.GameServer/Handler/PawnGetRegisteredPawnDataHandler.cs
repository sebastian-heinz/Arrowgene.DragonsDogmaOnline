using System;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetRegisteredPawnDataHandler : StructurePacketHandler<GameClient, C2SPawnGetRegisteredPawnDataReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetRegisteredPawnDataHandler));

        private Random Random = new Random();

        public PawnGetRegisteredPawnDataHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPawnGetRegisteredPawnDataReq> packet)
        {
            S2CPawnGetRegisteredPawnDataRes res = new S2CPawnGetRegisteredPawnDataRes();
            res.PawnId = (uint) packet.Structure.PawnId;

            // TODO: Figure out what registered pawns are and why this seems to be requested only sometimes
            if(client.Character.Pawns.Count > 0)
            {
                Pawn pawn = client.Character.Pawns[Random.Next(client.Character.Pawns.Count)];
                GameStructure.CDataPawnInfo(res.PawnInfo, pawn);
            }

            client.Send(res);
            
        }
    }
}