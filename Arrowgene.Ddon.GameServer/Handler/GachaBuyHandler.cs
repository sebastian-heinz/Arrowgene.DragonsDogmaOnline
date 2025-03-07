using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GachaBuyHandler : GameRequestPacketHandler<C2SGachaBuyReq, S2CGachaBuyRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GachaBuyHandler));

        public GachaBuyHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CGachaBuyRes Handle(GameClient client, C2SGachaBuyReq request)
        {
            throw new ResponseErrorException(ErrorCode.ERROR_CODE_NOT_IMPLEMENTED);

            S2CGachaBuyRes res = new S2CGachaBuyRes()
            {
                GachaId = request.GachaId,
            };

            var gachaAsset = Server.AssetRepository.GachaAsset;

            if (!gachaAsset.GachaInfoList.ContainsKey(request.GachaId))
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_GACHA_NOT_FOUND);
            }

            var lootBox = gachaAsset.GachaInfoList[request.GachaId];

            WalletType walletType;
            switch (request.SettlementId)
            {
                case 1:
                    walletType = WalletType.GoldenGemstones;
                    break;
                case 2:
                    walletType = WalletType.SilverTickets;
                    break;
                default:
                    return res;
            }
            
            if (Server.WalletManager.RemoveFromWalletNtc(client, client.Character, walletType, request.Price))
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_GACHA_PRICE_NO_MATCH);
            }

            foreach (var drawGroup in lootBox.DrawGroups)
            {
                var itemList = drawGroup.GachaDrawList[0];
                res.GachaItemList.Add(itemList.GachaItemInfo[Random.Shared.Next(itemList.GachaItemInfo.Count)]);
            }

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = 0
            };

            Server.Database.ExecuteInTransaction(connection =>
            {
                foreach (var item in res.GachaItemList)
                {
                    if (Server.ItemManager.IsItemWalletPoint(item.ItemId))
                    {
                        (WalletType walletType, uint amount) = Server.ItemManager.ItemToWalletPoint(item.ItemId);
                        var result = Server.WalletManager.AddToWallet(client.Character, walletType, amount * item.ItemNum, connectionIn: connection);
                        updateCharacterItemNtc.UpdateWalletList.Add(result);
                    }
                    else if (item.ItemNum > 0)
                    {
                        var result = Server.ItemManager.AddItem(Server, client.Character, true, item.ItemId, item.ItemNum, connectionIn: connection);
                        updateCharacterItemNtc.UpdateItemList.AddRange(result);
                    }
                }
            });

            client.Send(updateCharacterItemNtc);

#if false
            // TODO: based on gacha ID & draw ID figure out which items were bought
            res.GachaId = request.GachaId;
            res.GachaItemList.Add(new CDataGachaItemInfo
            {
                ItemId = 10290,
                ItemNum = 1,
                Rank = 1,
                Effect = 0,
                Probability = 0.68
            });

            List<CDataItemUpdateResult> itemUpdateResult = new List<CDataItemUpdateResult>();
            foreach (CDataGachaItemInfo gachaItemInfo in res.GachaItemList)
            {
                // TODO: support adding to item post
                itemUpdateResult.AddRange(Server.ItemManager.AddItem(Server, client.Character, true, gachaItemInfo.ItemId, gachaItemInfo.ItemNum));
            }

            // TODO: based on Settlement ID figure out which currency was used
            Server.WalletManager.RemoveFromWalletNtc(client, client.Character, WalletType.GoldenGemstones, request.Price);
#endif

            return res;
        }
    }
}
