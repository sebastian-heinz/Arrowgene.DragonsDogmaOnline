using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Arrowgene.Ddon.Client;
using Arrowgene.Ddon.Client.Resource;
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

            if (parameter.ArgumentMap.ContainsKey("extract"))
            {
                DirectoryInfo outDirectory = new DirectoryInfo(parameter.ArgumentMap["extract"]);
                Extract(romDirectory, outDirectory);
                return CommandResultType.Exit;
            }
            
            if (parameter.ArgumentMap.ContainsKey("tex2dds"))
            {
                DirectoryInfo outDirectory = new DirectoryInfo(parameter.ArgumentMap["tex2dds"]);
                TexToDds(romDirectory, outDirectory);
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

        public void Extract(DirectoryInfo romDirectory, DirectoryInfo outDir)
        {
            if (outDir == null)
            {
                Logger.Error("Failed to extract. (outDir == null)");
                return;
            }

            if (!outDir.Exists)
            {
                outDir.Create();
                Logger.Info($"Created Dir: {outDir.FullName}");
            }

            string[] files = Directory.GetFiles(romDirectory.FullName, "*.arc", SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; i++)
            {
                // TODO utput folder of .arc folder name
                string filePath = files[i];
                string relativePath = filePath.Substring(romDirectory.FullName.Length);
                ArcArchive archive = new ArcArchive();
                archive.Open(filePath);
                foreach (ArcArchive.FileIndex fi in archive.GetFileIndices())
                {
                    ArcArchive.ArcFile af = archive.GetFile(fi);
                    if (af == null)
                    {
                        continue;
                    }

                    string outDirectory = Path.Combine(outDir.FullName, fi.Directory);
                    if (!Directory.Exists(outDirectory))
                    {
                        Directory.CreateDirectory(outDirectory);
                    }

                    string outPath = Path.Combine(outDirectory, fi.Name);
                    File.WriteAllBytes(outPath, af.Data);
                }

                Logger.Info($"Processing {i}/{files.Length} {filePath}");
            }
        }

        public void TexToDds(DirectoryInfo romDirectory, DirectoryInfo outDir)
        {
       Texture tex = ArcArchive.GetResource<Texture>(
           romDirectory,
           "game_common.arc",
           "scr/sky/DDBaseCube4_CM",
           "tex"
       );
       tex.SaveDds("E:/Games/ARCtool/DDBaseCube4_CM.tex.dds");
       tex.Save("E:/Games/ARCtool/DDBaseCube4_CM.tex");
       
         
      //  string p3 = "E:/Games/ARCtool/DefaultCube_CM.tex";
      //  Texture t3 = new Texture();
      //  t3.Open(p3);
      //  t3.SaveDds(p3 + ".dds");
      //  
      //  string p0 = "E:/Games/Dragon's Dogma Online/nativePC/system/texture/sysfont_AM_NOMIP.tex";
      //  Texture t0 = new Texture();
      //  t0.Open(p0);
      //  
      //     string p1 = "E:/Games/Dragon's Dogma Online/nativePC/system/texture/detail_sysfont_AM_NOMIP.tex";
      //     Texture t1 = new Texture();
      //     t1.Open(p1);
            
            
            int i = 1;
        }

        public void Shutdown()
        {
        }
    }
}
