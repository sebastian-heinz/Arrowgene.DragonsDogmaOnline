using System.Collections.Generic;
using System.IO;
using System.Text.Json;
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

        private DirectoryInfo _directory;

        public ClientResourceRepository()
        {
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

            LandList = GetResource<LandListLal>("base.arc", "scr/land_list");
            AreaStageList = GetResource<AreaStageList>("base.arc", "scr/area_stage_list");
            AreaList = GetResource<AreaList>("base.arc", "scr/area_list");
            StageList = GetResource<StageList>("base.arc", "scr/stage_list");
            FieldAreaList = GetResource<FieldAreaList>("game_common.arc", "etc/FieldArea/field_area_list");
            StageToSpot = GetFile<StageToSpot>("game_common.arc", "param/stage_to_spot");

            // Land has areas, area has stages, and stages have spots
            // Land -> Area -> Stage -> Spot

            // for each land
            foreach (LandListLal.LandInfo land in LandList.LandInfos)
            {
                // for each area in the land
                foreach (uint landAreaId in land.AreaIds)
                {
                    //lookup stages and spots for area
                    List<StageList.Info> stageInfos = new List<StageList.Info>();
                    List<StageToSpot.Entry> spots = new List<StageToSpot.Entry>();
                    foreach (AreaStageList.AreaInfoStage ais in AreaStageList.AreaInfoStages)
                    {
                        if (ais.AreaId == landAreaId)
                        {
                            foreach (StageList.Info sli in StageList.StageInfos)
                            {
                                if (sli.StageNo == ais.StageNo)
                                {
                                    stageInfos.Add(sli);
                                    foreach (StageToSpot.Entry sts in StageToSpot.Entries)
                                    {
                                        if (sts.StageNo == sli.StageNo)
                                        {
                                            spots.Add(sts);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // lookup area info for area
                    AreaList.AreaInfo areaInfo;
                    foreach (AreaList.AreaInfo ai in AreaList.AreaInfos)
                    {
                        if (ai.AreaId == landAreaId)
                        {
                            areaInfo = ai;
                        }
                    }
                }
            }

            List<JsonTest> tests = new List<JsonTest>();
            Gmd fieldAreaNames = GetResource<Gmd>(
                "game_common.arc",
                "ui/00_message/common/field_area_name",
                "gmd"
            );
            foreach (FieldAreaList.FieldAreaInfo fai in FieldAreaList.FieldAreaInfos)
            {
                Gmd.Entry areaName = fieldAreaNames.Entries[(int) fai.GmdIdx];
                FieldAreaMarkerInfo omMarker = GetResource<FieldAreaMarkerInfo>(
                    $"/FieldArea/FieldArea{fai.FieldAreaId:000}_marker.arc",
                    $"etc/FieldArea/FieldArea{fai.FieldAreaId:000}_marker_om",
                    "fmi"
                );
                FieldAreaMarkerInfo sceMarker = GetResource<FieldAreaMarkerInfo>(
                    $"/FieldArea/FieldArea{fai.FieldAreaId:000}_marker.arc",
                    $"etc/FieldArea/FieldArea{fai.FieldAreaId:000}_marker_sce",
                    "fmi"
                );
                FieldAreaMarkerInfo npcMarker = GetResource<FieldAreaMarkerInfo>(
                    $"/FieldArea/FieldArea{fai.FieldAreaId:000}_marker.arc",
                    $"etc/FieldArea/FieldArea{fai.FieldAreaId:000}_marker_npc",
                    "fmi"
                );
                if (npcMarker == null)
                {
                    // no npcs
                    continue;
                }
                
                FieldAreaMarkerInfo ectMarker = GetResource<FieldAreaMarkerInfo>(
                    $"/FieldArea/FieldArea{fai.FieldAreaId:000}_marker.arc",
                    $"etc/FieldArea/FieldArea{fai.FieldAreaId:000}_marker_ect",
                    "fmi"
                );
                // transition points
                FieldAreaAdjoinList adjoin = GetResource<FieldAreaAdjoinList>(
                    $"/FieldArea/FieldArea{fai.FieldAreaId:000}_marker.arc",
                    $"etc/FieldArea/FieldArea{fai.FieldAreaId:000}_adjoin",
                    "faa"
                );

                JsonTest test = new JsonTest();
                test.name = areaName.Msg;

                // lets try to build npc spots
                foreach (int stageNo in fai.StageNoList)
                {
                    foreach (FieldAreaMarkerInfo.MarkerInfo npc in npcMarker.MarkerInfos)
                    {
                        if (npc.StageNo == stageNo)
                        {
                            JsonTestNpc nt = new JsonTestNpc();
                            nt.x = npc.Pos.X;
                            nt.y = npc.Pos.Y;
                            nt.z = npc.Pos.Z;
                            nt.stage_no = npc.StageNo;
                            nt.group_no = npc.GroupNo;
                            nt.unique_id = npc.UniqueId;
                            test.npcs.Add(nt);
                        }
                    }
                }

                tests.Add(test);
            }

            string json = JsonSerializer.Serialize(tests);
            File.WriteAllText("F:\\npcs.json", json);
        }

        public class JsonTestNpc
        {
            public float x{ get; set; }
            public float y{ get; set; }
            public float z{ get; set; }
            public int stage_no{ get; set; }
            public uint group_no{ get; set; }
            public uint unique_id{ get; set; }
        }

        public class JsonTest
        {
            public List<JsonTestNpc> npcs{ get; set; }
            public string name{ get; set; }

            public JsonTest()
            {
                npcs = new List<JsonTestNpc>();
            }
        }

        private T GetFile<T>(string arcPath, string filePath, string ext = null) where T : ClientFile, new()
        {
            ArcArchive.ArcFile arcFile = GetArcFile(arcPath, filePath, ext);
            if (arcFile == null)
            {
                return null;
            }

            T file = new T();
            file.Open(arcFile.Data);
            return file;
        }

        private T GetResource<T>(string arcPath, string filePath, string ext = null) where T : ResourceFile, new()
        {
            ArcArchive.ArcFile arcFile = GetArcFile(arcPath, filePath, ext);
            if (arcFile == null)
            {
                return null;
            }

            T resource = new T();
            resource.Open(arcFile.Data);
            return resource;
        }

        private ArcArchive.ArcFile GetArcFile(string arcPath, string filePath, string ext = null)
        {
            string path = Path.Combine(_directory.FullName, Util.UnrootPath(arcPath));
            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                Logger.Error($"File does not exist. ({path})");
                return null;
            }

            ArcArchive archive = new ArcArchive();
            archive.Open(file.FullName);
            ArcArchive.FileIndexSearch search = ArcArchive.Search()
                .ByArcPath(filePath)
                .ByExtension(ext);
            ArcArchive.ArcFile arcFile = archive.GetFile(search);
            if (arcFile == null)
            {
                Logger.Error($"File:{filePath} could not be located in archive:{path}");
                return null;
            }

            return arcFile;
        }
    }
}
