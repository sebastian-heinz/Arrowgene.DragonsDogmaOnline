using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceTraningRoomGetEnemyListHandler : GameRequestPacketHandler<C2SInstanceTraningRoomGetEnemyListReq, S2CInstanceTraningRoomGetEnemyListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetQuestCompletedListHandler));


        public InstanceTraningRoomGetEnemyListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CInstanceTraningRoomGetEnemyListRes Handle(GameClient client, C2SInstanceTraningRoomGetEnemyListReq request)
        {
            // These OptionIds are intepreted in InstanceTraningRoomSetEnemyHandler.
            return new()
            {
                MaxLv = client.Character.CharacterJobDataList.Max(x => x.Lv) + 10,
                InfoList = [.. Server.AssetRepository.TrainingRoomAsset.Select((x, i) => new CDataTraningRoomEnemyHeader()
                {
                    Name = x.EntryName,
                    OptionId = (uint)(i + 1)
                })]
            };
        }
    }
}
