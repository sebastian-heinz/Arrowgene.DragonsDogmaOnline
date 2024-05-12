using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipGetCraftLockedElementListHandler : GameStructurePacketHandler<C2SEquipGetCraftLockedElementListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipGetCraftLockedElementListHandler));
        
        public EquipGetCraftLockedElementListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SEquipGetCraftLockedElementListReq> packet)
        {
            S2CEquipGetCraftLockedElementListRes res = EntitySerializer.Get<S2CEquipGetCraftLockedElementListRes>().Read(InGameDump.data_Dump_110);
            client.Send(res);
        }
    }
}