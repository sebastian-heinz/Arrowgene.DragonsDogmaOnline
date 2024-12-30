using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public class QuestProcess
    {
        public ushort ProcessNo { get; set; }
        public Dictionary<uint, QuestBlock> Blocks { get; set; }

        public QuestProcess(ushort processNo = 0)
        {
            ProcessNo = processNo;
            Blocks = new Dictionary<uint, QuestBlock>();
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
