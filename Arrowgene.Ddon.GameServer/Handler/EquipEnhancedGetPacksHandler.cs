using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipEnhancedGetPacks : GameStructurePacketHandler<C2SEquipEnhancedGetPacksReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipEnhancedGetPacks));
        
        public EquipEnhancedGetPacks(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SEquipEnhancedGetPacksReq> packet)
        {
            S2CEquipEnhancedGetPacksRes res = EntitySerializer.Get<S2CEquipEnhancedGetPacksRes>().Read(InGameDump.data_Dump_111);
            client.Send(res);
        }
    }
}