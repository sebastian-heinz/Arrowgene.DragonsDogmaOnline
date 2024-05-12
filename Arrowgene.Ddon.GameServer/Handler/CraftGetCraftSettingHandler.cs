using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftGetCraftSettingHandler : GameStructurePacketHandler<C2SCraftGetCraftSettingReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftGetCraftSettingHandler));
        
        public CraftGetCraftSettingHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCraftGetCraftSettingReq> packet)
        {
            S2CCraftGetCraftSettingRes res = EntitySerializer.Get<S2CCraftGetCraftSettingRes>().Read(InGameDump.data_Dump_107);
            client.Send(res);
        }
    }
}