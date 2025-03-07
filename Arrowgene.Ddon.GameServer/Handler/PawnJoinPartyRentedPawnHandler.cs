using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnJoinPartyRentedPawnHandler : GameRequestPacketHandler<C2SPawnJoinPartyRentedPawnReq, S2CPawnJoinPartyRentedPawnRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnJoinPartyRentedPawnHandler));

        public PawnJoinPartyRentedPawnHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnJoinPartyRentedPawnRes Handle(GameClient client, C2SPawnJoinPartyRentedPawnReq request)
        {

            Pawn pawn = client.Character.RentedPawnBySlotNo(request.SlotNo);
            PawnPartyMember partyMember = client.Party.Join(pawn);

            // Rented pawn need to have character ID of the player using them
            // Otherwise they spawn in WDT and stand still
            pawn.CharacterId = client.Character.CharacterId;
            pawn.IsRented = true;
            pawn.PawnType = PawnType.Support;

            client.Party.SendToAll(new S2CPawnJoinPartyPawnNtc() { PartyMember = partyMember.GetCDataPartyMember() });
            client.Party.SendToAll(partyMember.GetS2CContextGetPartyRentedPawn_ContextNtc());

            return new S2CPawnJoinPartyRentedPawnRes();
        }
    }
}
