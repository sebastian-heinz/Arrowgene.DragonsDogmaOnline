using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnJoinPartyMyPawnHandler : GameRequestPacketHandler<C2SPawnJoinPartyMyPawnReq, S2CPawnJoinPartyMyPawnRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnJoinPartyMyPawnHandler));

        public PawnJoinPartyMyPawnHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnJoinPartyMyPawnRes Handle(GameClient client, C2SPawnJoinPartyMyPawnReq request)
        {
            Pawn pawn = client.Character.Pawns[request.PawnNumber-1];
            PawnPartyMember partyMember = client.Party.Join(pawn);

            pawn.PawnState = PawnState.Party;

            var joinNtc = new S2CPawnJoinPartyPawnNtc()
            {
                PartyMember = partyMember.GetCDataPartyMember()
            };

            var contextNtc = partyMember.GetPartyContext();
            contextNtc.Context.Base.EmblemStatList = Server.JobEmblemManager.GetEmblemStatsForCurrentJob(client.Character, pawn.Job);

            client.Party.SendToAll(joinNtc);
            client.Party.SendToAll(contextNtc);

            if (pawn.PawnId == client.Character.PartnerPawnId)
            {
                Server.PartnerPawnManager.CreateAdventureTimer(client);
            }

            return new();
        }
    }
}
