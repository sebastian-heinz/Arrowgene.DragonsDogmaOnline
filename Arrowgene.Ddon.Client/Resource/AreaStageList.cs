using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Client.Resource
{
    public class AreaStageList : ResourceFile
    {
        public class AreaInfoStage
        {
            public uint StageNo { get; set; }
            public uint AreaId { get; set; }
        }

        public List<AreaInfoStage> AreaInfoStages { get; }

        public AreaStageList()
        {
            AreaInfoStages = new List<AreaInfoStage>();
        }

        protected override void ReadResource(IBuffer buffer)
        {
            uint version = ReadUInt32(buffer);
            AreaInfoStages.Clear();
            AreaInfoStages.AddRange(ReadMtArray(buffer, ReadEntry));
        }

        private AreaInfoStage ReadEntry(IBuffer buffer)
        {
            AreaInfoStage ais = new AreaInfoStage();
            ais.StageNo = ReadUInt32(buffer);
            ais.AreaId = ReadUInt32(buffer);
            return ais;
        }
    }
}
