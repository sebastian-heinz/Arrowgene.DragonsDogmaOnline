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

            if (parameter.Arguments[0] == "gmd2Csv" &&
                parameter.ArgumentMap.ContainsKey("gmdCsv") &&
                parameter.ArgumentMap.ContainsKey("romDir") &&
                parameter.ArgumentMap.ContainsKey("romLang"))
            {
                // Merge all .arc files of ROM dir into existing CSVs language column

                string gmdCsvArg = parameter.ArgumentMap["gmdCsv"];
                string romDirArg = parameter.ArgumentMap["romDir"];
                string romLangArg = parameter.ArgumentMap["romLang"];

                if (!Enum.TryParse<GuiMessage.Language>(romLangArg, out GuiMessage.Language romLanguage))
                {
                    Logger.Error($"Provided romLang:{romLangArg} is invalid");
                    return CommandResultType.Exit;
                }

                DirectoryInfo romDir = new DirectoryInfo(romDirArg);
                FileInfo gmdCsvFile = new FileInfo(gmdCsvArg);
                if (!romDir.Exists)
                {
                    Logger.Error($"Provided romDir:{romDirArg} does not exist");
                    return CommandResultType.Exit;
                }

                GmdCsv gmdCsvReader = new GmdCsv();
                List<GmdCsv.Entry> csvEntries = gmdCsvReader.ReadPath(gmdCsvFile.FullName);
                Dictionary<string, GmdCsv.Entry> csvIndex = new Dictionary<string, GmdCsv.Entry>();
                foreach (GmdCsv.Entry csvEntry in csvEntries)
                {
                    string hash = csvEntry.GetUniqueQualifierLanguageAgnostic();
                    if (!csvIndex.ContainsKey(hash))
                    {
                        csvIndex.Add(hash, csvEntry);
                    }
                    else
                    {
                        GmdCsv.Entry existingRomCsvEntry = csvIndex[hash];
                        Logger.Error($"Duplicate entry in CSV, dropping");
                    }
                }

                string[] files = Directory.GetFiles(romDir.FullName, "*.arc", SearchOption.AllDirectories);
                List<GmdCsv.Entry> romCsvEntries = new List<GmdCsv.Entry>();
                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo arcFile = new FileInfo(files[i]);
                    romCsvEntries.AddRange(ExtractGmdCsvEntries(arcFile, romLanguage));
                    Logger.Info($"Processing {i}/{files.Length} {arcFile.FullName}");
                }

                List<GmdCsv.Entry> resultCsvEntries = new List<GmdCsv.Entry>();
                foreach (GmdCsv.Entry romCsvEntry in romCsvEntries)
                {
                    string hash = romCsvEntry.GetUniqueQualifierLanguageAgnostic();
                    GmdCsv.Entry resultEntry;
                    if (csvIndex.ContainsKey(hash))
                    {
                        resultEntry = csvIndex[hash];
                        MergeGmdCsvLanguage(romCsvEntry, romLanguage, resultEntry);
                    }
                    else
                    {
                        resultEntry = romCsvEntry;
                    }

                    resultCsvEntries.Add(resultEntry);
                }

                string outPath = Path.Combine(gmdCsvFile.FullName);
                gmdCsvReader.WritePath(resultCsvEntries, outPath);
                Logger.Info($"Done: {outPath}");
                return CommandResultType.Exit;
            }


            if (parameter.Arguments[0] == "packGmd" &&
                parameter.ArgumentMap.ContainsKey("gmdCsv") &&
                parameter.ArgumentMap.ContainsKey("romDir") &&
                parameter.ArgumentMap.ContainsKey("romLang"))
            {
                // pack gmd.csv into all .arc files of rom
                string gmdCsvArg = parameter.ArgumentMap["gmdCsv"];
                string romDirArg = parameter.ArgumentMap["romDir"];
                string romLangArg = parameter.ArgumentMap["romLang"];

                if (!Enum.TryParse<GuiMessage.Language>(romLangArg, out GuiMessage.Language romLanguage))
                {
                    Logger.Error($"Provided romLang:{romLangArg} is invalid");
                    return CommandResultType.Exit;
                }

                DirectoryInfo romDir = new DirectoryInfo(romDirArg);
                FileInfo gmdCsvFile = new FileInfo(gmdCsvArg);
                if (!romDir.Exists)
                {
                    Logger.Error($"Provided romDir:{romDirArg} does not exist");
                    return CommandResultType.Exit;
                }

                Logger.Info($"Reading CSV: {gmdCsvFile.FullName}");
                GmdCsv gmdCsvReader = new GmdCsv();
                List<GmdCsv.Entry> csvEntries = gmdCsvReader.ReadPath(gmdCsvFile.FullName);
                Dictionary<string, List<GmdCsv.Entry>> gmdLookup = new Dictionary<string, List<GmdCsv.Entry>>();
                foreach (GmdCsv.Entry csvEntry in csvEntries)
                {
                    string gmdHash = csvEntry.ArcPath;
                    List<GmdCsv.Entry> gmds;
                    if (!gmdLookup.ContainsKey(gmdHash))
                    {
                        gmds = new List<GmdCsv.Entry>();
                        gmdLookup.Add(gmdHash, gmds);
                    }
                    else
                    {
                        gmds = gmdLookup[gmdHash];
                    }

                    gmds.Add(csvEntry);
                }
                
                int count = gmdLookup.Keys.Count;
                int current = 0;
                Dictionary<string, ArcArchive.ArcFile> archiveLookup = new Dictionary<string, ArcArchive.ArcFile>();
                foreach (string gmdHash in gmdLookup.Keys)
                {
                    current++;
                    List<GmdCsv.Entry> gmds = gmdLookup[gmdHash];
                    List<ArcArchive.ArcFile> gmdFiles;
                    string path = Path.Combine(romDir.FullName, Util.UnrootPath(gmdHash));
                    using (FileStream fs = new FileStream(path, FileMode.Open))
                    {
                        List<ArcArchive.FileIndex> gmdIndices = ArcArchive.IndexProbeStream(fs,
                            ArcArchive.Search().ByExtension("gmd")
                        );
                        gmdFiles = ArcArchive.ReadFileStream(fs, gmdIndices);
                    }

                    string gmdPath = gmds[0].GmdPath;
                    foreach (ArcArchive.ArcFile arcArchiveFile in gmdFiles)
                    {
                        if (gmdPath == arcArchiveFile.Index.Path)
                        {
                            archiveLookup.Add(gmdHash, arcArchiveFile);
                            break;
                        }
                    }
                    Logger.Info($"Creating Lookup {current}/{count} {path}");
                }


                current = 0;
                foreach (string gmdHash in gmdLookup.Keys)
                {
                    current++;
                    ArcArchive.ArcFile arcFile = archiveLookup[gmdHash];
                    List<GmdCsv.Entry> gmdIntermediates = gmdLookup[gmdHash];
                    List<GuiMessage.Entry> guiMessages = new List<GuiMessage.Entry>();
                    foreach (GmdCsv.Entry csvEntry in gmdIntermediates)
                    {
                        GuiMessage.Entry newEntry = new GuiMessage.Entry();
                        newEntry.Index = csvEntry.Index;
                        newEntry.Key = csvEntry.Key;
                        if (romLanguage == GuiMessage.Language.Japanese)
                        {
                            newEntry.Msg = csvEntry.MsgJp;
                        }
                        else if (romLanguage == GuiMessage.Language.English)
                        {
                            newEntry.Msg = csvEntry.MsgEn;
                        }
                        else
                        {
                            Logger.Error($"Language {romLanguage} not supported");
                        }

                        newEntry.ReadIndex = csvEntry.ReadIndex;
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
                    string path = Path.Combine(romDir.FullName, Util.UnrootPath(gmdHash));
                    archive.Open(path);
                    archive.PutFile(arcFile.Index.Path, arcFile.Data);
                    byte[] savedArc = archive.Save();
                    File.WriteAllBytes(path, savedArc);
                    Logger.Info($"Writing {current}/{count} {path}");
                }

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

            return CommandResultType.Exit;
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
                foreach (ArcArchive.ArcFile file in archive.GetFiles())
                {
                    ArcArchive.FileIndex fi = file.Index;
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
            foreach (ArcArchive.ArcFile af in archive.GetFiles())
            {
                ArcArchive.FileIndex fi = af.Index;
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

        private void MergeGmdCsvLanguage(GmdCsv.Entry entrySrc, GuiMessage.Language langSrc, GmdCsv.Entry entryDst)
        {
            switch (langSrc)
            {
                case GuiMessage.Language.Japanese:
                    entryDst.MsgJp = entrySrc.MsgJp;
                    break;
                case GuiMessage.Language.English:
                    entryDst.MsgEn = entrySrc.MsgEn;
                    break;
                default:
                    Logger.Error($"Language {langSrc} not supported");
                    break;
            }
        }

        private List<GmdCsv.Entry> ExtractGmdCsvEntries(FileInfo arcFile, GuiMessage.Language language)
        {
            List<GmdCsv.Entry> csvEntries = new List<GmdCsv.Entry>();

            if (!arcFile.Exists || arcFile.Extension != ".arc")
            {
                Logger.Error($"Source file not exists or is not a .arc file. ({arcFile.FullName})");
                return csvEntries;
            }

            List<ArcArchive.ArcFile> gmdFiles;
            using (FileStream fs = new FileStream(arcFile.FullName, FileMode.Open))
            {
                List<ArcArchive.FileIndex> gmdIndices = ArcArchive.IndexProbeStream(fs,
                    ArcArchive.Search().ByExtension("gmd")
                );
                gmdFiles = ArcArchive.ReadFileStream(fs, gmdIndices);
            }

            foreach (ArcArchive.ArcFile gmdFile in gmdFiles)
            {
                GuiMessage gmd = new GuiMessage();
                gmd.Open(gmdFile.Data);
                foreach (GuiMessage.Entry gmdEntry in gmd.Entries)
                {
                    GmdCsv.Entry csvEntry = new GmdCsv.Entry();
                    csvEntry.Index = gmdEntry.Index;
                    csvEntry.Key = gmdEntry.Key;

                    if (language == GuiMessage.Language.Japanese)
                    {
                        csvEntry.MsgJp = gmdEntry.Msg;
                    }
                    else if (language == GuiMessage.Language.English)
                    {
                        csvEntry.MsgEn = gmdEntry.Msg;
                    }
                    else
                    {
                        Logger.Error($"Language {language} not supported");
                    }

                    csvEntry.GmdPath = gmdFile.Index.Path;
                    csvEntry.ArcName = arcFile.Name;
                    csvEntry.ReadIndex = gmdEntry.ReadIndex;
                    string search = "nativePC\\rom";
                    int romIdx = arcFile.FullName.IndexOf(search, StringComparison.InvariantCultureIgnoreCase);
                    if (romIdx == -1)
                    {
                        search = "nativePC/rom";
                        romIdx = arcFile.FullName.IndexOf(search, StringComparison.InvariantCultureIgnoreCase);
                    }

                    if (romIdx >= 0)
                    {
                        csvEntry.ArcPath = arcFile.FullName.Substring(romIdx + search.Length);
                    }

                    csvEntries.Add(csvEntry);
                }
            }

            return csvEntries;
        }

        public void Shutdown()
        {
        }
    }
}
