using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using Arrowgene.Ddon.Client;
using Arrowgene.Ddon.Client.Resource;
using Arrowgene.Ddon.Client.Resource.Texture;
using Arrowgene.Ddon.Client.Resource.Texture.Dds;
using Arrowgene.Ddon.Client.Resource.Texture.Tex;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Csv;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Cli.Command
{
    public class ClientCommand : ICommand
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(ClientCommand));

        public string Key => "client";

        public string Description => "Usage: `client \"E:\\Games\\Dragon's Dogma Online\\nativePC\\rom\"`";


        public CommandResultType Run(CommandParameter parameter)
        {
            if (parameter.ArgumentMap.ContainsKey("extract"))
            {
                string extract = parameter.ArgumentMap["extract"];
                extract = extract.TrimEnd(Path.DirectorySeparatorChar);
                extract = extract.TrimEnd(Path.AltDirectorySeparatorChar);
                DirectoryInfo outDirectory = new DirectoryInfo(extract);
                if (!outDirectory.Exists)
                {
                    Logger.Error($"extract directory does not exists ({outDirectory.FullName})");
                    return CommandResultType.Exit;
                }

                string source = parameter.Arguments[0];
                if (File.Exists(source))
                {
                    FileInfo arcFile = new FileInfo(source);
                    if (arcFile.Extension != ".arc")
                    {
                        Logger.Error($"Source file is not a .arc file. ({source})");
                        return CommandResultType.Exit;
                    }

                    Extract(arcFile, outDirectory);
                    return CommandResultType.Exit;
                }

                if (Directory.Exists(source))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(source);
                    Extract(directoryInfo, outDirectory);
                    return CommandResultType.Exit;
                }

                Logger.Error($"Source file or folder does not exist. ({outDirectory.FullName})");
                return CommandResultType.Exit;
            }

            if (parameter.ArgumentMap.ContainsKey("extractGmd"))
            {
                string extractGmd = parameter.ArgumentMap["extractGmd"];
                DirectoryInfo outDir = new DirectoryInfo(extractGmd);
                if (!outDir.Exists)
                {
                    Logger.Error($"Directory does not exists. ({extractGmd})");
                    return CommandResultType.Exit;
                }

                string source = parameter.Arguments[0];
                FileInfo arcFile = new FileInfo(source);

                List<GmdIntermediateContainer> containers = ExtractGmdContainer(arcFile);

                string outPath = Path.Combine(outDir.FullName, arcFile.Name + ".csv");
                GmdCsv writer = new GmdCsv(false);
                writer.WritePath(containers, outPath);
                Logger.Info($"Written gmd to: {outPath}");

                return CommandResultType.Exit;
            }

            if (parameter.ArgumentMap.ContainsKey("packGmdCsv")
                && parameter.ArgumentMap.ContainsKey("packGmdRom"))
            {
                string packGmdCsv = parameter.ArgumentMap["packGmdCsv"];
                string packGmdRom = parameter.ArgumentMap["packGmdRom"];

                GmdCsv gmdCsvReader = new GmdCsv(true);
                List<GmdIntermediateContainer> gmdCsv = gmdCsvReader.ReadPath(packGmdCsv);
                Dictionary<string, List<GmdIntermediateContainer>> gmdLookup =
                    new Dictionary<string, List<GmdIntermediateContainer>>();
                foreach (GmdIntermediateContainer gmd in gmdCsv)
                {
                    string gmdHash = gmd.ArcPath;
                    List<GmdIntermediateContainer> gmds;
                    if (!gmdLookup.ContainsKey(gmdHash))
                    {
                        gmds = new List<GmdIntermediateContainer>();
                        gmdLookup.Add(gmdHash, gmds);
                    }
                    else
                    {
                        gmds = gmdLookup[gmdHash];
                    }

                    gmds.Add(gmd);
                }

                Dictionary<string, ArcArchive.ArcFile> archiveLookup =
                    new Dictionary<string, ArcArchive.ArcFile>();
                foreach (string gmdHash in gmdLookup.Keys)
                {
                    List<GmdIntermediateContainer> gmds = gmdLookup[gmdHash];
                    ArcArchive archive = new ArcArchive();
                    string path = Path.Combine(packGmdRom, Util.UnrootPath(gmdHash));
                    archive.Open(path);
                    List<ArcArchive.ArcFile> gmdFiles = archive.GetFiles(
                        ArcArchive.Search().ByExtension("gmd")
                    );
                    string gmdPath = gmds[0].GmdPath;
                    foreach (ArcArchive.ArcFile arcArchiveFile in gmdFiles)
                    {
                        if (gmdPath == arcArchiveFile.Index.Path)
                        {
                            archiveLookup.Add(gmdHash, arcArchiveFile);
                            break;
                        }
                    }
                }

                foreach (string gmdHash in gmdLookup.Keys)
                {
                    ArcArchive.ArcFile arcFile = archiveLookup[gmdHash];
                    List<GmdIntermediateContainer> gmdIntermediates = gmdLookup[gmdHash];
                    List<GuiMessage.Entry> guiMessages = new List<GuiMessage.Entry>();
                    foreach (GmdIntermediateContainer gmdIntermediate in gmdIntermediates)
                    {
                        GuiMessage.Entry newEntry = new GuiMessage.Entry();
                        newEntry.Index = gmdIntermediate.Index;
                        newEntry.Key = gmdIntermediate.Key;
                        newEntry.Msg = gmdIntermediate.MsgEn;
                        newEntry.a2 = gmdIntermediate.a2;
                        newEntry.a3 = gmdIntermediate.a3;
                        newEntry.a4 = gmdIntermediate.a4;
                        newEntry.a5 = gmdIntermediate.a5;
                        newEntry.KeyReadIndex = gmdIntermediate.KeyReadIndex;
                        newEntry.MsgReadIndex = gmdIntermediate.MsgReadIndex;
                        guiMessages.Add(newEntry);
                    }

                    GuiMessage gmd = new GuiMessage();
                    gmd.Open(arcFile.Data);
                    if (gmd.Entries.Count != guiMessages.Count)
                    {
                      Logger.Error("gmd.Entries.Count != guiMessages.Count");
                    }

                    gmd.Entries.Clear();
                    gmd.Entries.AddRange(guiMessages);
                    arcFile.Data = gmd.Save();
                    
                    ArcArchive archive = new ArcArchive();
                    string path = Path.Combine(packGmdRom, Util.UnrootPath(gmdHash));
                    path = "C:\\Users\\nxspirit\\Downloads\\character_edit_select - Copy.arc";
                    archive.Open(path);
                    List<ArcArchive.ArcFile> testSearch = archive.GetFiles(
                        ArcArchive.Search()
                    );
                    archive.PutFile(arcFile.Index.Path, arcFile.Data);
                   // byte[] newArchive = archive.Save();
                  //  File.WriteAllBytes("C:\\Users\\nxspirit\\Downloads\\" + gmdHash + ".new", newArchive);
                    
                    break;
                }


                return CommandResultType.Exit;
            }

            if (parameter.ArgumentMap.ContainsKey("gmdOrg")
                && parameter.ArgumentMap.ContainsKey("gmdEn")
                && parameter.ArgumentMap.ContainsKey("gmdOut"))
            {
                string gmdCsvOrg = parameter.ArgumentMap["gmdOrg"];
                string gmdCsvEn = parameter.ArgumentMap["gmdEn"];
                string gmdOut = parameter.ArgumentMap["gmdOut"];

                GmdCsv gmdCsvReader = new GmdCsv(false);

                // index english
                List<GmdIntermediateContainer> gmdContainerEn = gmdCsvReader.ReadPath(gmdCsvEn);
                Dictionary<string, GmdIntermediateContainer> gmdIndexEn =
                    new Dictionary<string, GmdIntermediateContainer>();
                foreach (GmdIntermediateContainer gmdEn in gmdContainerEn)
                {
                    string gmdHash = gmdEn.GetUniqueQualifierLanguageAgnostic();
                    if (!gmdIndexEn.ContainsKey(gmdHash))
                    {
                        gmdIndexEn.Add(gmdHash, gmdEn);
                    }
                    else
                    {
                        GmdIntermediateContainer gmdEnEx = gmdIndexEn[gmdHash];
                        int i = 1;
                    }
                }

                // enrich original
                List<GmdIntermediateContainer> gmdContainerOrg = gmdCsvReader.ReadPath(gmdCsvOrg);
                foreach (GmdIntermediateContainer gmdOrg in gmdContainerOrg)
                {
                    string gmdOrgHash = gmdOrg.GetUniqueQualifierLanguageAgnostic();
                    if (gmdIndexEn.ContainsKey(gmdOrgHash))
                    {
                        GmdIntermediateContainer gmdEn = gmdIndexEn[gmdOrgHash];
                        if (gmdOrg.MsgOrg != gmdEn.MsgOrg)
                        {
                            gmdOrg.MsgEn = gmdEn.MsgOrg;
                        }
                    }
                }

                string outPath = Path.Combine(gmdOut, "gmd_merged.csv");
                GmdCsv gmdCsvWriter = new GmdCsv(true);
                gmdCsvWriter.WritePath(gmdContainerOrg, outPath);
                Logger.Info($"Done: {outPath}");

                return CommandResultType.Exit;
            }


            if (parameter.Arguments.Count < 1)
            {
                Logger.Error($"To few arguments. {Description}");
                return CommandResultType.Exit;
            }

            FileInfo fileInfo = new FileInfo(parameter.Arguments[0]);
            if (fileInfo.Exists)
            {
                if (".tex".Equals(fileInfo.Extension, StringComparison.InvariantCultureIgnoreCase))
                {
                    TexToDds(fileInfo);
                    return CommandResultType.Exit;
                }

                if (".dds".Equals(fileInfo.Extension, StringComparison.InvariantCultureIgnoreCase))
                {
                    DdsToTex(fileInfo, parameter);
                    return CommandResultType.Exit;
                }

                Logger.Error("No available action for provided file");
                return CommandResultType.Exit;
            }

            // assuming first parameter is rom dir for these actions
            DirectoryInfo romDirectory = new DirectoryInfo(parameter.Arguments[0]);
            if (!romDirectory.Exists)
            {
                Logger.Error("Rom Path Invalid");
                return CommandResultType.Exit;
            }

            if (parameter.ArgumentMap.ContainsKey("dump"))
            {
                DirectoryInfo outDirectory = new DirectoryInfo(parameter.ArgumentMap["dump"]);
                DumpPaths(romDirectory, outDirectory);
                return CommandResultType.Exit;
            }

            if (parameter.ArgumentMap.ContainsKey("export"))
            {
                DirectoryInfo outDirectory = new DirectoryInfo(parameter.ArgumentMap["export"]);
                ExportResourceRepository(romDirectory, outDirectory);
                return CommandResultType.Exit;
            }

            if (parameter.ArgumentMap.ContainsKey("extractAllGmd"))
            {
                DirectoryInfo outDirectory = new DirectoryInfo(parameter.ArgumentMap["extractAllGmd"]);
                if (!outDirectory.Exists)
                {
                    outDirectory.Create();
                    Logger.Info($"Created Dir: {outDirectory.FullName}");
                }

                string[] files = Directory.GetFiles(romDirectory.FullName, "*.arc", SearchOption.AllDirectories);
                List<GmdIntermediateContainer> containers = new List<GmdIntermediateContainer>();
                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo arcFile = new FileInfo(files[i]);
                    containers.AddRange(ExtractGmdContainer(arcFile));
                    Logger.Info($"Processing {i}/{files.Length} {arcFile.FullName}");
                }

                string outPath = Path.Combine(outDirectory.FullName, "gmd.csv");
                GmdCsv writer = new GmdCsv(false);
                writer.WritePath(containers, outPath);

                Logger.Info($"Done: {outPath}");
                return CommandResultType.Exit;
            }

            return CommandResultType.Exit;
        }

        private List<GmdIntermediateContainer> ExtractGmdContainer(FileInfo arcFile)
        {
            List<GmdIntermediateContainer> containers = new List<GmdIntermediateContainer>();

            if (!arcFile.Exists || arcFile.Extension != ".arc")
            {
                Logger.Error($"Source file not exists or is not a .arc file. ({arcFile.FullName})");
                return containers;
            }

            ArcArchive archive = new ArcArchive();
            archive.Open(arcFile.FullName);
            List<ArcArchive.ArcFile> gmdFiles = archive.GetFiles(
                ArcArchive.Search().ByExtension("gmd")
            );

            foreach (ArcArchive.ArcFile gmdFile in gmdFiles)
            {
                GuiMessage gmd = new GuiMessage();
                gmd.Open(gmdFile.Data);
                foreach (GuiMessage.Entry gmdEntry in gmd.Entries)
                {
                    GmdIntermediateContainer container = new GmdIntermediateContainer();
                    container.Index = gmdEntry.Index;
                    container.Key = gmdEntry.Key;
                    container.MsgOrg = gmdEntry.Msg;
                    container.a2 = gmdEntry.a2;
                    container.a3 = gmdEntry.a3;
                    container.a4 = gmdEntry.a4;
                    container.a5 = gmdEntry.a5;
                    container.GmdPath = gmdFile.Index.Path;
                    container.ArcName = arcFile.Name;
                    string search = "nativePC\\rom";
                    int romIdx = arcFile.FullName.IndexOf(search, StringComparison.InvariantCultureIgnoreCase);
                    if (romIdx == -1)
                    {
                        search = "nativePC/rom";
                        romIdx = arcFile.FullName.IndexOf(search, StringComparison.InvariantCultureIgnoreCase);
                    }

                    if (romIdx >= 0)
                    {
                        container.ArcPath = arcFile.FullName.Substring(romIdx + search.Length);
                    }

                    container.KeyReadIndex = gmdEntry.KeyReadIndex;
                    container.MsgReadIndex = gmdEntry.MsgReadIndex;
                    container.Str = gmd.Str;
                    containers.Add(container);
                }
            }

            return containers;
        }

        public void ExportResourceRepository(DirectoryInfo romDirectory, DirectoryInfo outDir)
        {
            ClientResourceRepository repo = new ClientResourceRepository();
            repo.Load(romDirectory);
            string json = JsonSerializer.Serialize(repo);
            string outPath = Path.Combine(outDir.FullName, "repo.json");
            File.WriteAllText(outPath, json);
            Logger.Info($"Done: {outPath}");
        }

        public void DumpPaths(DirectoryInfo romDirectory, DirectoryInfo outDir)
        {
            if (outDir == null)
            {
                Logger.Error("Failed to dump paths. (outDir == null)");
                return;
            }

            if (!outDir.Exists)
            {
                outDir.Create();
                Logger.Info($"Created Dir: {outDir.FullName}");
            }

            StringBuilder sb = new StringBuilder();
            sb.Append($"Path,ArcPath,Ext,JamCrcStr,Class,JamCrc,Size,SizeCompress,Offset{Environment.NewLine}");
            string[] files = Directory.GetFiles(romDirectory.FullName, "*.arc", SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; i++)
            {
                string filePath = files[i];
                string relativePath = filePath.Substring(romDirectory.FullName.Length);
                ArcArchive archive = new ArcArchive();
                archive.Open(filePath);
                foreach (ArcArchive.FileIndex fi in archive.GetFileIndices())
                {
                    sb.Append($"{relativePath},{fi.ArcPath}.{fi.ArcExt.Extension},{fi.ArcExt.Extension},");
                    sb.Append($"{fi.ArcExt.JamCrcStr},{fi.ArcExt.Class},{fi.JamCrc},");
                    sb.Append($"{fi.Size},{fi.CompressedSize},{fi.Offset}{Environment.NewLine}");
                }

                Logger.Info($"Processing {i}/{files.Length} {filePath}");
            }

            string outPath = Path.Combine(outDir.FullName, "dump.csv");
            File.WriteAllText(outPath, sb.ToString());
            Logger.Info($"Done: {outPath}");
        }

        public void Extract(DirectoryInfo source, DirectoryInfo outDir)
        {
            if (outDir == null)
            {
                Logger.Error("Failed to extract. (outDir == null)");
                return;
            }

            if (!outDir.Exists)
            {
                outDir.Create();
                Logger.Info($"Created Output Directory: {outDir.FullName}");
            }

            string[] files = Directory.GetFiles(source.FullName, "*.arc", SearchOption.AllDirectories);

            if (files.Length <= 0)
            {
                Logger.Error($"No *.arc files found in source directory. ({source.FullName})");
                return;
            }

            for (int i = 0; i < files.Length; i++)
            {
                string filePath = files[i];
                FileInfo fi = new FileInfo(filePath);
                Extract(fi, outDir);
                Logger.Info($"Processing {i}/{files.Length} {filePath}");
            }
        }

        public void Extract(FileInfo fileInfo, DirectoryInfo outDir)
        {
            ArcArchive archive = new ArcArchive();
            archive.Open(fileInfo.FullName);
            foreach (ArcArchive.FileIndex fi in archive.GetFileIndices())
            {
                ArcArchive.ArcFile af = archive.GetFile(fi);
                if (af == null)
                {
                    continue;
                }

                string outDirectory = Path.Combine(outDir.FullName, fileInfo.Name, fi.Directory);
                if (!Directory.Exists(outDirectory))
                {
                    Directory.CreateDirectory(outDirectory);
                }

                string outPath = Path.Combine(outDirectory, fi.Name);
                File.WriteAllBytes(outPath, af.Data);

                Logger.Info($"Written: {outPath}");
            }
        }

        public void TexToDds(FileInfo fileInfo)
        {
            TexTexture texTexture = new TexTexture();
            texTexture.Open(fileInfo.FullName);
            DdsTexture ddsTexture = TexConvert.ToDdsTexture(texTexture);
            string outPath = $"{fileInfo.FullName}.dds";
            ddsTexture.Save(outPath);
            Logger.Info($"Written: {outPath}");
        }

        public void DdsToTex(FileInfo fileInfo, CommandParameter parameter)
        {
            TexHeaderVersion headerVersion = TexHeaderVersion.Ddon;
            if (parameter.Switches.Contains("--ddda"))
            {
                headerVersion = TexHeaderVersion.Ddda;
            }
            else if (parameter.Switches.Contains("--ddon"))
            {
                headerVersion = TexHeaderVersion.Ddon;
            }

            DdsTexture ddsTexture = new DdsTexture();
            ddsTexture.Open(fileInfo.FullName);
            TexTexture texTexture = TexConvert.ToTexTexture(ddsTexture, headerVersion);
            string outPath = $"{fileInfo.FullName}.tex";
            texTexture.Save(outPath);
            Logger.Info($"Written: {outPath}");
        }

        public void Shutdown()
        {
        }
    }
}
