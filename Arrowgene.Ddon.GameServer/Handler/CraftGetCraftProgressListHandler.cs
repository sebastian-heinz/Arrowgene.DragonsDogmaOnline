using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftGetCraftProgressListHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftGetCraftProgressListHandler));

        public CraftGetCraftProgressListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_CRAFT_GET_CRAFT_PROGRESS_LIST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            client.Send(new Packet(PacketId.S2C_CRAFT_GET_CRAFT_PROGRESS_LIST_RES, new byte[20]));
        }
    }
}