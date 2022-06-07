using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Arrowgene.Ddon.Client;
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


            FileInfo fileInfo = new FileInfo(parameter.Arguments[0]);
            if (fileInfo.Exists)
            {
                if (".tex".Equals(fileInfo.Extension, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (parameter.Switches.Contains("--meta"))
                    {
                        WriteTexMeta(fileInfo);
                        return CommandResultType.Exit;
                    }
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

                string outDirectory = Path.Combine(outDir.FullName, $"{fileInfo.Name}.extract", fi.Directory);
                if (!Directory.Exists(outDirectory))
                {
                    Directory.CreateDirectory(outDirectory);
                }

                string outPath = Path.Combine(outDirectory, fi.Name);
                File.WriteAllBytes(outPath, af.Data);

                Logger.Info($"Written: {outPath}");
            }
        }

        public void WriteTexMeta(FileInfo fileInfo)
        {
            string path = fileInfo.FullName;
            TexTexture texTexture = new TexTexture();
            texTexture.Open(path);
            File.WriteAllText($"{path}.meta", texTexture.Header.GetMetadata());
        }

        public void TexToDds(FileInfo fileInfo)
        {
            string path = fileInfo.FullName;

            TexTexture texTexture = new TexTexture();
            texTexture.Open(path);
            DdsTexture ddsTexture = TexConvert.ToDdsTexture(texTexture);
            if (ddsTexture == null)
            {
                Logger.Error($"Failed to convert Tex->Dds. ({path})");
                return;
            }
            
            
            string outPath = $"{path}.dds";

            if (texTexture.Header.HasSphericalHarmonicsFactor)
            {
                if (!texTexture.SphericalHarmonics.Loaded)
                {
                    Logger.Error("HasSphericalHarmonicsFactor defined but not marked as loaded, writing anyways.");
                }

                File.WriteAllBytes($"{path}.shfactor", texTexture.SphericalHarmonics.Encode());
            }

            File.WriteAllText($"{path}.meta", texTexture.Header.GetMetadata());

            ddsTexture.Save(outPath);
            Logger.Info($"Written: {outPath}");
        }

        public void DdsToTex(FileInfo fileInfo, CommandParameter parameter)
        {
            string path = fileInfo.FullName;

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
            ddsTexture.Open(path);

            TexSphericalHarmonics? sphericalHarmonics = null;
            string shFactorFile = null;
            if (path.LastIndexOf('.') > 0)
            {
                shFactorFile = path.Substring(0, path.LastIndexOf('.'));
                shFactorFile += ".shfactor";
            }

            if (File.Exists(shFactorFile))
            {
                byte[] shFactor = File.ReadAllBytes(shFactorFile);
                TexSphericalHarmonics tmpSphericalHarmonics = new TexSphericalHarmonics();
                tmpSphericalHarmonics.Decode(shFactor);
                sphericalHarmonics = tmpSphericalHarmonics;
            }

            TexTexture texTexture = TexConvert.ToTexTexture(ddsTexture, headerVersion, sphericalHarmonics);
            if (texTexture == null)
            {
                Logger.Error($"Failed to convert Dds->Tex. ({path})");
                return;
            }

            string outPath = $"{fileInfo.FullName}.tex";
            texTexture.Save(outPath);
            Logger.Info($"Written: {outPath}");
        }

        public void Shutdown()
        {
        }
    }
}
