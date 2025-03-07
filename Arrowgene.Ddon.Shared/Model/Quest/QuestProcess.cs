using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public class QuestProcess
    {
        public ushort ProcessNo { get; private set; }
        public uint QuestScheduleId { get; private set; }

        public Dictionary<uint, QuestEnemyGroup> EnemyGroups { get; set; }
        public Dictionary<uint, QuestBlock> Blocks { get; set; }

        public QuestProcess(ushort processNo, uint questScheduleId)
        {
            ProcessNo = processNo;
            Blocks = new Dictionary<uint, QuestBlock>();
            QuestScheduleId = questScheduleId;
            EnemyGroups = new Dictionary<uint, QuestEnemyGroup>();
        }

        public QuestProcess AddBlock(QuestBlock block)
        {
            block.ProcessNo = ProcessNo;
            block.BlockNo = (ushort)(Blocks.Count + 1);

            if (Blocks.ContainsKey(block.BlockNo))
            {
                throw new Exception($"(ProcessNo={block.ProcessNo},BlockNo={block.BlockNo}) is defined multiple times");
            }

            Blocks[block.BlockNo] = block;

            return this;
        }

        public QuestProcess AddBlocks(List<QuestBlock> blocks)
        {
            foreach (var block in blocks)
            {
                AddBlock(block);
            }
            return this;
        }
    }
}
