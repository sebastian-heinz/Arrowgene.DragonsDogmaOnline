using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BazaarGetItemListHandler : GameStructurePacketHandler<C2SBazaarGetItemListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BazaarGetItemListHandler));
        
        public BazaarGetItemListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SBazaarGetItemListReq> packet)
        {
            // TODO: Fetch from DB
            
            client.Send(new S2CBazaarGetItemListRes() {
                ItemList = packet.Structure.ItemIdList.Select(itemId => new CDataBazaarItemNumOfExhibitionInfo()
                {
                    ItemId = itemId.Value,
                    Num = (ushort) Enumerable.Range(1, 10).Sum()
                }).ToList()
            });
        }
    }
}