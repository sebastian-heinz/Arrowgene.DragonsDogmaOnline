using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyMemberSetValueHandler : GameRequestPacketHandler<C2SPartyPartyMemberSetValueReq, S2CPartyPartyMemberSetValueRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyMemberSetValueHandler));

        public PartyMemberSetValueHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPartyPartyMemberSetValueRes Handle(GameClient client, C2SPartyPartyMemberSetValueReq request)
        {
            // client.Send(GameFull.Dump_900);
            S2CPartyPartyMemberSetValueRes res = new S2CPartyPartyMemberSetValueRes();

            S2CPartyPartyMemberSetValueNtc ntc = new S2CPartyPartyMemberSetValueNtc()
            {
                CharacterId = client.Character.CharacterId,
                Index = request.Index,
                Value = request.Value
            };
            client.Party.SendToAllExcept(ntc, client);

            return res;
        }
    }
}
