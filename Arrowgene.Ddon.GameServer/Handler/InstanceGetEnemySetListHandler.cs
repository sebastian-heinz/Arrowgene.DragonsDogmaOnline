using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Crypto;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetEnemySetListHandler : StructurePacketHandler<GameClient, C2SInstanceGetEnemySetListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetEnemySetListHandler));

        public InstanceGetEnemySetListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceGetEnemySetListReq> request)
        {
            StageId stageId = StageId.FromStageLayoutId(request.Structure.LayoutId);
            byte subGroupId = request.Structure.SubGroupId;
            client.Character.Stage = stageId;

            Logger.Info($"GroupId={request.Structure.LayoutId.GroupId}, SubGroupId={request.Structure.SubGroupId}, LayerNo={request.Structure.LayoutId.LayerNo}");

            S2CInstanceGetEnemySetListRes response = new S2CInstanceGetEnemySetListRes
            {
                LayoutId = stageId.ToStageLayoutId(),
                SubGroupId = subGroupId,
                RandomSeed = CryptoRandom.Instance.GetRandomUInt32(),
                EnemyList = client.Party.InstanceEnemyManager.GetAssets(stageId, subGroupId).Select((enemy, index) => new CDataLayoutEnemyData()
                {
                    PositionIndex = (byte) index,
                    EnemyInfo = enemy.asCDataStageLayoutEnemyPresetEnemyInfoClient()
                })
                .ToList()
            };

            // TODO: Make this configurable
            if (response.LayoutId.GroupId == 26)
            {
                // Knight's Bitter Enemy
                response.QuestId = 20005010;
            }
            else if (response.LayoutId.GroupId == 2)
            {
                // Spirit Dragon
                response.QuestId = 50300010;
            }

            client.Send(response);
        }
    }
}
