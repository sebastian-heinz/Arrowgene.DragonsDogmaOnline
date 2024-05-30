#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BazaarProceedsHandler : GameRequestPacketHandler<C2SBazaarProceedsReq, S2CBazaarProceedsRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BazaarProceedsHandler));
        
        private readonly ItemManager _itemManager;

        public BazaarProceedsHandler(DdonGameServer server) : base(server)
        {
            _itemManager = server.ItemManager;
        }

        public override S2CBazaarProceedsRes Handle(GameClient client, C2SBazaarProceedsReq request)
        {
            BazaarExhibition exhibition = Server.BazaarManager.GetExhibitionByBazaarId(request.BazaarId);
            
            int totalItemAmount = request.ItemStorageIndicateNum.Select(x => (int) x.ItemNum).Sum();
            int totalPrice = (int) exhibition.Info.ItemInfo.ItemBaseInfo.Price * totalItemAmount;

            if(exhibition.Info.ItemInfo.ItemBaseInfo.ItemId != request.ItemId || exhibition.Info.ItemInfo.ItemBaseInfo.Num != totalItemAmount)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_BAZAAR_INTERNAL_ERROR);
            }

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            updateCharacterItemNtc.UpdateType = 0;

            // UPDATE INVENTORY
            foreach (CDataItemStorageIndicateNum itemStorageIndicateNum in request.ItemStorageIndicateNum)
            {
                bool sendToItemBag;
                switch(itemStorageIndicateNum.StorageType) {
                    case 19:
                        sendToItemBag = true;
                        break;
                    case 20:
                        sendToItemBag = false;
                        break;
                    default:
                        throw new ResponseErrorException(ErrorCode.ERROR_CODE_BAZAAR_INTERNAL_ERROR, "Unexpected destination when buying goods: "+itemStorageIndicateNum.StorageType);
                }
                List<CDataItemUpdateResult> itemUpdateResult = _itemManager.AddItem(Server, client.Character, sendToItemBag, request.ItemId, itemStorageIndicateNum.ItemNum);                
                updateCharacterItemNtc.UpdateItemList.AddRange(itemUpdateResult);
            }

            // UPDATE CHARACTER WALLET
            CDataWalletPoint wallet = client.Character.WalletPointList.Where(wp => wp.Type == WalletType.Gold).Single();
            wallet.Value = (uint) Math.Max(0, (int)wallet.Value - totalPrice);
            Database.UpdateWalletPoint(client.Character.CharacterId, wallet);
            updateCharacterItemNtc.UpdateWalletList.Add(new CDataUpdateWalletPoint()
            {
                Type = WalletType.Gold,
                AddPoint = (int) -totalPrice,
                Value = wallet.Value
            });

            Server.BazaarManager.SetExhibitionState(exhibition.Info.ItemInfo.BazaarId, BazaarExhibitionState.Sold);

            // Notify seller
            GameClient seller = Server.ClientLookup.GetClientByCharacterId(exhibition.CharacterId);
            if(seller != null)
            {
                seller.Send(new S2CBazaarProceedsNtc()
                {
                    BazaarId = exhibition.Info.ItemInfo.BazaarId,
                    ItemId = exhibition.Info.ItemInfo.ItemBaseInfo.ItemId,
                    Proceeds = exhibition.Info.Proceeds
                });
            }
            
            // Send packets
            client.Send(updateCharacterItemNtc);

            return new S2CBazaarProceedsRes
            {
                BazaarId = exhibition.Info.ItemInfo.BazaarId
            };
        }
    }
}