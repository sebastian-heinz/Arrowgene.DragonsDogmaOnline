using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

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
            S2CGachaBuyRes res = new S2CGachaBuyRes();

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

            return res;
        }
    }
}
