using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public class QuestProcess
    {
        public ushort ProcessNo { get; set; }
        public List<QuestBlock> Blocks { get; set; }

        public QuestProcess()
        {
            Blocks = new List<QuestBlock>();
        }

        public QuestProcess(ushort processNo)
        {
            ProcessNo = processNo;
            Blocks = new List<QuestBlock>();
        }

        public QuestProcess AddBlock(QuestBlock block)
        {
            if (Blocks.FirstOrDefault(x => x.BlockNo == block.BlockNo) != null)
            {
                throw new Exception($"(ProcessNo={block.ProcessNo},BlockNo={block.BlockNo}) is defined multiple times");
            }

            block.ProcessNo = ProcessNo;
            block.BlockNo = (ushort)(Blocks.Count + 1);

            Blocks.Add(block);

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
