using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetPartyPawnDataHandler : GameStructurePacketHandler<C2SPawnGetPartyPawnDataReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetPartyPawnDataHandler));
        
        public PawnGetPartyPawnDataHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPawnGetPartyPawnDataReq> packet)
        {
            GameClient owner = this.Server.ClientLookup.GetClientByCharacterId(packet.Structure.CharacterId);
            // TODO: Move this to a function or lookup class
            Pawn pawn = owner.Character.Pawns.Where(pawn => pawn.Id == packet.Structure.PawnId).First();

            var res = new S2CPawnGetPartyPawnDataRes();
            res.CharacterId = pawn.CharacterId;
            res.PawnId = pawn.Id;
            GameStructure.CDataPawnInfo(res.PawnInfo, pawn);
            
            client.Send(res);
        }
    }
}