using Arrowgene.Ddon.GameServer.Context;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceTraningRoomSetEnemyHandler : GameRequestPacketQueueHandler<C2SInstanceTraningRoomSetEnemyReq, S2CInstanceTraningRoomSetEnemyRes>
    {
        public InstanceTraningRoomSetEnemyHandler(DdonGameServer server) : base(server)
        {
        }

        private static readonly CDataStageLayoutId TrainingRoomLayout = Stage.TrainingRoom.AsCDataStageLayoutId(1);
        private static readonly int RepopDelay = 2000; // ms
        public override PacketQueue Handle(GameClient client, C2SInstanceTraningRoomSetEnemyReq request)
        {
            PacketQueue queue = new();

            client.Enqueue(new S2CInstanceTraningRoomSetEnemyRes(), queue);

            client.Party.InstanceEnemyManager.ResetEnemyNode(TrainingRoomLayout.AsStageLayoutId());

            client.Party.EnqueueToAll(new S2CInstanceEnemyGroupResetNtc()
            {
                LayoutId = TrainingRoomLayout
            }, queue);
            
            for (ulong i = 0; i < 6; i++)
            {
                var uid = ContextManager.CreateEnemyUID(i, TrainingRoomLayout);
                ContextManager.RemoveContext(client.Party, uid);
            }

            ushort level = (ushort)request.Lv;

            if (request.OptionId > Server.AssetRepository.TrainingRoomAsset.Count)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_INSTANCE_AREA_ENEMY_GROUP_NOT_CREATED, 
                    $"Missing training room data for entry {request.OptionId}");
            }

            Task.Delay(RepopDelay).ContinueWith(_ =>
            {
                foreach (var entry in Server.AssetRepository.TrainingRoomAsset[(int)(request.OptionId - 1)].EnemyData)
                {
                    var ntc = new S2CInstanceEnemyRepopNtc()
                    {
                        LayoutId = TrainingRoomLayout,
                        EnemyData = entry,
                        WaitSecond = 0
                    };
                    ntc.EnemyData.EnemyInfo.Lv = level;
                    ntc.EnemyData.EnemyInfo.RepopCount = 10;
                    client.Party.SendToAll(ntc);
                }
            });

            return queue;
        }
    }
}
