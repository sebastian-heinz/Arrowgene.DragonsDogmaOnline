using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemGetEquipRareTypeItemsHandler : GameRequestPacketHandler<C2SItemGetEquipRareTypeItemsReq, S2CItemGetEquipRareTypeItemsRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemGetEquipRareTypeItemsHandler));

        public ItemGetEquipRareTypeItemsHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CItemGetEquipRareTypeItemsRes Handle(GameClient client, C2SItemGetEquipRareTypeItemsReq request)
        {
            return new();
        }
    }
}
