using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BazaarGetItemInfoHandler : GameStructurePacketHandler<C2SBazaarGetItemInfoReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BazaarGetItemInfoHandler));
        
        public BazaarGetItemInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SBazaarGetItemInfoReq> packet)
        {
            // TODO: Fetch from DB
            
            ClientItemInfo queriedItem = ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, packet.Structure.ItemId);
            S2CBazaarGetItemInfoRes res = new S2CBazaarGetItemInfoRes();
            for (ushort i = 1; i <= 10; i++)
            {
                res.BazaarItemList.Add(new CDataBazaarItemInfo()
                {
                    BazaarId = 0,
                    Sequence = 0,
                    ItemBaseInfo = new CDataBazaarItemBaseInfo() {
                        ItemId = packet.Structure.ItemId,
                        Num = i,
                        Price = queriedItem.Price,
                    },
                    ExhibitionTime = 0
                });
            }
            client.Send(res);
        }
    }
}