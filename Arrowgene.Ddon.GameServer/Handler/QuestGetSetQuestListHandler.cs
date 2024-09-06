using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetSetQuestListHandler : GameStructurePacketHandler<C2SQuestGetSetQuestListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetQuestPartyBonusListHandler));

        public QuestGetSetQuestListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SQuestGetSetQuestListReq> packet)
        {
            // client.Send(GameFull.Dump_132);

            S2CQuestGetSetQuestListRes res = new S2CQuestGetSetQuestListRes();
            foreach (var questId in QuestManager.GetWorldQuestIdsByAreaId(packet.Structure.DistributeId))
            {
                var quest = client.Party.QuestState.GetQuest(questId);
                var questStats = client.Party.Leader.Client.Character.CompletedQuests.GetValueOrDefault(quest.QuestId);
                var questState = client.Party.QuestState.GetQuestState(questId);
                /**
                 * World quests get added here instead of QuestGetWorldManageQuestListHandler because
                 * "World Manage Quests" are different from "World Quests". World manage quests appear
                 * to control the state of the game world (doors, paths, gates, etc.). World quests
                 * are random fetch, deliver and kill type quests.
                 */
                res.SetQuestList.Add(new CDataSetQuestList()
                {
                    Detail = new CDataSetQuestDetail() 
                    { 
                        IsDiscovery = (questStats == null) ? quest.IsDiscoverable : true,
                        ClearCount = (questStats == null) ? 0 : questStats.ClearCount
                    },
                    Param = quest.ToCDataQuestList(questState == null ? 0 : questState.Step),
                });
            }

            S2CQuestGetSetQuestListNtc ntc = new S2CQuestGetSetQuestListNtc()
            { 
                SelectCharacterId = client.Party.Leader.Client.Character.CharacterId,
                SetQuestList = res.SetQuestList
            };

            client.Party.SendToAll(ntc);


            client.Send(res);
        }
    }
}
