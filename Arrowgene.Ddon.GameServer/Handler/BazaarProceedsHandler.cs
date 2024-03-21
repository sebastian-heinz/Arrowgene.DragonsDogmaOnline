#nullable enable

using System;
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
    public class BazaarProceedsHandler : GameStructurePacketHandler<C2SBazaarProceedsReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BazaarProceedsHandler));
        
        private readonly ItemManager _itemManager;

        public BazaarProceedsHandler(DdonGameServer server) : base(server)
        {
            _itemManager = server.ItemManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SBazaarProceedsReq> packet)
        {
            // TODO: Fetch price by the BazaarId
            ClientItemInfo boughtItemInfo = ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, packet.Structure.ItemId);
            int totalItemAmount = packet.Structure.ItemStorageIndicateNum.Select(x => (int) x.ItemNum).Sum();
            int totalPrice = boughtItemInfo.Price * totalItemAmount;

            S2CBazaarProceedsRes res = new S2CBazaarProceedsRes();
            res.BazaarId = packet.Structure.BazaarId;

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            updateCharacterItemNtc.UpdateType = 0;

            // UPDATE CHARACTER WALLET
            CDataWalletPoint wallet = client.Character.WalletPointList.Where(wp => wp.Type == WalletType.Gold).Single();
            wallet.Value = (uint) Math.Max(0, wallet.Value - totalPrice);
            Database.UpdateWalletPoint(client.Character.CharacterId, wallet);
            updateCharacterItemNtc.UpdateWalletList.Add(new CDataUpdateWalletPoint()
            {
                Type = WalletType.Gold,
                AddPoint = (int) -totalPrice,
                Value = wallet.Value
            });

            // UPDATE INVENTORY
            foreach (CDataItemStorageIndicateNum itemStorageIndicateNum in packet.Structure.ItemStorageIndicateNum)
            {
                bool sendToItemBag;
                switch(itemStorageIndicateNum.StorageType) {
                    case 19:
                        // If packet.Structure.Destination is 19: Send to corresponding item bag
                        sendToItemBag = true;
                        break;
                    case 20:
                        // If packet.Structure.Destination is 20: Send to storage 
                        sendToItemBag = false;
                        break;
                    default:
                        throw new Exception("Unexpected destination when buying goods: "+itemStorageIndicateNum.StorageType);
                }

                CDataItemUpdateResult? itemUpdateResult = _itemManager.AddItem(Server, client.Character, sendToItemBag, packet.Structure.ItemId, itemStorageIndicateNum.ItemNum);                
                updateCharacterItemNtc.UpdateItemList.Add(itemUpdateResult);
            }

            // Send packets
            client.Send(updateCharacterItemNtc);
            client.Send(res);
            // TODO: Send the NTC to the seller?
        }
    }
}