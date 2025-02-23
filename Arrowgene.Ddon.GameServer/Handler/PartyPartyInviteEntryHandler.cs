using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyInviteEntryHandler : GameRequestPacketQueueHandler<C2SPartyPartyInviteEntryReq, S2CPartyPartyInviteEntryRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyPartyInviteEntryHandler));

        public PartyPartyInviteEntryHandler(DdonGameServer server) : base(server)
        {
        }

        // No idea what this is for, i think for the extreme mission sorties
        public override PacketQueue Handle(GameClient client, C2SPartyPartyInviteEntryReq request)
        {
            PacketQueue queue = new();

            client.Enqueue(new S2CPartyPartyInviteEntryRes(), queue);

            S2CPartyPartyInviteEntryNtc ntc = new S2CPartyPartyInviteEntryNtc
            {
                CharacterId = client.Character.CharacterId,
                NowMember = (uint)client.Party.MemberCount(),
                MaxMember = client.Party.MaxSlots // TODO: Check if i can place like 20 players or something
            };
            client.Party.EnqueueToAll(ntc, queue);

            return queue;
        }
    }
}
