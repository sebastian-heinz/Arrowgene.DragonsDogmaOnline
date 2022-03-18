using System.Collections.Generic;
using System.IO;
using Arrowgene.Ddon.Client.Data;
using Arrowgene.Ddon.Client.Resource;
using Arrowgene.Ddon.Shared;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Client
{
    public class ClientResourceRepository
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(ClientResourceRepository));

        public FieldAreaList FieldAreaList { get; private set; }
        public AreaStageList AreaStageList { get; private set; }
        public AreaList AreaList { get; private set; }
        public StageList StageList { get; private set; }
        public LandListLal LandList { get; private set; }
        public StageToSpot StageToSpot { get; private set; }
        public GuiMessage FieldAreaNames { get; private set; }
        public WarpLocationList WarpLocationList { get; private set; }
        public WarpLocationList WarpLocationListRim { get; private set; }
        public Dictionary<uint, LocationData> StageLocations { get; private set; }
        public Dictionary<uint, List<FieldAreaMarkerInfo.MarkerInfo>> StageOmMarker { get; private set; }
        public Dictionary<uint, List<FieldAreaMarkerInfo.MarkerInfo>> StageSceMarker { get; private set; }
        public Dictionary<uint, List<FieldAreaMarkerInfo.MarkerInfo>> StageNpcMarker { get; private set; }
        public Dictionary<uint, List<FieldAreaMarkerInfo.MarkerInfo>> StageEctMarker { get; private set; }
        public Dictionary<uint, List<FieldAreaAdjoinList.AdjoinInfo>> StageAdJoin { get; private set; }
        public Dictionary<uint, List<StageToSpot.Entry>> StageSpots { get; private set; }
        public ClientData Data { get; private set; }

        private DirectoryInfo _directory;

        public ClientResourceRepository()
        {
            // Nested
            Data = new ClientData();

            // Flat Lookup
            StageLocations = new Dictionary<uint, LocationData>();
            StageOmMarker = new Dictionary<uint, List<FieldAreaMarkerInfo.MarkerInfo>>();
            StageSceMarker = new Dictionary<uint, List<FieldAreaMarkerInfo.MarkerInfo>>();
            StageNpcMarker = new Dictionary<uint, List<FieldAreaMarkerInfo.MarkerInfo>>();
            StageEctMarker = new Dictionary<uint, List<FieldAreaMarkerInfo.MarkerInfo>>();
            StageAdJoin = new Dictionary<uint, List<FieldAreaAdjoinList.AdjoinInfo>>();
            StageSpots = new Dictionary<uint, List<StageToSpot.Entry>>();

            // Client Resources
            AreaStageList = new AreaStageList();
            AreaList = new AreaList();
            StageList = new StageList();
            FieldAreaList = new FieldAreaList();
            LandList = new LandListLal();
            StageToSpot = new StageToSpot();
        }

        public void Load(DirectoryInfo romDirectory)
        {
            _directory = romDirectory;
            if (_directory == null || !_directory.Exists)
            {
                Logger.Error("Rom Path Invalid");
                return;
            }
            
            WarpLocationList = GetResource<WarpLocationList>("ui/gui_cmn.arc", "ui/03_warp/warpLocationList", "wal");
            WarpLocationListRim = GetResource<WarpLocationList>("ui/uGUIRimWarp.arc", "ui/03_warp/lobbyWarpLocationList", "wal");
            LandList = GetResource<LandListLal>("base.arc", "scr/land_list");
            AreaStageList = GetResource<AreaStageList>("base.arc", "scr/area_stage_list");
            AreaList = GetResource<AreaList>("base.arc", "scr/area_list");
            StageList = GetResource<StageList>("base.arc", "scr/stage_list");
            FieldAreaList = GetResource<FieldAreaList>("game_common.arc", "etc/FieldArea/field_area_list");
            StageToSpot = GetFile<StageToSpot>("game_common.arc", "param/stage_to_spot");
            FieldAreaNames = GetResource<GuiMessage>("game_common.arc", "ui/00_message/common/field_area_name", "gmd");

            foreach (StageList.Info sli in StageList.StageInfos)
            {
                LocationData lcd = GetResource_NoLog<LocationData>(
                    $"stage/st{sli.StageNo:0000}/st{sli.StageNo:0000}.arc",
                    $"scr/st{sli.StageNo:0000}/etc/st{sli.StageNo:0000}",
                    "lcd"
                );
                if (lcd != null)
                {
                    if (!StageLocations.ContainsKey(sli.StageNo))
                    {
                        StageLocations.Add(sli.StageNo, lcd);
                    }
                }
            }

            foreach (FieldAreaList.FieldAreaInfo fai in FieldAreaList.FieldAreaInfos)
            {
                GuiMessage.Entry areaName = FieldAreaNames.Entries[(int) fai.GmdIdx];
                FieldAreaMarkerInfo omMarker = GetResource_NoLog<FieldAreaMarkerInfo>(
                    $"/FieldArea/FieldArea{fai.FieldAreaId:000}_marker.arc",
                    $"etc/FieldArea/FieldArea{fai.FieldAreaId:000}_marker_om",
                    "fmi"
                );
                if (omMarker != null)
                {
                    AddMarker(omMarker.MarkerInfos, StageOmMarker);
                }

                FieldAreaMarkerInfo sceMarker = GetResource_NoLog<FieldAreaMarkerInfo>(
                    $"/FieldArea/FieldArea{fai.FieldAreaId:000}_marker.arc",
                    $"etc/FieldArea/FieldArea{fai.FieldAreaId:000}_marker_sce",
                    "fmi"
                );
                if (sceMarker != null)
                {
                    AddMarker(sceMarker.MarkerInfos, StageSceMarker);
                }

                FieldAreaMarkerInfo npcMarker = GetResource_NoLog<FieldAreaMarkerInfo>(
                    $"/FieldArea/FieldArea{fai.FieldAreaId:000}_marker.arc",
                    $"etc/FieldArea/FieldArea{fai.FieldAreaId:000}_marker_npc",
                    "fmi"
                );
                if (npcMarker != null)
                {
                    AddMarker(npcMarker.MarkerInfos, StageNpcMarker);
                }

                FieldAreaMarkerInfo ectMarker = GetResource_NoLog<FieldAreaMarkerInfo>(
                    $"/FieldArea/FieldArea{fai.FieldAreaId:000}_marker.arc",
                    $"etc/FieldArea/FieldArea{fai.FieldAreaId:000}_marker_ect",
                    "fmi"
                );
                if (ectMarker != null)
                {
                    AddMarker(ectMarker.MarkerInfos, StageEctMarker);
                }

                FieldAreaAdjoinList adjoin = GetResource_NoLog<FieldAreaAdjoinList>(
                    $"/FieldArea/FieldArea{fai.FieldAreaId:000}_marker.arc",
                    $"etc/FieldArea/FieldArea{fai.FieldAreaId:000}_adjoin",
                    "faa"
                );
                if (adjoin != null)
                {
                    AddAdjoin(adjoin.AdjoinInfos, StageAdJoin);
                }
            }

            foreach (StageToSpot.Entry sts in StageToSpot.Entries)
            {
                if (!StageSpots.ContainsKey(sts.StageNo))
                {
                    StageSpots[sts.StageNo] = new List<StageToSpot.Entry>();
                }

                StageSpots[sts.StageNo].Add(sts);
            }

            // Land -> Area -> Stage -> Spot
            // for each land
            foreach (LandListLal.LandInfo land in LandList.LandInfos)
            {
                LandData landData = Data.ProvideLand(land.LandId);
                // for each area in the land
                foreach (uint areaId in land.AreaIds)
                {
                    AreaData areaData = Data.ProvideArea(areaId);
                    landData.AddArea(areaData);
                    foreach (AreaStageList.AreaInfoStage ais in AreaStageList.AreaInfoStages)
                    {
                        // find stages that belong to this area
                        if (ais.AreaId == areaId)
                        {
                            foreach (StageList.Info sli in StageList.StageInfos)
                            {
                                if (sli.StageNo == ais.StageNo)
                                {
                                    StageData stageData = Data.ProvideStage(sli.StageNo);
                                    stageData.AddOmMarker(StageOmMarker);
                                    stageData.AddEctMarker(StageEctMarker);
                                    stageData.AddNpcMarker(StageNpcMarker);
                                    stageData.AddSceMarker(StageSceMarker);
                                    stageData.AddAdJoin(StageAdJoin);
                                    stageData.AddLocations(StageLocations);
                                    stageData.AddSpots(StageSpots);

                                    areaData.AddStage(stageData);
                                }
                            }
                        }
                    }
                }
            }

        }

        private void AddMarker(List<FieldAreaMarkerInfo.MarkerInfo> markers,
            Dictionary<uint, List<FieldAreaMarkerInfo.MarkerInfo>> dst)
        {
            foreach (FieldAreaMarkerInfo.MarkerInfo marker in markers)
            {
                if (!dst.ContainsKey((uint) marker.StageNo))
                {
                    dst[(uint) marker.StageNo] = new List<FieldAreaMarkerInfo.MarkerInfo>();
                }

                dst[(uint) marker.StageNo].Add(marker);
            }
        }

        private void AddAdjoin(List<FieldAreaAdjoinList.AdjoinInfo> adjoins,
            Dictionary<uint, List<FieldAreaAdjoinList.AdjoinInfo>> dst)
        {
            foreach (FieldAreaAdjoinList.AdjoinInfo adjoin in adjoins)
            {
                if (!dst.ContainsKey((uint) adjoin.DestinationStageNo))
                {
                    dst[(uint) adjoin.DestinationStageNo] = new List<FieldAreaAdjoinList.AdjoinInfo>();
                }

                dst[(uint) adjoin.DestinationStageNo].Add(adjoin);
            }
        }

        private T GetFile<T>(string arcPath, string filePath, string ext = null) where T : ClientFile, new()
        {
            return ArcArchive.GetFile<T>(_directory, arcPath, filePath, ext);
        }

        private T GetResource<T>(string arcPath, string filePath, string ext = null) where T : ResourceFile, new()
        {
            return ArcArchive.GetResource<T>(_directory, arcPath, filePath, ext);
        }

        private T GetResource_NoLog<T>(string arcPath, string filePath, string ext = null) where T : ResourceFile, new()
        {
           return ArcArchive.GetResource_NoLog<T>(_directory, arcPath, filePath, ext);
        }
    }
}
