using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
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
            CharacterEditGetShopPriceHandler.CheckPrice(Server, packet.UpdateType, packet.EditPrice.PointType, packet.EditPrice.Value);

            bool walletUpdate = Server.WalletManager.RemoveFromWalletNtc(client, client.Character, packet.EditPrice.PointType, packet.EditPrice.Value);
            if (!walletUpdate)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_GP_LACK_GP);
            }

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
