using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BazaarGetItemListHandler : GameRequestPacketHandler<C2SBazaarGetItemListReq, S2CBazaarGetItemListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BazaarGetItemListHandler));
        
        public BazaarGetItemListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBazaarGetItemListRes Handle(GameClient client, C2SBazaarGetItemListReq request)
        {
            S2CBazaarGetItemListRes response = new S2CBazaarGetItemListRes();
            Server.Database.ExecuteInTransaction(connection => {
                foreach (CDataCommonU32 itemId in request.ItemIdList)
                {
                    List<BazaarExhibition> exhibitionsForItemId = Server.BazaarManager.GetActiveExhibitionsForItemId(itemId.Value, client.Character, connection);
                    if (exhibitionsForItemId.Count > 0)
                    {
                        CDataBazaarItemNumOfExhibitionInfo exhibitionInfo = new CDataBazaarItemNumOfExhibitionInfo();
                        exhibitionInfo.ItemId = itemId.Value;
                        foreach (BazaarExhibition exhibition in exhibitionsForItemId)
                        {
                            exhibitionInfo.Num += exhibition.Info.ItemInfo.ItemBaseInfo.Num;
                        }
                        response.ItemList.Add(exhibitionInfo);
                    }
                }
            });

            return response;
        }
    }
}
