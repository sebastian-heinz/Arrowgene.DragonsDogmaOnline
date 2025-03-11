using System.Linq;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyMemberKickHandler : GameRequestPacketHandler<C2SPartyPartyMemberKickReq, S2CPartyPartyMemberKickRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyPartyCreateHandler));

        public PartyPartyMemberKickHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPartyPartyMemberKickRes Handle(GameClient client, C2SPartyPartyMemberKickReq request)
        {
            S2CPartyPartyMemberKickRes res = new();

            PartyGroup party = client.Party
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_NOT_FOUNDED, "(party == null)");

            PartyMember member = party.Kick(client, request.MemberIndex);

            S2CPartyPartyMemberKickNtc ntc = new()
            {
                MemberIndex = (byte)member.MemberIndex
            };
            party.SendToAll(ntc);
            if (member is PlayerPartyMember playerMember)
            {
                playerMember.Client.Send(ntc);
            }           
            if (member is PawnPartyMember pawnMember)
            {
                pawnMember.Pawn.PawnState = PawnState.None;

                if (pawnMember.PawnId == client.Character.PartnerPawnId)
                {
                    Server.PartnerPawnManager.HandleKickFromParty(client);
                }
            }

            return res;
        }
    }
}
