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
            
            // \stage\st0100\st0100.arc,scr\st0100\etc\st0100.lcd,lcd,0x68BFA317,rLocationData,1757389591,2949,1952,139728
            LocationData st0100 = GetResource<LocationData>("stage/st0100/st0100.arc", "scr/st0100/etc/st0100","lcd");
            string json1 = JsonSerializer.Serialize(st0100.Entries);
            File.WriteAllText("F:\\st0100.json", json1);
            

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
                   // continue;
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
                List<FieldAreaMarkerInfo.MarkerInfo> joined = new List<FieldAreaMarkerInfo.MarkerInfo>();
                if (omMarker != null)
                {
                    joined.AddRange(omMarker.MarkerInfos);
                    foreach (FieldAreaMarkerInfo.MarkerInfo npc in omMarker.MarkerInfos)
                    {
                        if (npc.StageNo == 100)
                        {
                            JsonTestNpc nt = new JsonTestNpc();
                            nt.Type = "om";
                            nt.X = npc.Pos.X;
                            nt.Y = npc.Pos.Y;
                            nt.Z = npc.Pos.Z;
                            nt.StageNo = npc.StageNo;
                            nt.GroupNo = npc.GroupNo;
                            nt.UniqueId = npc.UniqueId;
                            test.npcs.Add(nt);
                        }
                    }
                }
                if (sceMarker != null)
                {
                    joined.AddRange(sceMarker.MarkerInfos);
                    foreach (FieldAreaMarkerInfo.MarkerInfo npc in sceMarker.MarkerInfos)
                    {
                        if (npc.StageNo == 100)
                        {
                            JsonTestNpc nt = new JsonTestNpc();
                            nt.Type = "sce";
                            nt.X = npc.Pos.X;
                            nt.Y = npc.Pos.Y;
                            nt.Z = npc.Pos.Z;
                            nt.StageNo = npc.StageNo;
                            nt.GroupNo = npc.GroupNo;
                            nt.UniqueId = npc.UniqueId;
                            test.npcs.Add(nt);
                        }
                    }
                }
                if (npcMarker != null)
                {
                    joined.AddRange(npcMarker.MarkerInfos);
                    foreach (FieldAreaMarkerInfo.MarkerInfo npc in npcMarker.MarkerInfos)
                    {
                        if (npc.StageNo == 100)
                        {
                            JsonTestNpc nt = new JsonTestNpc();
                            nt.Type = "npc";
                            nt.X = npc.Pos.X;
                            nt.Y = npc.Pos.Y;
                            nt.Z = npc.Pos.Z;
                            nt.StageNo = npc.StageNo;
                            nt.GroupNo = npc.GroupNo;
                            nt.UniqueId = npc.UniqueId;
                            test.npcs.Add(nt);
                        }
                    }
                }
                if (ectMarker != null)
                {
                    joined.AddRange(ectMarker.MarkerInfos);
                    foreach (FieldAreaMarkerInfo.MarkerInfo npc in ectMarker.MarkerInfos)
                    {
                        if (npc.StageNo == 100)
                        {
                            JsonTestNpc nt = new JsonTestNpc();
                            nt.Type = "ect";
                            nt.X = npc.Pos.X;
                            nt.Y = npc.Pos.Y;
                            nt.Z = npc.Pos.Z;
                            nt.StageNo = npc.StageNo;
                            nt.GroupNo = npc.GroupNo;
                            nt.UniqueId = npc.UniqueId;
                            test.npcs.Add(nt);
                        }
                    }
                }
                // lets try to build npc spots
            //    foreach (int stageNo in fai.StageNoList)
              //  {
                    foreach (FieldAreaMarkerInfo.MarkerInfo npc in joined)
                    {
                        if (npc.StageNo == 100)
                        {
                            JsonTestNpc nt = new JsonTestNpc();
                            nt.X = npc.Pos.X;
                            nt.Y = npc.Pos.Y;
                            nt.Z = npc.Pos.Z;
                            nt.StageNo = npc.StageNo;
                            nt.GroupNo = npc.GroupNo;
                            nt.UniqueId = npc.UniqueId;
                         //   test.npcs.Add(nt);
                        }
                 //   }
                }

                tests.Add(test);
            }

            string json = JsonSerializer.Serialize(tests);
            File.WriteAllText("F:\\npcs.json", json);
        }

        public class JsonTestNpc
        {
            public string Type{ get; set; }
            public float X{ get; set; }
            public float Y{ get; set; }
            public float Z{ get; set; }
            public int StageNo{ get; set; }
            public uint GroupNo{ get; set; }
            public uint UniqueId{ get; set; }
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
