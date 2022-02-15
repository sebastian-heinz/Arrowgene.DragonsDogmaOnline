using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class StageAreaChangeHandler : StructurePacketHandler<GameClient, C2SStageAreaChangeReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StageAreaChangeHandler));


        public StageAreaChangeHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SStageAreaChangeReq> packet)
        {
            S2CStageAreaChangeRes res = new S2CStageAreaChangeRes();
            res.StageNo = convertIdToStageNo(packet.Structure.StageId);
            res.IsBase = false;
            
            client.Send(res);
        }

        private uint convertIdToStageNo(uint stageId)
        {
            foreach(CDataStageInfo stageInfo in (Server as DdonGameServer).StageList)
            {
                if(stageInfo.ID == stageId)
                    return stageInfo.StageNo;
            }

            Logger.Error($"No stage found with ID ${stageId}");
            return 0; // TODO: Maybe throw an exception?
        }
    }
}
