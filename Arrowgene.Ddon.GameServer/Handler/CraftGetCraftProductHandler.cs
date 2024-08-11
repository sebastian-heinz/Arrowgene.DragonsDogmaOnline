using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftGetCraftProductHandler : GameRequestPacketHandler<C2SCraftGetCraftProductReq, C2SCraftGetCraftProductRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftGetCraftProductHandler));

        public CraftGetCraftProductHandler(DdonGameServer server) : base(server)
        {
        }

        public override C2SCraftGetCraftProductRes Handle(GameClient client, C2SCraftGetCraftProductReq request)
        {
            CraftProgress craftProgress = Server.Database.SelectPawnCraftProgress(client.Character.CharacterId, request.CraftMainPawnID);

            C2SCraftGetCraftProductRes craftGetCraftProductRes = new C2SCraftGetCraftProductRes();

            craftGetCraftProductRes.CraftProduct = new CDataCraftProduct()
            {
                ItemID = craftProgress.ItemId,
                ItemNum = craftProgress.CreateCount,
                PlusValue = (byte)craftProgress.PlusValue
            };

            List<CDataItemUpdateResult> itemUpdateResult = Server.ItemManager.AddItem(Server, client.Character, request.StorageType != StorageType.ReceiveInStorageCraft,
                craftProgress.ItemId, craftProgress.CreateCount, (byte)craftProgress.PlusValue);
            craftGetCraftProductRes.UpdateItemList.AddRange(itemUpdateResult);
            
            Server.Database.DeletePawnCraftProgress(client.Character.CharacterId, request.CraftMainPawnID);

            return craftGetCraftProductRes;
        }
    }
}
