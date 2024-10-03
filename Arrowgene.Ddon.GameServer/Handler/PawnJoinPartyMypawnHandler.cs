using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.GameServer.Party;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnJoinPartyMypawnHandler : GameStructurePacketHandler<C2SPawnJoinPartyMypawnReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnJoinPartyMypawnHandler));

        public PawnJoinPartyMypawnHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPawnJoinPartyMypawnReq> req)
        {
            Pawn pawn = client.Character.Pawns[req.Structure.PawnNumber-1];
            PawnPartyMember partyMember = client.Party.Join(pawn);
            if (partyMember == null)
            {
                // TODO error response
                Logger.Error(client, $"could not join pawn");
                return;
            }

            client.Party.SendToAll(new S2CPawnJoinPartyPawnNtc() { PartyMember = partyMember.GetCDataPartyMember() });
            client.Party.SendToAll(partyMember.GetS2CContextGetParty_ContextNtc());

            S2CPawnJoinPartyMypawnRes res = new S2CPawnJoinPartyMypawnRes();
            client.Send(res);
        }
    }
}
