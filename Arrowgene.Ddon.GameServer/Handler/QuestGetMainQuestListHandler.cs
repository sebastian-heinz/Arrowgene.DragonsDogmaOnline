using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.GameServer.Quests.Missions;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Text.Json;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetMainQuestListHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetMainQuestListHandler));

        private readonly QuestAsset _QuestAssets;

        public QuestGetMainQuestListHandler(DdonGameServer server) : base(server)
        {
            _QuestAssets = server.AssetRepository.QuestAsset;
        }

        public override PacketId Id => PacketId.C2S_QUEST_GET_MAIN_QUEST_LIST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            // client.Send(GameFull.Dump_123);

            S2CQuestGetMainQuestListRes res = new S2CQuestGetMainQuestListRes();
            res.MainQuestList.Add(Mq000002_TheSlumberingGod.Create(_QuestAssets));

            // res.MainQuestList.Add(Quest25);    // Can't find this quest
            // res.MainQuestList.Add(Quest30260); // Hopes Bitter End (White Dragon)
            // res.MainQuestList.Add(Quest30270); // Those Who Follow the Dragon (White Dragon)
            // res.MainQuestList.Add(Quest30410); // Japanese Name (Joseph Historian)

            client.Send(res);
        }
    }
}
