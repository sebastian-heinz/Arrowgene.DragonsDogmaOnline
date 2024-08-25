using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemGetValuableItemListHandler : GameRequestPacketHandler<C2SItemGetValuableItemListReq, S2CItemGetValuableItemListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemGetValuableItemListHandler));


        public ItemGetValuableItemListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CItemGetValuableItemListRes Handle(GameClient client, C2SItemGetValuableItemListReq request)
        {
            //TODO: Implement this properly.
            //This is enough to prevent the client from hanging if you accidentally use this menu.
            var res = new S2CItemGetValuableItemListRes();
            res.Unk0 = 0;
            res.Unk1 = 0;

            return res;
        }
    }
}
