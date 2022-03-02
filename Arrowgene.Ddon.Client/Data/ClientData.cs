using System.Collections.Generic;

namespace Arrowgene.Ddon.Client.Data;

public class ClientData
{
    public ClientData()
    {
        Lands = new Dictionary<uint, LandData>();
        Areas = new Dictionary<uint, AreaData>();
        Stages = new Dictionary<uint, StageData>();
    }

    public Dictionary<uint, LandData> Lands { get; private set; }
    public Dictionary<uint, AreaData> Areas { get; private set; }
    public Dictionary<uint, StageData> Stages { get; private set; }


    public LandData ProvideLand(uint landId)
    {
        if (Lands.ContainsKey(landId))
        {
            return Lands[landId];
        }

        LandData land = new LandData();
        land.LandId = landId;
        Lands.Add(land.LandId, land);
        return land;
    }

    public AreaData ProvideArea(uint areaId)
    {
        if (Areas.ContainsKey(areaId))
        {
            return Areas[areaId];
        }

        AreaData area = new AreaData();
        area.AreaId = areaId;
        Areas.Add(area.AreaId, area);
        return area;
    }

    public StageData ProvideStage(uint stageNo)
    {
        if (Stages.ContainsKey(stageNo))
        {
            return Stages[stageNo];
        }

        StageData stage = new StageData();
        stage.StageNo = stageNo;
        Stages.Add(stage.StageNo, stage);
        return stage;
    }
}
