using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arrowgene.Ddon.Client.Resource;
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

        public void Load(string romDirectory)
        {
            _directory = new DirectoryInfo(romDirectory);
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

                    // unsure what field area is
                    // FieldAreaList.FieldAreaInfo  fieldAreaInfo;
                    // foreach (FieldAreaList.FieldAreaInfo fai in FieldAreaList.FieldAreaInfos)
                    // {
                    //     if (fai.AreaId == landAreaId)
                    //     {
                    //         fieldAreaInfo = fai;
                    //     }
                    // }
                }
            }


            // MsgSet = GetResource<MsgSet>("stage/st0100/st0100.arc",
            //     "ui/00_message/examine_message/stage/stage_examine_st0100", "mss");

            // MsgSet msgSet = GetResource<MsgSet>("game_common.arc", "ui/00_message/pw/pwtlk05", "mss");
            //Gmd gmd = GetResource<Gmd>("game_common.arc", "ui/00_message/pw/pwtlk05", "gmd");


            //   var fas = GetArcFile("game_common.arc", "ui/00_message/pw/pwtlk05", "gmd");
            //   File.WriteAllBytes("F:\\asda",fas.Data);
            Gmd gmd = new Gmd();

            // LandList -> AreaList 
            //           -> AreaStageList -> stage no  -> stage list
            //           -> FieldAreaList -> stagNo list  -> stage list


            // gmd.Open("E:\\Games\\ARCtool\\st0100\\ui\\00_message\\examine_message\\stage\\stage_examine_st0100.gmd");
            //   gmd.Open("E:\\Games\\ARCtool\\game_common\\ui\\00_message\\npc\\func_select_name.gmd");
            gmd.Open("E:\\Games\\ARCtool\\game_common\\ui\\00_message\\common\\field_area_name.gmd");
            int i = 1;
        }

        public void DumpPaths(string outPath)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Path,ArcPath,JamCrc{Environment.NewLine}");
            string[] files = Directory.GetFiles(_directory.FullName, "*.arc", SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; i++)
            {
                string filePath = files[i];
                string relativePath = filePath.Substring(_directory.FullName.Length);
                ArcArchive archive = new ArcArchive();
                archive.Open(filePath);
                foreach (ArcArchive.FileIndex fi in archive.GetFileIndices())
                {
                    sb.Append($"{relativePath},{fi.ArcPath}.{fi.ArcExt.Extension},{fi.JamCrc}{Environment.NewLine}");
                }

                Logger.Info($"Processing {i}/{files.Length} {filePath}");
            }

            File.WriteAllText(outPath, sb.ToString());
            Logger.Info($"Done: {outPath}");
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
            string path = Path.Combine(_directory.FullName, arcPath);
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
