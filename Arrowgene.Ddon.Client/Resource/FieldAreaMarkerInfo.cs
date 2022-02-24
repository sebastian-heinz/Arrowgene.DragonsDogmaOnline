using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Client.Resource;

public class FieldAreaMarkerInfo : ResourceFile
{
    protected override MagicIdWidth IdWidth => MagicIdWidth.UInt;

    public class MarkerInfo
    {
        public MtVector3 Pos { get; set; }
        public int StageNo { get; set; }
        public uint GroupNo { get; set; }
        public uint UniqueId { get; set; }
    }

    public List<MarkerInfo> MarkerInfos { get; }

    public FieldAreaMarkerInfo()
    {
        MarkerInfos = new List<MarkerInfo>();
    }

    protected override void ReadResource(IBuffer buffer)
    {
    }
}
