using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyMemberKickHandler : GameStructurePacketHandler<C2SPartyPartyMemberKickReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyPartyCreateHandler));

        public PartyPartyMemberKickHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPartyPartyMemberKickReq> packet)
        {
            PartyGroup party = client.Party;
            if (party == null)
            {
                Logger.Error(client, "party null");
                // TODO
                return;
            }

            PartyMember member = party.Kick(client, packet.Structure.MemberIndex);
            if (member == null)
            {
                Logger.Error(client, "failed to kick");
                // TODO
                return;
            }

            S2CPartyPartyMemberKickNtc ntc = new S2CPartyPartyMemberKickNtc();
            ntc.MemberIndex = (byte)member.MemberIndex;
            party.SendToAll(ntc);
            if (member is PlayerPartyMember playerMember)
            {
                playerMember.Client.Send(ntc);
            }
            
            S2CPartyPartyMemberKickRes res = new S2CPartyPartyMemberKickRes();
            client.Send(res);
        }
    }
}
