using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftGetCraftIRCollectionValueListHandler : GameStructurePacketHandler<C2SCraftGetCraftIRCollectionValueListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftGetCraftIRCollectionValueListHandler));
        
        public CraftGetCraftIRCollectionValueListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCraftGetCraftIRCollectionValueListReq> packet)
        {
            S2CCraftGetCraftIRCollectionValueListRes res = EntitySerializer.Get<S2CCraftGetCraftIRCollectionValueListRes>().Read(InGameDump.data_Dump_109);
            client.Send(res);
        }
    }
}