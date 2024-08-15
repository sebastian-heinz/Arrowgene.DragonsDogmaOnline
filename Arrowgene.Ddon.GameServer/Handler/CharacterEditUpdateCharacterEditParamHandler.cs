using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterEditUpdateCharacterEditParamHandler : GameRequestPacketHandler<C2SCharacterEditUpdateCharacterEditParamReq, S2CCharacterEditUpdateCharacterEditParamRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterEditUpdateCharacterEditParamHandler));

        public CharacterEditUpdateCharacterEditParamHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCharacterEditUpdateCharacterEditParamRes Handle(GameClient client, C2SCharacterEditUpdateCharacterEditParamReq packet)
        {
            CharacterEditGetShopPriceHandler.CheckPrice(packet.UpdateType, packet.EditPrice.PointType, packet.EditPrice.Value);

            Server.WalletManager.RemoveFromWalletNtc(client, client.Character,
                            packet.EditPrice.PointType, packet.EditPrice.Value);

            client.Character.EditInfo = packet.EditInfo;
            Server.Database.UpdateEditInfo(client.Character);

            foreach(Client other in Server.ClientLookup.GetAll()) {
                other.Send(new S2CCharacterEditUpdateEditParamNtc() {
                    CharacterId = client.Character.CharacterId,
                    PawnId = 0,
                    EditInfo = client.Character.EditInfo
                });
            }

            return new S2CCharacterEditUpdateCharacterEditParamRes();
        }
    }
}
