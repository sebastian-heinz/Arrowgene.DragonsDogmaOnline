using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnJoinPartyMypawnHandler : GameRequestPacketHandler<C2SPawnJoinPartyMypawnReq, S2CPawnJoinPartyMypawnRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnJoinPartyMypawnHandler));

        public PawnJoinPartyMypawnHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnJoinPartyMypawnRes Handle(GameClient client, C2SPawnJoinPartyMypawnReq request)
        {
            Pawn pawn = client.Character.Pawns[request.PawnNumber-1];
            PawnPartyMember partyMember = client.Party.Join(pawn);

            pawn.PawnState = PawnState.Party;
            client.Party.SendToAll(new S2CPawnJoinPartyPawnNtc() { PartyMember = partyMember.GetCDataPartyMember() });
            client.Party.SendToAll(partyMember.GetS2CContextGetParty_ContextNtc());

            return new();
        }
    }
}
