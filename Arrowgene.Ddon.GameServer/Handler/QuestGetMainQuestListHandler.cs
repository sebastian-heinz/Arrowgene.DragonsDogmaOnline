using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
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
        private readonly QuestManager _QuestManager;

        public QuestGetMainQuestListHandler(DdonGameServer server) : base(server)
        {
            _QuestAssets = server.AssetRepository.QuestAsset;
            _QuestManager = server.QuestManager;
        }

        public override PacketId Id => PacketId.C2S_QUEST_GET_MAIN_QUEST_LIST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            // client.Send(GameFull.Dump_123);

            S2CQuestGetMainQuestListRes res = new S2CQuestGetMainQuestListRes();

            var quest = QuestManager.CloneMainQuest(_QuestAssets, MainQuestId.TheGreatAlchemist);
            // var quest = new CDataMainQuest();
            quest.QuestId = (int) MainQuestId.TheSlumberingGod;
            quest.KeyId = 1337;
            quest.QuestScheduleId = 287350;
            quest.BaseLevel = 1;
            quest.Unk0 = 1;
            quest.Unk1 = 1;
            quest.Unk2 = 1;
            quest.BaseLevel = 1;
            quest.DistributionStartDate = 1440993600;
            quest.DistributionEndDate = 4103413199;

            quest.QuestOrderConditionParamList.Clear();
            quest.QuestOrderConditionParamList = new List<CDataQuestOrderConditionParam>()
            {
                QuestManager.AcceptConditions.MinimumLevelRestriction(1)
            };

            // These correspond with QstTalkChg items in CDataQuestProcessState[0].ResultCommandList
            // quest.QuestTalkInfoList.Add(new CDataQuestTalkInfo(NpcId.TheWhiteDragon, 0x57b1));
            // quest.QuestTalkInfoList.Add(new CDataQuestTalkInfo(NpcId.Unknown0x001e, 0x75d4));
            // quest.QuestTalkInfoList.Add(new CDataQuestTalkInfo(NpcId.Unknown0x0292, 0x75a4));
            // quest.QuestTalkInfoList.Add(new CDataQuestTalkInfo(NpcId.Unknown0x0293, 0x75a5));
            // quest.QuestTalkInfoList.Add(new CDataQuestTalkInfo(NpcId.Unknown0x01f5, 0x7809));
            // quest.QuestTalkInfoList.Add(new CDataQuestTalkInfo(NpcId.Unknown0x000d, 0x750a));
            // quest.QuestTalkInfoList.Add(new CDataQuestTalkInfo(NpcId.Unknown0x0015, 0x780b));

            // Not sure what these do yet
            // quest.QuestLayoutFlagList.Add(new CDataQuestLayoutFlag() { FlagId = 0 });
            // quest.QuestLayoutFlagList.Add(new CDataQuestLayoutFlag() { FlagId = 0x1eb4 }); // This makes Lise, Gurdolin and Elliot appear
            // quest.QuestLayoutFlagList.Add(new CDataQuestLayoutFlag() { FlagId = 0x1f4d });

            // I think each one of these represents a step in the quest
            quest.QuestProcessStateList = new List<CDataQuestProcessState>()
            {
                new CDataQuestProcessState()
                {
                    ProcessNo = 0x0, SequenceNo = 0x0, BlockNo = 0x1,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.IsStageNo(StageNo.WhiteDragonTemple)
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Leo0, 0x2a65)
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 0x1, SequenceNo = 0x0, BlockNo = 0x0,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        // QuestManager.CheckCommand.MyQstFlagOn(0x1234),
                        // QuestManager.CheckCommand.IsStageNo(StageNo.AudienceChamber),
                        QuestManager.CheckCommand.TalkNpc(StageNo.AudienceChamber, NpcId.Leo0)
                        // QuestManager.CheckCommand.NpcTalkAndOrderUi(StageNo.AudienceChamber, NpcId.Leo0)
                    })
                }
            };

            Logger.Info(JsonSerializer.Serialize<CDataMainQuest>(quest));
            
            res.MainQuestList.Add(quest);

            // res.MainQuestList.Add(Quest25);    // Can't find this quest
            // res.MainQuestList.Add(Quest30260); // Hopes Bitter End (White Dragon)
            // res.MainQuestList.Add(Quest30270); // Those Who Follow the Dragon (White Dragon)
            // res.MainQuestList.Add(Quest30410); // Japanese Name (Joseph Historian)

            client.Send(res);
        }
    }
}
