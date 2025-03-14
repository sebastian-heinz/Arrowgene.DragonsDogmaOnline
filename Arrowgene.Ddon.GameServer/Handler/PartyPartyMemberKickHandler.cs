using System.Linq;
using System.Numerics;
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
            else if (member is PawnPartyMember pawnMember)
            {
                // todo handle other party member pawn
                pawnMember.Pawn.PawnState = PawnState.None;

                if (!pawnMember.Pawn.IsRented)
                {
                    var memberClient = Server.ClientLookup.GetClientByCharacterId(pawnMember.Pawn.CharacterId);
                    if (memberClient != null && (pawnMember.PawnId == memberClient.Character.PartnerPawnId))
                    {
                        Server.PartnerPawnManager.HandleLeaveFromParty(memberClient);
                    }
                }
            }

            return res;
        }
    }
}
