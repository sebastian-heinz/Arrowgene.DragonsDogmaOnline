using System.Linq;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobUpdateExpModeHandler : GameStructurePacketHandler<C2SJobUpdateExpModeReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobUpdateExpModeHandler));

        public JobUpdateExpModeHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SJobUpdateExpModeReq> packet)
        {
            // TODO: Store updated Exp mode in the character object and in DB

            S2CJobGetJobChangeListRes jobChangeList = EntitySerializer.Get<S2CJobGetJobChangeListRes>().Read(InGameDump.data_Dump_52);
            client.Send(new S2CJobUpdateExpModeRes()
            {
                PlayPointList = jobChangeList.PlayPointList.Where(x => packet.Structure.UpdateExpModeList.Any(y => y.Job == x.Job)).ToList()
            });
        }
    }
}