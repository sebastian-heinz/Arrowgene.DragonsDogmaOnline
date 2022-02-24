using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Client.Resource;

public class FieldAreaAdjoinList : ResourceFile
{
    protected override MagicIdWidth IdWidth => MagicIdWidth.UInt;

    public class Vector3
    {
        public MtVector3 Pos { get; set; }
        public uint QuestId { get; set; }
        public uint FlagId { get; set; }
    }

    public class AdjoinInfo
    {
        public List<Vector3> Positions { get; set; }
        public short DestinationStageNo { get; set; }
        public short NextStageNo { get; set; }
        public byte Priority { get; set; }

        public AdjoinInfo()
        {
            Positions = new List<Vector3>();
        }
    }

    public List<AdjoinInfo> AdjoinInfos { get; }

    public FieldAreaAdjoinList()
    {
        AdjoinInfos = new List<AdjoinInfo>();
    }

    protected override void ReadResource(IBuffer buffer)
    {
    }
}
