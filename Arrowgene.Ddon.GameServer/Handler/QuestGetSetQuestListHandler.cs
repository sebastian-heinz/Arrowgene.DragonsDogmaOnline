using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.GameServer.Quests.WorldQuests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetSetQuestListHandler : StructurePacketHandler<GameClient, C2SQuestGetSetQuestListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetQuestPartyBonusListHandler));

        private readonly QuestAsset _QuestAssets;
        private readonly QuestManager _QuestManager;

        public QuestGetSetQuestListHandler(DdonGameServer server) : base(server)
        {
            _QuestAssets = server.AssetRepository.QuestAsset;
            _QuestManager = server.QuestManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SQuestGetSetQuestListReq> packet)
        {
            // client.Send(GameFull.Dump_132);
            S2CQuestGetSetQuestListRes res = new S2CQuestGetSetQuestListRes();
            foreach (var quest in client.Character.Quests)
            {
                if (quest.Value.QuestType == QuestType.World)
                {
                    res.SetQuestList.Add(new CDataSetQuestList()
                    {
                        Detail = new CDataSetQuestDetail() { IsDiscovery = quest.Value.IsDiscoverable },
                        Param = quest.Value.ToCDataQuestList(),
                    });
                }
            }

            client.Send(res);
        }
    }
}
