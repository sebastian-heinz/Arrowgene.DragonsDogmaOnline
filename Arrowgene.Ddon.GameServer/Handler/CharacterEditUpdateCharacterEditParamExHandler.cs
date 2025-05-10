#nullable enable
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterEditUpdateCharacterEditParamExHandler : GameRequestPacketHandler<C2SCharacterEditUpdateCharacterEditParamExReq, S2CCharacterEditUpdateCharacterEditParamExRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterEditUpdateCharacterEditParamExHandler));
        
        public CharacterEditUpdateCharacterEditParamExHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCharacterEditUpdateCharacterEditParamExRes Handle(GameClient client, C2SCharacterEditUpdateCharacterEditParamExReq packet)
        {
            CharacterEditGetShopPriceHandler.CheckPrice(Server, packet.UpdateType, packet.EditPrice.PointType, packet.EditPrice.Value);

            var walletUpdate = Server.WalletManager.RemoveFromWalletNtc(client, client.Character, packet.EditPrice.PointType, packet.EditPrice.Value);
            if (!walletUpdate)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_GP_LACK_GP);
            }

            client.Character.EditInfo = packet.EditInfo;
            Server.Database.UpdateEditInfo(client.Character);
            
            if(packet.FirstName.Length > 0) {
                client.Character.FirstName = packet.FirstName;
                Server.Database.UpdateCharacterBaseInfo(client.Character);
            }

            //Client won't let you reincarnate if you're wearing a gender-locked item, but EquipmentTemplates also have to be cleaned.
            Server.Database.ExecuteInTransaction(connection =>
            {
                Server.EquipManager.CleanGenderedEquipTemplates(Server, client.Character, connection);
            });

            foreach(Client other in Server.ClientLookup.GetAll()) {
                other.Send(new S2CCharacterEditUpdateEditParamExNtc() {
                    CharacterId = client.Character.CharacterId,
                    PawnId = 0,
                    EditInfo = client.Character.EditInfo,
                    Name = client.Character.FirstName
                });
            }

            return new S2CCharacterEditUpdateCharacterEditParamExRes();
        }
    }
}
