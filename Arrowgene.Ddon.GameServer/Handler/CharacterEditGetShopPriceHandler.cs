using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterEditGetShopPriceHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterEditGetShopPriceHandler));


        public CharacterEditGetShopPriceHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_CHARACTER_EDIT_GET_SHOP_PRICE_REQ;

        public override void Handle(GameClient client, IPacket packet)
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
                        Value = 0,
                    },
                    new()
                    {
                        Type = WalletType.SilverTickets,
                        Value = 0,
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
                        Value = 0,
                    },
                }
            });

            client.Send(res);
        }
    }
}
