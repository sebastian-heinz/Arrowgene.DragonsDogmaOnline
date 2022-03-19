using System.Collections.Generic;

namespace Arrowgene.Ddon.Client.Data;

public class AreaData
{
    public AreaData()
    {
        Stages = new Dictionary<uint, StageData>();
    }

    public Dictionary<uint, StageData> Stages { get; set; }
    public uint AreaId { get; set; }

    public void AddStage(StageData stage)
    {
        if (!Stages.ContainsKey(stage.StageNo))
        {
            Stages.Add(stage.StageNo, stage);
        }
    }
}
