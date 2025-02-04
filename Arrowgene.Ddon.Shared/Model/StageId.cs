using System;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    public readonly struct StageId
    {
        public static StageId Invalid = new StageId(0, 0, 0);

        public static StageId FromStageLayoutId(CDataStageLayoutId stageLayoutId)
        {
            return new StageId(stageLayoutId.StageId, stageLayoutId.LayerNo, stageLayoutId.GroupId);
        }

        public readonly uint Id;
        public readonly byte LayerNo;
        public readonly uint GroupId;

        public StageId(uint id, byte layerNo, uint groupId)
        {
            Id = id;
            LayerNo = layerNo;
            GroupId = groupId;
        }

        public StageId(CDataStageLayoutId stageLayoutId)
        {
            Id = stageLayoutId.StageId;
            LayerNo = stageLayoutId.LayerNo;
            GroupId = stageLayoutId.GroupId;
        }

        public CDataStageLayoutId ToStageLayoutId()
        {
            return new CDataStageLayoutId
            {
                StageId = Id,
                LayerNo = LayerNo,
                GroupId = GroupId
            };
        }

        public bool Equals(StageId other)
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

        public static readonly StageId Lestania          = new StageId(1, 0, 0);
        public static readonly StageId WhiteDragonTemple = new StageId(2, 0, 0);
        public static readonly StageId AudienceChamber   = new StageId(3, 0, 0);
        public static readonly StageId PawnCathedral     = new StageId(78, 0, 0);
    }
}
