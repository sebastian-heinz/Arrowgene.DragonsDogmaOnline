using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceTraningRoomGetEnemyListHandler : GameRequestPacketHandler<C2SInstanceTraningRoomGetEnemyListReq, S2CInstanceTraningRoomGetEnemyListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetQuestCompletedListHandler));

        public static readonly CDataStageLayoutId TrainingRoomLayout = new CDataStageLayoutId(349, 0, 1);

        public InstanceTraningRoomGetEnemyListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CInstanceTraningRoomGetEnemyListRes Handle(GameClient client, C2SInstanceTraningRoomGetEnemyListReq request)
        {
            // TODO: Figure out a better way to do this.
            // If the reset notice is too soon before the repop notice (in InstanceTraningRoomSetEnemyHandler),
            // the repop sometimes fails to happen.
            client.Party.SendToAll(new S2CInstanceEnemyGroupResetNtc()
            {
                LayoutId = TrainingRoomLayout
            });

            return new S2CInstanceTraningRoomGetEnemyListRes()
            {
                MaxLv = 100,
                InfoList = new List<CDataTraningRoomEnemyHeader>()
                {
                    new CDataTraningRoomEnemyHeader()
                    {
                        OptionId = 1,
                        Name = "Orc Soldiers"
                    },
                    new CDataTraningRoomEnemyHeader()
                    {
                        OptionId = 2,
                        Name = "Cyclops"
                    },
                    new CDataTraningRoomEnemyHeader()
                    {
                        OptionId = 3,
                        Name = "Ogre"
                    },
                    new CDataTraningRoomEnemyHeader()
                    {
                        OptionId = 4,
                        Name = "Training Dummy Zuhl"
                    },
                }
            };
        }
    }
}
