#nullable enable
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipChangeCharacterEquipJobItemHandler : GameRequestPacketQueueHandler<C2SEquipChangeCharacterEquipJobItemReq, S2CEquipChangeCharacterEquipJobItemRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipChangeCharacterEquipJobItemHandler));

        public EquipChangeCharacterEquipJobItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SEquipChangeCharacterEquipJobItemReq request)
        {
            return Server.EquipManager.EquipJobItem(Server, client, client.Character, request.ChangeEquipJobItemList);
        }
    }
}
