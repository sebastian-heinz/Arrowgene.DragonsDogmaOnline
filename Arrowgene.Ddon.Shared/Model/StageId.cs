using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    public readonly struct StageId
    {
        public static StageId Invalid = new StageId(0, 0, 0);

        public static StageId FromStageLayoutId(CStageLayoutID stageLayoutId)
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

        public CStageLayoutID ToStageLayoutId()
        {
            return new CStageLayoutID
            {
                StageId = Id,
                LayerNo = LayerNo,
                GroupId = GroupId
            };
        }
    }
}
