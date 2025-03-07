using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public class QuestOrderCondition
    {
        public QuestOrderConditionType Type { get; set; }
        public int Param01 { get; set; }
        public int Param02 { get; set; }

        public CDataQuestOrderConditionParam ToCDataQuestOrderConditionParam()
        {
            return new CDataQuestOrderConditionParam()
            {
                Type = (uint)Type,
                Param01 = Param01,
                Param02 = Param02
            };
        }

        public static QuestOrderCondition MinimumLevel(int level)
        {
            return new QuestOrderCondition()
            {
                Type = QuestOrderConditionType.MinimumLevel,
                Param01 = level
            };
        }

        public static QuestOrderCondition MinimumVocationLevel(JobId jobId, int level)
        {
            return new QuestOrderCondition()
            {
                Type = QuestOrderConditionType.MinimumVocationLevel,
                Param01 = (int) jobId,
                Param02 = level,
            };
        }

        public static QuestOrderCondition Solo()
        {
            return new QuestOrderCondition()
            {
                Type = QuestOrderConditionType.Solo
            };
        }

        public static QuestOrderCondition MainQuestCompleted(QuestId questId)
        {
            return new QuestOrderCondition()
            {
                Type = QuestOrderConditionType.MainQuestCompleted,
                Param01 = (int) questId
            };
        }

        public static QuestOrderCondition PersonalQuestCleared(QuestId questId)
        {
            return new QuestOrderCondition()
            {
                Type = QuestOrderConditionType.ClearPersonalQuest,
                Param01 = (int)questId
            };
        }

        public static QuestOrderCondition ExtremeMissionCleared(QuestId questId)
        {
            return new QuestOrderCondition()
            {
                Type = QuestOrderConditionType.ClearExtremeMission,
                Param01 = (int)questId
            };
        }

        public static QuestOrderCondition HasAreaRank(QuestAreaId areaId, int areaRank)
        {
            return new QuestOrderCondition()
            {
                Type = QuestOrderConditionType.AreaRank,
                Param01 = (int) areaId,
                Param02 = areaRank
            };
        }

        public static QuestOrderCondition SoloWithPawns()
        {
            return new QuestOrderCondition()
            {
                Type = QuestOrderConditionType.SoloWithPawns,
            };
        }

        public static QuestOrderCondition ArisenTactics()
        {
            return new QuestOrderCondition()
            {
                Type = QuestOrderConditionType.ArisenTactics,
            };
        }

        public static QuestOrderCondition PrepareEquipment()
        {
            return new QuestOrderCondition()
            {
                Type = QuestOrderConditionType.PrepareEquipment,
            };
        }

        public static QuestOrderCondition PartnerPawnInParty()
        {
            return new QuestOrderCondition()
            {
                Type = QuestOrderConditionType.PartnerPawnInParty,
            };
        }

        public static QuestOrderCondition RequiredItemRank(int itemRank)
        {
            return new QuestOrderCondition()
            {
                Type = QuestOrderConditionType.ItemRank,
                Param01 = itemRank
            };
        }

        public static QuestOrderCondition HasItem(ItemId itemId)
        {
            return new QuestOrderCondition()
            {
                Type = QuestOrderConditionType.PocessesItem,
                Param01 = (int) itemId
            };
        }

        public static QuestOrderCondition SoloWithPawns(int amount)
        {
            return new QuestOrderCondition()
            {
                Type = QuestOrderConditionType.SoloWithPawnCount,
                Param01 = amount
            };
        }

        public static QuestOrderCondition WorldQuestCleared(QuestId questId)
        {
            return new QuestOrderCondition()
            {
                Type = QuestOrderConditionType.ClearWorldQuest,
                Param01 = (int)questId
            };
        }

        public static QuestOrderCondition SubstoryCleared(QuestId questId)
        {
            return new QuestOrderCondition()
            {
                Type = QuestOrderConditionType.ClearSubstory,
                Param01 = (int)questId
            };
        }

        public static QuestOrderCondition MinimumJobLevel(int level)
        {
            return new QuestOrderCondition()
            {
                Type = QuestOrderConditionType.MinimumJobLevel,
                Param01 = level
            };
        }
    }
}
