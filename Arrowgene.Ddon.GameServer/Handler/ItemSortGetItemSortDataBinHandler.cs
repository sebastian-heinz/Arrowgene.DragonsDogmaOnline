using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemSortGetItemSortDataBinHandler : GameRequestPacketHandler<C2SItemSortGetItemSortDataBinReq, S2CItemSortGetItemSortdataBinRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemSortGetItemSortDataBinHandler));


        public ItemSortGetItemSortDataBinHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CItemSortGetItemSortdataBinRes Handle(GameClient client, C2SItemSortGetItemSortDataBinReq request)
        {
            S2CItemSortGetItemSortdataBinNtc ntc = new S2CItemSortGetItemSortdataBinNtc();
            S2CItemSortGetItemSortdataBinRes res = new S2CItemSortGetItemSortdataBinRes();
            foreach (var item in request.SortList)
            {
                StorageType storageType = (StorageType)item.Value; // why the hell is it U32 then
                Storage storage = client.Character.Storage.GetStorage(storageType);

                CDataItemSort cdata = new CDataItemSort()
                {
                    StorageType = storageType,
                    Bin = storage.SortData
                };

                res.SortData.Add(cdata);
                ntc.SortData.Add(cdata);
            }

            client.Send(ntc); // Whats the ntc for if the res has the same info

            return res;
        }
    }
}
