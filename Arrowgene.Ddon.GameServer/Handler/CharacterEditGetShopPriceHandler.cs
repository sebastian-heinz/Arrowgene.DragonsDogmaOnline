using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterEditGetShopPriceHandler : GameRequestPacketHandler<C2SCharacterEditGetShopPriceReq, S2CCharacterEditGetShopPriceRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterEditGetShopPriceHandler));

        public CharacterEditGetShopPriceHandler(DdonGameServer server) : base(server)
        {
        }

        // TODO: Figure out what the updateTypes actually are.
        public static readonly uint UNK_TYPE_GG_PRICE = 0;

        public override S2CCharacterEditGetShopPriceRes Handle(GameClient client, C2SCharacterEditGetShopPriceReq request)
        {
            S2CCharacterEditGetShopPriceRes res = new S2CCharacterEditGetShopPriceRes();
            res.PriceInfo.Add(new CDataCharacterEditPriceInfo()
            {
                UpdateType = 1, //Beauty Parlor
                Prices = new List<CDataWalletPoint>()
                {
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
                }
            });

            res.PriceInfo.Add(new CDataCharacterEditPriceInfo()
            {
                UpdateType = 2, //Reincarnation
                Prices = new List<CDataWalletPoint>()
                {
                    new()
                    {
                        Type = WalletType.GoldenGemstones,
                        Value = Server.GameSettings.GameServerSettings.ReincarnationGGPrice,
                    },
                }
            });

            res.PriceInfo.Add(new CDataCharacterEditPriceInfo()
            {
                UpdateType = 3, //Unknown Reincarnation (gender change?)
                Prices = new List<CDataWalletPoint>()
                {
                    new()
                    {
                        Type = WalletType.GoldenGemstones,
                        Value = UNK_TYPE_GG_PRICE,
                    },
                }
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
                    if (priceType == WalletType.GoldenGemstones && value == UNK_TYPE_GG_PRICE) return;
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_SHOP_PRICE_NO_MATCH);
                default:
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_SHOP_PRICE_NO_MATCH);
            }       
        }
    }
}
