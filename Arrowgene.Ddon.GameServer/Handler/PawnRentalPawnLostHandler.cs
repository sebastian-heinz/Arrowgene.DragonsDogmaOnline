using System.Linq;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnRentalPawnLostHandler : GameStructurePacketHandler<C2SPawnRentalPawnLostReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnRentalPawnLostHandler));
        
        public PawnRentalPawnLostHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPawnRentalPawnLostReq> packet)
        {
            Pawn pawn = client.Character.RentedPawns.Where(pawn => pawn.PawnId == packet.Structure.PawnId).Single();
            // TODO: Decrement by one the rented pawn's adventure count

            S2CPawnRentalPawnLostNtc ntc = new S2CPawnRentalPawnLostNtc()
            {
                PawnId = pawn.PawnId,
                PawnName = pawn.Name,
                AdventureCount = 5
            };
            client.Party.SendToAll(ntc);

            client.Send(new S2CPawnRentalPawnLostRes()
            {
                PawnId = pawn.PawnId,
                PawnName = pawn.Name,
                AdventureCount = 5
            });

            int pawnIndex = client.Party.Members.FindIndex(x => x is PawnPartyMember xpawn && xpawn.PawnId == packet.Structure.PawnId);
            if (pawnIndex >= 0)
            {
                // Handle serverside tracking. C2SPawnPawnLostReq is only sent to the owner, and only they can kick their own pawn, so it works out.
                client.Party.Kick(client, (byte)pawnIndex);

                // Free up the party slot so that the client allows new invites, if there are less than 4 people remaining.
                client.Party.SendToAll(new S2CPartyPartyMemberKickNtc()
                {
                    MemberIndex = (byte)pawnIndex
                });
            }
        }
    }
}