using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetDropItemHandler : GameStructurePacketHandler<C2SInstanceGetDropItemReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetDropItemHandler));
        
        public InstanceGetDropItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceGetDropItemReq> packet)
        {
            List<InstancedGatheringItem> items = client.InstanceDropItemManager.GetAssets(packet.Structure.LayoutId, packet.Structure.SetId);
            
            S2CInstanceGetDropItemRes res = new S2CInstanceGetDropItemRes();
            res.LayoutId = packet.Structure.LayoutId;
            res.SetId = packet.Structure.SetId;
            res.GatheringItemGetRequestList = packet.Structure.GatheringItemGetRequestList;
            client.Send(res);

            S2CItemUpdateCharacterItemNtc ntc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = ItemNoticeType.Drop
            };

            Server.Database.ExecuteInTransaction(connection =>
            {
                foreach (CDataGatheringItemGetRequest gatheringItemRequest in packet.Structure.GatheringItemGetRequestList)
                {
                    InstancedGatheringItem dropItem = items[(int)gatheringItemRequest.SlotNo];
                    Server.ItemManager.GatherItem(Server, client.Character, ntc, dropItem, gatheringItemRequest.Num, connection);
                }
            });
            
            client.Send(ntc);
        }
    }
}
