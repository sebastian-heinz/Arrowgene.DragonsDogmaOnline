using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
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
            S2CPartyPartyMemberKickRes res = new S2CPartyPartyMemberKickRes();

            PartyGroup party = client.Party;
            if (party == null)
            {
                Logger.Error(client, "(party == null)");
                res.Error = (uint)ErrorCode.ERROR_CODE_FAIL;
                client.Send(res);
                return;
            }

            ErrorRes<PartyMember> member = party.Kick(client, packet.Structure.MemberIndex);
            if (member.HasError)
            {
                res.Error = (uint)member.ErrorCode;
                client.Send(res);
                return;
            }

            S2CPartyPartyMemberKickNtc ntc = new S2CPartyPartyMemberKickNtc();
            ntc.MemberIndex = (byte)member.Value.MemberIndex;
            party.SendToAll(ntc);
            if (member.Value is PlayerPartyMember playerMember)
            {
                playerMember.Client.Send(ntc);
            }

            client.Send(res);
        }
    }
}
