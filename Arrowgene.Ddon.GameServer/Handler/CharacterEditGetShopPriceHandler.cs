using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterEditGetShopPriceHandler : GameRequestPacketHandler<C2SCharacterEditGetShopPriceReq, S2CCharacterEditGetShopPriceRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterEditGetShopPriceHandler));

        public CharacterEditGetShopPriceHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCharacterEditGetShopPriceRes Handle(GameClient client, C2SCharacterEditGetShopPriceReq request)
        {
            S2CCharacterEditGetShopPriceRes res = new S2CCharacterEditGetShopPriceRes();
            res.PriceInfo.Add(new CDataCharacterEditPriceInfo()
            {
                UpdateType = 1, //Beauty Parlor
                Prices =
                [
                    new()
                    {
                        Type = WalletType.GoldenGemstones,
                        Value = Server.GameSettings.GameServerSettings.BeautyParlorGGPrice,
                    },
                    new()
                    {
                        Type = WalletType.SilverTickets,
                        Value = Server.GameSettings.GameServerSettings.BeautyParlorSTPrice,
                    },
                ]
            });

            res.PriceInfo.Add(new CDataCharacterEditPriceInfo()
            {
                UpdateType = 2, //Reincarnation
                Prices =
                [
                    new()
                    {
                        Type = WalletType.GoldenGemstones,
                        Value = Server.GameSettings.GameServerSettings.ReincarnationGGPrice,
                    },
                ]
            });

            res.PriceInfo.Add(new CDataCharacterEditPriceInfo()
            {
                UpdateType = 3, // Pawn Beauty Parlor
                Prices =
                [
                    new()
                    {
                        Type = WalletType.GoldenGemstones,
                        Value = Server.GameSettings.GameServerSettings.BeautyParlorGGPrice,
                    },
                ]
            });

            return res;
        }

        public static void CheckPrice(DdonGameServer server, byte updateType, WalletType priceType, uint value)
        {            
            switch (updateType)
            {
                case 1:
                    if (priceType == WalletType.GoldenGemstones && value == server.GameSettings.GameServerSettings.BeautyParlorGGPrice) return;
                    if (priceType == WalletType.SilverTickets && value == server.GameSettings.GameServerSettings.BeautyParlorSTPrice) return;
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_SHOP_PRICE_NO_MATCH);
                case 2:
                    if (priceType == WalletType.GoldenGemstones && value == server.GameSettings.GameServerSettings.ReincarnationGGPrice) return;
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_SHOP_PRICE_NO_MATCH);
                case 3:
                    if (priceType == WalletType.GoldenGemstones && value == server.GameSettings.GameServerSettings.BeautyParlorGGPrice) return;
                    if (priceType == WalletType.SilverTickets && value == server.GameSettings.GameServerSettings.BeautyParlorSTPrice) return;
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_SHOP_PRICE_NO_MATCH);
                default:
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_SHOP_PRICE_NO_MATCH);
            }       
        }
    }
}
