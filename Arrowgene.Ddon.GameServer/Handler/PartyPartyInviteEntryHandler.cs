using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyInviteEntryHandler : StructurePacketHandler<GameClient, C2SPartyPartyInviteEntryReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyPartyInviteEntryHandler));

        public PartyPartyInviteEntryHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPartyPartyInviteEntryReq> packet)
        {
            client.Send(new S2CPartyPartyInviteEntryRes());

            S2CPartyPartyInviteEntryNtc ntc = new S2CPartyPartyInviteEntryNtc
            {
                CharacterId = client.Character.Id,
                NowMember = (uint) client.Party.Members.Count,
                MaxMember = 4 // TODO: Check if i can place like 20 players or something
            };
            foreach(GameClient member in client.Party.Members)
            {
                member.Send(ntc);
            }
        }
    }
}