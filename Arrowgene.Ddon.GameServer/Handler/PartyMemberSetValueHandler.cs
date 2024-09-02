using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyMemberSetValueHandler : GameStructurePacketHandler<C2SPartyPartyMemberSetValueReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyMemberSetValueHandler));

        public PartyMemberSetValueHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPartyPartyMemberSetValueReq> req)
        {
            // client.Send(GameFull.Dump_900);
            S2CPartyPartyMemberSetValueRes res = new S2CPartyPartyMemberSetValueRes();

            S2CPartyPartyMemberSetValueNtc ntc = new S2CPartyPartyMemberSetValueNtc()
            {
                CharacterId = client.Character.CharacterId,
                Index = req.Structure.Index,
                Value = req.Structure.Value
            };
            client.Party.SendToAllExcept(ntc, client);

            client.Send(res);
        }
    }
}
