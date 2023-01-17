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
            if (parameter.Arguments.Count < 1)
            {
                Logger.Error($"To few arguments. {Description}");
                return CommandResultType.Exit;
            }

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
                if (!arcFile.Exists || arcFile.Extension != ".arc")
                {
                    Logger.Error($"Source file not exists or is not a .arc file. ({source})");
                    return CommandResultType.Exit;
                }

                ArcArchive archive = new ArcArchive();
                archive.Open(arcFile.FullName);
                List<ArcArchive.ArcFile> gmdFiles = archive.GetFiles(
                    ArcArchive.Search().ByExtension("gmd")
                );

                StringBuilder sb = new StringBuilder();
                sb.Append("Index, key, Msg, a2, a3, a4, a5, Path");
                sb.Append($"{Environment.NewLine}");
                foreach (ArcArchive.ArcFile gmdFile in gmdFiles)
                {
                    GuiMessage gmd = new GuiMessage();
                    gmd.Open(gmdFile.Data);

                    foreach (GuiMessage.Entry gmdEntry in gmd.Entries)
                    {
                        sb.Append($"{gmdEntry.Index},");
                        sb.Append($"{gmdEntry.Key},");
                        sb.Append($"{gmdEntry.Msg},");
                        sb.Append($"{gmdEntry.a2},");
                        sb.Append($"{gmdEntry.a3},");
                        sb.Append($"{gmdEntry.a4},");
                        sb.Append($"{gmdEntry.a5},");
                        sb.Append($"{gmdFile.Index.Path}");
                        sb.Append($"{Environment.NewLine}");
                    }
                }

                string outPath = Path.Combine(outDir.FullName, arcFile.Name, ".csv");
                File.WriteAllText(outPath, sb.ToString(), Encoding.UTF8);
                Logger.Info($"Written gmd to: {outPath}");

                return CommandResultType.Exit;
            }

            if (parameter.ArgumentMap.ContainsKey("packGmd"))
            {
                string packGmd = parameter.ArgumentMap["packGmd"];
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
