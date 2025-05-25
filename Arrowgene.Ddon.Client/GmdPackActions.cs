using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Arrowgene.Ddon.Client.Resource;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Csv;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Client
{

    public class GmdActions {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(GmdActions));

        // pack gmd.csv into all .arc files of rom
        public static void Pack(List<GmdCsv.Entry> csvEntries, string romDirArg, string romLangArg, IProgress<PackProgressReport> progress = null)
        {
            if (!Enum.TryParse<GuiMessage.Language>(romLangArg, out GuiMessage.Language romLanguage))
            {
                throw new Exception($"Provided romLang:{romLangArg} is invalid");
            }

            DirectoryInfo romDir = new DirectoryInfo(romDirArg);
            if (!romDir.Exists)
            {
                throw new Exception($"Provided romDir:{romDirArg} does not exist");
            }

            int totalGmdEntries = 0;
            Dictionary<string, Dictionary<string, List<GmdCsv.Entry>>> csvArcLookup =
                new Dictionary<string, Dictionary<string, List<GmdCsv.Entry>>>();
            foreach (GmdCsv.Entry csvEntry in csvEntries)
            {
                string arcPath = csvEntry.ArcPath;
                Dictionary<string, List<GmdCsv.Entry>> gmdLookup;
                if (csvArcLookup.ContainsKey(arcPath))
                {
                    gmdLookup = csvArcLookup[arcPath];
                }
                else
                {
                    gmdLookup = new Dictionary<string, List<GmdCsv.Entry>>();
                    csvArcLookup.Add(arcPath, gmdLookup);
                }

                string gmdPath = csvEntry.GmdPath;
                List<GmdCsv.Entry> gmdEntries;
                if (gmdLookup.ContainsKey(gmdPath))
                {
                    gmdEntries = gmdLookup[gmdPath];
                }
                else
                {
                    gmdEntries = new List<GmdCsv.Entry>();
                    gmdLookup.Add(gmdPath, gmdEntries);
                }

                gmdEntries.Add(csvEntry);
                totalGmdEntries++;
            }

            int processedGmdEntries = 0;
            foreach (string arcPath in csvArcLookup.Keys)
            {
                string fullPath = Path.Combine(romDir.FullName, Util.UnrootPath(arcPath));
                Dictionary<string, List<GmdCsv.Entry>> gmdCsvLookup = csvArcLookup[arcPath];
                ArcArchive archive = new ArcArchive();
                archive.Open(fullPath);
                List<ArcArchive.ArcFile> gmdFiles = archive.GetFiles(ArcArchive.Search().ByExtension("gmd"));
                foreach (ArcArchive.ArcFile gmdFile in gmdFiles)
                {
                    string gmdPath = gmdFile.Index.Path;
                    if (!gmdCsvLookup.ContainsKey(gmdPath))
                    {
                        Logger.Info($"No Updates for GMD (ArcPath:{arcPath}, GmdPath:{gmdPath})");
                        continue;
                    }

                    List<GmdCsv.Entry> gmdCsvEntries = gmdCsvLookup[gmdPath];
                    GuiMessage gmd = new GuiMessage();
                    gmd.Open(gmdFile.Data);
                    foreach (GuiMessage.Entry entry in gmd.Entries)
                    {
                        Logger.Info($"Writing {processedGmdEntries}/{totalGmdEntries} {fullPath}");
                        if (progress != null)
                        {
                            progress.Report(new PackProgressReport()
                            {
                                Current = processedGmdEntries++,
                                Total = totalGmdEntries,
                                Path = fullPath
                            });
                        }

                        GmdCsv.Entry matchCsvEntry = null;
                        List<GmdCsv.Entry> keyMatches = new();
                        List<GmdCsv.Entry> indexMatches = new();
                        foreach (GmdCsv.Entry csvEntry in gmdCsvEntries)
                        {
                            if (!string.IsNullOrEmpty(entry.Key) && csvEntry.Key == entry.Key)
                            {
                                keyMatches.Add(csvEntry);
                                continue;
                            }

                            if (entry.ReadIndex == csvEntry.ReadIndex)
                            {
                                indexMatches.Add(csvEntry);
                                continue;
                            }
                        }

                        matchCsvEntry = keyMatches.Where(x => x.ReadIndex == entry.ReadIndex).FirstOrDefault()
                            ?? keyMatches.FirstOrDefault()
                            ?? indexMatches.FirstOrDefault();

                        if (matchCsvEntry == null)
                        {
                            Logger.Info(
                                $"No Update for GMD Entry, skipping (ArcPath:{arcPath}, GmdPath:{gmdPath}, Key:{entry.Key}. Msg:{entry.Msg})");
                            continue;
                        }

                        string newMsg;
                        if (romLanguage == GuiMessage.Language.Japanese)
                        {
                            newMsg = matchCsvEntry.MsgJp;
                        }
                        else if (romLanguage == GuiMessage.Language.English)
                        {
                            newMsg = matchCsvEntry.MsgEn;
                        }
                        else
                        {
                            Logger.Error(
                                $"Language {romLanguage} not supported, skipping (ArcPath:{arcPath}, GmdPath:{gmdPath}, Key:{entry.Key}. Msg:{entry.Msg})");
                            continue;
                        }

                        if (string.IsNullOrEmpty(newMsg))
                        {
                            Logger.Info(
                                $"csv message is empty, skipping (ArcPath:{arcPath}, GmdPath:{gmdPath}, Key:{entry.Key}. Msg:{entry.Msg})");
                            continue;
                        }

                        entry.Msg = newMsg;
                    }

                    gmdFile.Data = gmd.Save();
                }

                byte[] savedArc = archive.Save();
                File.WriteAllBytes(fullPath, savedArc);
            }
        }

        public class PackProgressReport
        {
            public int Current { get; set; } = 0;
            public int Total { get; set; } = 0;
            public string Path { get; set; } = string.Empty;
        }
    }
}