using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnPawnLostHandler : GameRequestPacketQueueHandler<C2SPawnPawnLostReq, S2CPawnPawnLostRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnPawnLostHandler));
        
        public PawnPawnLostHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SPawnPawnLostReq request)
        {
            PacketQueue queue = new();

            Pawn pawn = client.Character.PawnById(request.PawnId, PawnType.Main);

            client.Enqueue(new S2CPawnPawnLostRes()
            {
                PawnId = pawn.PawnId,
                PawnName = pawn.Name,
                IsLost = true
            }, queue);

            S2CPawnPawnLostNtc ntc = new S2CPawnPawnLostNtc()
            {
                PawnId = pawn.PawnId,
                PawnName = pawn.Name,
                IsLost = true
            };
            client.Party.EnqueueToAllExcept(ntc, queue, client);

            var pawnMember = client.Party.GetPartyMemberByCharacter(pawn);
            if (pawnMember is not null)
            {
                // Handle serverside tracking. C2SPawnPawnLostReq is only sent to the owner, and only they can kick their own pawn, so it works out.
                client.Party.Kick(client, (byte)pawnMember.MemberIndex);

                // Free up the party slot so that the client allows new invites, if there are less than 4 people remaining.
                client.Party.EnqueueToAll(new S2CPartyPartyMemberLostNtc()
                {
                    MemberIndex = (byte)pawnMember.MemberIndex
                }, queue);
            }

            pawn.PawnState = PawnState.Lost;
            Server.Database.UpdatePawnBaseInfo(pawn);

            return queue;
        }
    }
}
