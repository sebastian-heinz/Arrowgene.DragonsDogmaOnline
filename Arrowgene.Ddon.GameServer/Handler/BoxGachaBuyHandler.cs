using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BoxGachaBuyHandler : GameRequestPacketHandler<C2SBoxGachaBuyReq, S2CBoxGachaBuyRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BoxGachaBuyHandler));

        public BoxGachaBuyHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBoxGachaBuyRes Handle(GameClient client, C2SBoxGachaBuyReq request)
        {
            S2CBoxGachaBuyRes res = new S2CBoxGachaBuyRes();

            // TODO: based on gacha ID & draw ID figure out which items were bought
            res.BoxGachaId = request.BoxGachaId;
            res.BoxGachaItemList.Add(new CDataBoxGachaItemInfo
            {
                ItemId = 13800,
                ItemNum = 5,
                ItemStock = 13,
                Rank = 2,
                Effect = 0,
                Probability = 0,
                DrawNum = 0
            });

            List<CDataItemUpdateResult> itemUpdateResult = new List<CDataItemUpdateResult>();
            foreach (CDataBoxGachaItemInfo gachaItemInfo in res.BoxGachaItemList)
            {
                // TODO: support adding to item post
                itemUpdateResult.AddRange(Server.ItemManager.AddItem(Server, client.Character, true, gachaItemInfo.ItemId, gachaItemInfo.ItemNum));
            }

            // TODO: based on Settlement ID figure out which currency was used
            CDataWalletPoint walletPoint = client.Character.WalletPointList.Find(l => l.Type == WalletType.GoldenGemstones);
            walletPoint.Value--;

            S2CItemUpdateCharacterItemNtc itemUpdateNtc = new S2CItemUpdateCharacterItemNtc
            {
                UpdateType = ItemNoticeType.Default,
                UpdateItemList = itemUpdateResult,
                UpdateWalletList = new List<CDataUpdateWalletPoint>
                {
                    new()
                    {
                        Type = WalletType.GoldenGemstones,
                        Value = walletPoint.Value,
                        AddPoint = (int)-request.Price,
                        ExtraBonusPoint = 0
                    }
                }
            };
            client.Send(itemUpdateNtc);

            return res;
        }
    }
}
