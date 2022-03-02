using System.Collections.Generic;

namespace Arrowgene.Ddon.Client.Data;

public class LandData
{
    public uint LandId { get; set; }
    public bool IsDispNews { get; set; }
    public byte GameMode { get; set; }
    public Dictionary<uint, AreaData> Areas { get; set; }

    public LandData()
    {
        Areas = new Dictionary<uint, AreaData>();
    }

    public void AddArea(AreaData area)
    {
        if (!Areas.ContainsKey(area.AreaId))
        {
            Areas.Add(area.AreaId, area);
        }
    }
}
