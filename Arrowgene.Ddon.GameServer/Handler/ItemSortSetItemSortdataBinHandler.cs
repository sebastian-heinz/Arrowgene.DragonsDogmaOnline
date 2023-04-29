using System.Reflection.Metadata;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemSortSetItemSortDataBinHandler : StructurePacketHandler<GameClient, C2SItemSortSetItemSortDataBinReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemSortSetItemSortDataBinHandler));


        public ItemSortSetItemSortDataBinHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SItemSortSetItemSortDataBinReq> packet)
        {
            Storage storage = client.Character.Storage.getStorage(packet.Structure.SortData.StorageType);
            storage.SortData = packet.Structure.SortData.Bin;
            Server.Database.UpdateStorage(client.Character.CharacterId, packet.Structure.SortData.StorageType, storage);

            client.Send(new S2CItemSortSetItemSortDataBinRes());
        }
    }
}
