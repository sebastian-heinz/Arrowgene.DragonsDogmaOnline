using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobGetPlayPointListHandler : GameStructurePacketHandler<C2SJobGetPlayPointListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobGetPlayPointListHandler));

        public JobGetPlayPointListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SJobGetPlayPointListReq> packet)
        {
            client.Send(new S2CJobGetPlayPointListRes()
            {
                PlayPointList = client.Character.PlayPointList
            });
        }
    }
}
