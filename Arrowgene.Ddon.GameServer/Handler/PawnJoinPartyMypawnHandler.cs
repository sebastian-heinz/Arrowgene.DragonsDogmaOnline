using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.GameServer.Dump;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnJoinPartyMypawnHandler : StructurePacketHandler<GameClient, C2SPawnJoinPartyMypawnReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnJoinPartyMypawnHandler));


        public PawnJoinPartyMypawnHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPawnJoinPartyMypawnReq> req)
        {
                S2CPawn_8_37_16_Ntc ntc8_37_16 = new S2CPawn_8_37_16_Ntc(Server.AssetRepository.MyPawnAsset, req.Structure);
                ntc8_37_16.CharacterId = client.Character.Id;
                client.Send(ntc8_37_16);

                S2CContext_35_3_16_Ntc ntc35_3_16 = new S2CContext_35_3_16_Ntc(Server.AssetRepository.MyPawnAsset, req.Structure);
                ntc35_3_16.CharacterId = client.Character.Id;
                client.Send(ntc35_3_16);

                S2CPawnJoinPartyMypawnRes res = new S2CPawnJoinPartyMypawnRes();
                client.Send(res);
        }
    }
}
