using System.Collections.Generic;
using Arrowgene.Ddon.Client.Resource;

namespace Arrowgene.Ddon.Client.Data;

public class StageData
{
    public StageData()
    {
        Locations = new List<LocationData.Entry>();
        OmMarker = new List<FieldAreaMarkerInfo.MarkerInfo>();
        SceMarker = new List<FieldAreaMarkerInfo.MarkerInfo>();
        NpcMarker = new List<FieldAreaMarkerInfo.MarkerInfo>();
        EctMarker = new List<FieldAreaMarkerInfo.MarkerInfo>();
        AdJoin = new List<FieldAreaAdjoinList.AdjoinInfo>();
        Spots = new List<StageToSpot.Entry>();
    }

    public uint StageNo { get; set; }
    public List<LocationData.Entry> Locations { get; set; }
    public List<FieldAreaMarkerInfo.MarkerInfo> OmMarker { get; set; }
    public List<FieldAreaMarkerInfo.MarkerInfo> SceMarker { get; set; }
    public List<FieldAreaMarkerInfo.MarkerInfo> NpcMarker { get; set; }
    public List<FieldAreaMarkerInfo.MarkerInfo> EctMarker { get; set; }
    public List<FieldAreaAdjoinList.AdjoinInfo> AdJoin { get; set; }
    public List<StageToSpot.Entry> Spots { get; set; }


    public void AddSpots(Dictionary<uint, List<StageToSpot.Entry>> stageSpots)
    {
        if (stageSpots.ContainsKey(StageNo))
        {
            Spots.AddRange(stageSpots[StageNo]);
        }
    }
    
    public void AddLocations(Dictionary<uint, LocationData> stageLocations)
    {
        if (stageLocations.ContainsKey(StageNo))
        {
            foreach (LocationData.Entry entry in stageLocations[StageNo].Entries)
            {
                if (!Locations.Contains(entry))
                {
                    Locations.Add(entry);
                }
            }
        }
    }

    public void AddOmMarker(Dictionary<uint, List<FieldAreaMarkerInfo.MarkerInfo>> stageOmMarker)
    {
        AddMarker(stageOmMarker, OmMarker);
    }

    public void AddSceMarker(Dictionary<uint, List<FieldAreaMarkerInfo.MarkerInfo>> stageSceMarker)
    {
        AddMarker(stageSceMarker, SceMarker);
    }

    public void AddNpcMarker(Dictionary<uint, List<FieldAreaMarkerInfo.MarkerInfo>> stageNpcMarker)
    {
        AddMarker(stageNpcMarker, NpcMarker);
    }

    public void AddEctMarker(Dictionary<uint, List<FieldAreaMarkerInfo.MarkerInfo>> stageEctMarker)
    {
        AddMarker(stageEctMarker, EctMarker);
    }

    public void AddAdJoin(Dictionary<uint, List<FieldAreaAdjoinList.AdjoinInfo>> stageAdJoin)
    {
        if (stageAdJoin.ContainsKey(StageNo))
        {
            AdJoin.AddRange(stageAdJoin[StageNo]);
        }
    }

    private void AddMarker(Dictionary<uint, List<FieldAreaMarkerInfo.MarkerInfo>> src,
        List<FieldAreaMarkerInfo.MarkerInfo> dst)
    {
        if (src.ContainsKey(StageNo))
        {
            dst.AddRange(src[StageNo]);
        }
    }
}
