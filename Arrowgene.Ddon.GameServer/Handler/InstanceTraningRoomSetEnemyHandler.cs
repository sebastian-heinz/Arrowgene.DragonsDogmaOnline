using Arrowgene.Ddon.GameServer.Context;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceTraningRoomSetEnemyHandler : GameRequestPacketQueueHandler<C2SInstanceTraningRoomSetEnemyReq, S2CInstanceTraningRoomSetEnemyRes>
    {
        public InstanceTraningRoomSetEnemyHandler(DdonGameServer server) : base(server)
        {
        }

        private static readonly CDataStageLayoutId TrainingRoomLayout = new CDataStageLayoutId(349, 0, 1);
        private static readonly int RepopDelay = 2000; // ms
        public override PacketQueue Handle(GameClient client, C2SInstanceTraningRoomSetEnemyReq request)
        {

            // TODO: Enemies that share the same positionIndex sometimes spawn with the wrong HP.
            // To avoid this in the meantime, each enemy must have its own unique positionIndex.
            // But the training room layout only has 6 (0-5) indices, so you can only have six different spawns.

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

            Task.Delay(RepopDelay).ContinueWith(_ => {
                switch (request.OptionId)
                {
                    case 1: // Two orc soldiers
                        client.Party.SendToAll(new S2CInstanceEnemyRepopNtc()
                        {
                            LayoutId = TrainingRoomLayout,
                            EnemyData = new CDataLayoutEnemyData()
                            {
                                PositionIndex = 0,
                                EnemyInfo = new CDataStageLayoutEnemyPresetEnemyInfoClient()
                                {
                                    EnemyId = 0x15800,
                                    NamedEnemyParamsId = 47, // Training <name>
                                    Lv = level,
                                    RepopCount = 10,
                                    Scale = 100,
                                    IsBossGauge = true,
                                }
                            },
                            WaitSecond = 0,
                        });
                        client.Party.SendToAll(new S2CInstanceEnemyRepopNtc()
                        {
                            LayoutId = TrainingRoomLayout,
                            EnemyData = new CDataLayoutEnemyData()
                            {
                                PositionIndex = 1,
                                EnemyInfo = new CDataStageLayoutEnemyPresetEnemyInfoClient()
                                {
                                    EnemyId = 0x15800,
                                    NamedEnemyParamsId = 47, // Training <name>
                                    Lv = level,
                                    RepopCount = 10,
                                    Scale = 100,
                                    IsBossGauge = true,
                                }
                            },
                            WaitSecond = 0,
                        });
                        break;
                    case 2: // Cyclops
                        client.Party.SendToAll(new S2CInstanceEnemyRepopNtc()
                        {
                            LayoutId = TrainingRoomLayout,
                            EnemyData = new CDataLayoutEnemyData()
                            {
                                PositionIndex = 0,
                                EnemyInfo = new CDataStageLayoutEnemyPresetEnemyInfoClient()
                                {
                                    EnemyId = 0x15000,
                                    NamedEnemyParamsId = 47, // Training <name>
                                    Lv = level,
                                    RepopCount = 10,
                                    Scale = 100,
                                    IsBossGauge = true,
                                }
                            },
                            WaitSecond = 0,
                        });
                        break;
                    case 3: //Ogre
                        client.Party.SendToAll(new S2CInstanceEnemyRepopNtc()
                        {
                            LayoutId = TrainingRoomLayout,
                            EnemyData = new CDataLayoutEnemyData()
                            {
                                PositionIndex = 0,
                                EnemyInfo = new CDataStageLayoutEnemyPresetEnemyInfoClient()
                                {
                                    EnemyId = 0x15500,
                                    NamedEnemyParamsId = 47, // Training <name>
                                    Lv = level,
                                    RepopCount = 10,
                                    Scale = 100,
                                    IsBossGauge = true,
                                }
                            },
                            WaitSecond = 0,
                        });
                        break;
                    case 4: //Passive Zuhl
                        client.Party.SendToAll(new S2CInstanceEnemyRepopNtc()
                        {
                            LayoutId = TrainingRoomLayout,
                            EnemyData = new CDataLayoutEnemyData()
                            {
                                PositionIndex = 4,
                                EnemyInfo = new CDataStageLayoutEnemyPresetEnemyInfoClient()
                                {
                                    EnemyId = 0x100101,
                                    NamedEnemyParamsId = 722, // Practice <name>, has extra HP.
                                    Lv = level,
                                    RepopCount = 10,
                                    Scale = 75,
                                    IsBossGauge = true,
                                }
                            },
                            WaitSecond = 0,
                        });
                        break;
                }
            });

            return queue;
        }
    }
}
