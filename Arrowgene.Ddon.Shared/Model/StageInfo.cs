using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Quest;
using System;

namespace Arrowgene.Ddon.Shared.Model
{
    public class StageInfo : IEquatable<StageInfo>
    {
        public readonly uint StageId;
        public readonly uint StageNo;
        public readonly QuestAreaId AreaId;
        public readonly string Name;

        public StageInfo(uint stageId, uint stageNo, QuestAreaId areaId, string name)
        {
            StageId = stageId;
            StageNo = stageNo;
            AreaId = areaId;
            Name = name;
        }

        public StageLayoutId AsStageLayoutId(byte layerNo, uint groupId)
        {
            return new StageLayoutId(StageId, layerNo, groupId);
        }

        public StageLayoutId AsStageLayoutId(uint groupId)
        {
            return AsStageLayoutId(0, groupId);
        }

        public CDataStageLayoutId AsCDataStageLayoutId(byte layerNo, uint groupId)
        {
            return new CDataStageLayoutId()
            {
                StageId = StageId,
                LayerNo = layerNo,
                GroupId = groupId
            };
        }

        public CDataStageLayoutId AsCDataStageLayoutId(uint groupId)
        {
            return AsCDataStageLayoutId(0, groupId);
        }

        public override string ToString()
        {
            return Name;
        }

        public bool Equals(StageInfo that)
        {
            if (that == null)
            {
                return false;
            }
            return (this.StageId == that.StageId) && (this.StageNo == that.StageNo);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return Equals((StageInfo)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StageId, StageNo);
        }

        public static bool operator == (StageInfo lhs, StageInfo rhs)
        {
            if (lhs is null)
            {
                return rhs is null;
            }
            return lhs.Equals(rhs);
        }

        public static bool operator != (StageInfo lhs, StageInfo rhs)
        {
            return !(lhs == rhs);
        }
    }
}
