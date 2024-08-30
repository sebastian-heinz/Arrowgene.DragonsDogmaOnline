using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceTraningRoomSetEnemyHandler : GameRequestPacketHandler<C2SInstanceTraningRoomSetEnemyReq, S2CInstanceTraningRoomSetEnemyRes>
    {
        public InstanceTraningRoomSetEnemyHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CInstanceTraningRoomSetEnemyRes Handle(GameClient client, C2SInstanceTraningRoomSetEnemyReq request)
        {

            // TODO: Enemies that share the same positionIndex sometimes spawn with the wrong HP.
            // To avoid this in the meantime, each enemy must have its own unique positionIndex.
            // But the training room layout only has 6 (0-5) indices, so you can only have six different spawns.

            var layoutId = InstanceTraningRoomGetEnemyListHandler.TrainingRoomLayout;

            switch (request.OptionId)
            {
                case 1: // Two orc soldiers
                    client.Party.SendToAll(new S2CInstanceEnemyRepopNtc()
                    {
                        LayoutId = layoutId,
                        EnemyData = new CDataLayoutEnemyData()
                        {
                            PositionIndex = 0,
                            EnemyInfo = new CDataStageLayoutEnemyPresetEnemyInfoClient()
                            {
                                EnemyId = 0x15800,
                                NamedEnemyParamsId = 47, // Training <name>
                                Lv = (ushort)request.Lv,
                                RepopCount = 10,
                                Scale = 100,
                                IsBossGauge = true,
                            }
                        },
                        WaitSecond = 1,
                    });
                    client.Party.SendToAll(new S2CInstanceEnemyRepopNtc()
                    {
                        LayoutId = layoutId,
                        EnemyData = new CDataLayoutEnemyData()
                        {
                            PositionIndex = 1,
                            EnemyInfo = new CDataStageLayoutEnemyPresetEnemyInfoClient()
                            {
                                EnemyId = 0x15800,
                                NamedEnemyParamsId = 47, // Training <name>
                                Lv = (ushort)request.Lv,
                                RepopCount = 10,
                                Scale = 100,
                                IsBossGauge = true,
                            }
                        },
                        WaitSecond = 1,
                    });
                    break;
                case 2: // Cyclops
                    client.Party.SendToAll(new S2CInstanceEnemyRepopNtc()
                    {
                        LayoutId = layoutId,
                        EnemyData = new CDataLayoutEnemyData()
                        {
                            PositionIndex = 2,
                            EnemyInfo = new CDataStageLayoutEnemyPresetEnemyInfoClient()
                            {
                                EnemyId = 0x15000,
                                NamedEnemyParamsId = 47, // Training <name>
                                Lv = (ushort)request.Lv,
                                RepopCount = 10,
                                Scale = 100,
                                IsBossGauge = true,
                            }
                        },
                        WaitSecond = 1,
                    });
                    break;
                case 3: //Ogre
                    client.Party.SendToAll(new S2CInstanceEnemyRepopNtc()
                    {
                        LayoutId = layoutId,
                        EnemyData = new CDataLayoutEnemyData()
                        {
                            PositionIndex = 3,
                            EnemyInfo = new CDataStageLayoutEnemyPresetEnemyInfoClient()
                            {
                                EnemyId = 0x15500,
                                NamedEnemyParamsId = 47, // Training <name>
                                Lv = (ushort)request.Lv,
                                RepopCount = 10,
                                Scale = 100,
                                IsBossGauge = true,
                            }
                        },
                        WaitSecond = 1,
                    });
                    break;
                case 4: //Passive Zuhl
                    client.Party.SendToAll(new S2CInstanceEnemyRepopNtc()
                    {
                        LayoutId = layoutId,
                        EnemyData = new CDataLayoutEnemyData()
                        {
                            PositionIndex = 4,
                            EnemyInfo = new CDataStageLayoutEnemyPresetEnemyInfoClient()
                            {
                                EnemyId = 0x100101,
                                NamedEnemyParamsId = 722, // Practice <name>, has extra HP.
                                Lv = (ushort)request.Lv,
                                RepopCount = 10,
                                Scale = 75,
                                IsBossGauge = true,
                            }
                        },
                        WaitSecond = 1,
                    });
                    break;
            }

            return new S2CInstanceTraningRoomSetEnemyRes();
        }
    }
}
