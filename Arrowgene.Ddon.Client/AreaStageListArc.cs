using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Client
{
    public class AreaStageListArc : ClientResourceFile
    {
        public class AreaInfoStage
        {
            public uint StageNo { get; set; }
            public uint AreaId { get; set; }
        }

        public List<AreaInfoStage> AreaInfoStages { get; }

        public AreaStageListArc()
        {
            AreaInfoStages = new List<AreaInfoStage>();
        }

        protected override void Read(IBuffer buffer)
        {
            AreaInfoStages.Clear();
            AreaInfoStages.AddRange(ReadMtArray(buffer, ReadAreaInfoStage));
        }

        private AreaInfoStage ReadAreaInfoStage(IBuffer buffer)
        {
            AreaInfoStage ais = new AreaInfoStage();
            ais.StageNo = ReadUInt32(buffer);
            ais.AreaId = ReadUInt32(buffer);
            return ais;
        }
    }
}
