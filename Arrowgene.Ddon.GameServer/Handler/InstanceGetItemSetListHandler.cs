using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetItemSetListHandler : StructurePacketHandler<GameClient, C2SInstanceGetItemSetListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetItemSetListHandler));


        public InstanceGetItemSetListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceGetItemSetListReq> req)
        {
            S2CInstanceGetItemSetListRes res = new S2CInstanceGetItemSetListRes(req.Structure);
            client.Send(res);
        }
    }
}







/*
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetItemSetListHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetItemSetListHandler));


        public InstanceGetItemSetListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_INSTANCE_GET_ITEM_SET_LIST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            client.Send(SelectedDump.Dump_93283);
        }
    }
}
*/
