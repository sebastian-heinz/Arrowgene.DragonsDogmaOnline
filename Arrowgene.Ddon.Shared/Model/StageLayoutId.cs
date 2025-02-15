using System;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    public readonly struct StageLayoutId
    {
        public static StageLayoutId Invalid = new StageLayoutId(0, 0, 0);

        public readonly uint Id;
        public readonly byte LayerNo;
        public readonly uint GroupId;

        public StageLayoutId(uint id, byte layerNo, uint groupId)
        {
            Id = id;
            LayerNo = layerNo;
            GroupId = groupId;
        }

        public StageLayoutId(CDataStageLayoutId stageLayoutId)
        {
            Id = stageLayoutId.StageId;
            LayerNo = stageLayoutId.LayerNo;
            GroupId = stageLayoutId.GroupId;
        }

        public StageLayoutId(StageInfo stageInfo, byte layerNo, uint groupId)
        {
            Id = stageInfo.StageId;
            LayerNo = layerNo;
            GroupId = groupId;
        }

        public CDataStageLayoutId ToCDataStageLayoutId()
        {
            return new CDataStageLayoutId
            {
                StageId = Id,
                LayerNo = LayerNo,
                GroupId = GroupId
            };
        }

        public bool Equals(StageLayoutId other)
        {
            return Id == other.Id && LayerNo == other.LayerNo && GroupId == other.GroupId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, LayerNo, GroupId);
        }

        public override string ToString()
        {
            return $"{Id}.{LayerNo}.{GroupId}";
        }
    }
}
