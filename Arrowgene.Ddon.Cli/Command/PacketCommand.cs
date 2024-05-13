using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Arrowgene.Ddon.Cli.Command.Packet;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using YamlDotNet.Serialization;

namespace Arrowgene.Ddon.Cli.Command
{
    public class PacketCommand : ICommand
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(PacketCommand));
        public string Key => "packet";
        private static string DecryptionKeySwitch => "--key";

        public string Description =>
            $"Usage: `{Key} \"E:\\dumps\\58_9.yaml` [{DecryptionKeySwitch}=]J2g4pE2_heyqIAengWy0N6D1SEklxz8I {PacketCommandOptions.GetUsage()}";

        public CommandResultType Run(CommandParameter parameter)
        {
            string yamlPath = parameter.Arguments[0];
            string camelliaKey = parameter.SwitchMap.GetValueOrDefault(DecryptionKeySwitch, null) ?? parameter.Arguments[1];
            byte[] camelliaBytes = Encoding.UTF8.GetBytes(camelliaKey);
            PacketCommandOptions packetCommandOptions = new PacketCommandOptions(parameter);

            List<PcapPacket> decryptedPcapPackets = DecryptPackets(yamlPath, camelliaBytes, packetCommandOptions.ExportDecryptedPackets);
            string annotatedDump = GetAnnotatedPacketDump(decryptedPcapPackets, packetCommandOptions);
            string outputPath = yamlPath + ".annotated.txt";
            File.WriteAllText(outputPath, annotatedDump);

            return CommandResultType.Exit;
        }

        public List<PcapPacket> DecryptPackets(string yamlPath, byte[] camelliaKeyBytes, bool exportDecryptedPackets = false)
        {
            string yamlPcap = File.ReadAllText(yamlPath);
            List<PcapPacket> pcapPackets = ReadYamlPcap(yamlPcap);
            return DecryptPackets(pcapPackets, camelliaKeyBytes, exportDecryptedPackets ? Path.GetFileNameWithoutExtension(yamlPath) : "");
        }

        public List<PcapPacket> DecryptPackets(List<PcapPacket> pcapPackets, byte[] camelliaKeyBytes, string outputFolder = "")
        {
            if (pcapPackets == null || pcapPackets.Count <= 0)
            {
                throw new PacketCommandException("No packets found to annotate.");
            }

            PacketServerType packetServerType = pcapPackets[0].PacketServerType;
            IPacketIdResolver packetIdResolver;
            if (packetServerType == PacketServerType.Login)
            {
                packetIdResolver = PacketIdResolver.LoginPacketIdResolver;
            }
            else if (packetServerType == PacketServerType.Game)
            {
                packetIdResolver = PacketIdResolver.GamePacketIdResolver;
            }
            else
            {
                throw new PacketCommandException("Could not determine server type.");
            }

            PacketFactory serverFactory = new PacketFactory(new ServerSetting(), packetIdResolver);
            PacketFactory clientFactory = new PacketFactory(new ServerSetting(), packetIdResolver);
            serverFactory.SetCamelliaKey(camelliaKeyBytes);
            clientFactory.SetCamelliaKey(camelliaKeyBytes);

            foreach (PcapPacket pcapPacket in pcapPackets)
            {
                try
                {
                    if (pcapPacket.Source == PacketSource.Client)
                    {
                        pcapPacket.ResolvedPackets = clientFactory.Read(pcapPacket.Data);
                    }
                    else if (pcapPacket.Source == PacketSource.Server)
                    {
                        pcapPacket.ResolvedPackets = serverFactory.Read(pcapPacket.Data);
                    }

                    if (!string.IsNullOrEmpty(outputFolder))
                    {
                        DirectoryInfo directoryInfo = Directory.CreateDirectory(outputFolder);
                        foreach (IPacket resolvedPacket in pcapPacket.ResolvedPackets)
                        {
                            string fileName = Path.Combine(directoryInfo.FullName,
                                $"{pcapPacket.Packet}_{resolvedPacket.Id.Name}_{pcapPacket.TimeStamp.Replace(':', '_')}.{resolvedPacket.Id.GroupId}_{resolvedPacket.Id.HandlerId}_{resolvedPacket.Id.HandlerSubId}");
                            File.WriteAllBytes(fileName, resolvedPacket.GetHeaderBytes().Concat(resolvedPacket.Data).ToArray());
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new PacketCommandException("Could not parse more packets, closing prematurely", ex);
                }
            }

            return pcapPackets;
        }

        public string GetAnnotatedPacketDump(List<PcapPacket> decryptedPcapPackets, PacketCommandOptions packetCommandOptions)
        {
            HashSet<string> packetIncludeFilterSet = new HashSet<string>(packetCommandOptions.PacketIncludeFilter.ToLowerInvariant().Split(','));
            StringBuilder annotated = new StringBuilder();
            {
                foreach (PcapPacket pcapPacket in decryptedPcapPackets)
                {
                    foreach (IPacket resolvedPacket in pcapPacket.ResolvedPackets)
                    {
                        if (packetCommandOptions.PacketIncludeFilter != "" && !packetIncludeFilterSet.Contains(resolvedPacket.Id.Name.ToLowerInvariant()) &&
                            !packetIncludeFilterSet.Contains(resolvedPacket.Id.ToString()))
                        {
                            continue;
                        }

                        annotated.AppendLine($"{resolvedPacket.PrintHeader()} Pcap(No:{pcapPacket.Packet} Ts:{pcapPacket.TimeStamp})");
                        annotated.Append(resolvedPacket.PrintData());
                        if (packetCommandOptions.AddStructureDump && resolvedPacket is IStructurePacket)
                        {
                            try
                            {
                                if (packetCommandOptions.StructureDumpFormat.Equals("yaml", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    annotated.Append($"StructDump: {StructurePacket.YamlSerializer.Serialize(resolvedPacket)}");
                                }
                                else
                                {
                                    annotated.Append($"StructDump: {StructurePacket.JsonSerializer.Serialize(resolvedPacket)}");
                                }
                            }
                            catch (Exception e)
                            {
                                Logger.Exception(new PacketCommandException(
                                    $"Unable to parse structure for resolved packet {resolvedPacket.Id} within pcap {pcapPacket.Packet}. Skipping.", e));
                            }
                        }

                        if (packetCommandOptions.AddByteDump)
                        {
                            string byteDump = "";
                            if (packetCommandOptions.AddByteDumpHeader)
                            {
                                byteDump += string.Join(packetCommandOptions.ByteDumpSeparator,
                                    resolvedPacket.GetHeaderBytes().Select(dataByte => $"{packetCommandOptions.ByteDumpPrefix}{dataByte:X2}"));
                                byteDump += packetCommandOptions.ByteDumpSeparator;
                            }

                            byteDump += string.Join(packetCommandOptions.ByteDumpSeparator,
                                resolvedPacket.Data.Select(dataByte => $"{packetCommandOptions.ByteDumpPrefix}{dataByte:X2}"));
                            annotated.AppendLine($"ByteDump: {byteDump}");
                        }

                        if (packetCommandOptions.AddUtf8StringDump)
                        {
                            string utf8dump = string.Concat(Encoding.UTF8.GetString(resolvedPacket.Data, 0, resolvedPacket.Data.Length).Select(c =>
                                char.IsLetterOrDigit(c) || char.IsPunctuation(c) || char.IsSeparator(c) || char.IsSymbol(c) ? c : '·'));
                            annotated.AppendLine($"StringDump: {utf8dump}");
                        }

                        annotated.AppendLine();
                        annotated.AppendLine();
                    }
                }
            }
            return annotated.ToString();
        }

        public List<PcapPacket> ReadYamlPcap(string yaml)
        {
            IDeserializer yamlDeserializer = new DeserializerBuilder()
                .WithTagMapping("tag:yaml.org,2002:binary", typeof(string))
                .IgnoreUnmatchedProperties()
                .Build();
            YamlFile yamlFile = yamlDeserializer.Deserialize<YamlFile>(yaml);
            if (yamlFile.peers.Count != 2)
            {
                throw new PacketCommandException("Expected two peers");
            }

            YamlPeer serverPeer;
            YamlPeer clientPeer;
            if (yamlFile.peers[0].port is (ushort)PacketServerType.Login or (ushort)PacketServerType.Game)
            {
                serverPeer = yamlFile.peers[0];
                clientPeer = yamlFile.peers[1];
            }
            else if (yamlFile.peers[1].port is (ushort)PacketServerType.Login or (ushort)PacketServerType.Game)
            {
                serverPeer = yamlFile.peers[1];
                clientPeer = yamlFile.peers[0];
            }
            else
            {
                throw new PacketCommandException("Could not identify peer roles");
            }

            PacketServerType packetServerType;
            if (serverPeer.port == (ushort)PacketServerType.Login)
            {
                packetServerType = PacketServerType.Login;
            }
            else if (serverPeer.port == (ushort)PacketServerType.Game)
            {
                packetServerType = PacketServerType.Game;
            }
            else
            {
                throw new PacketCommandException("Could not identify server type");
            }

            List<PcapPacket> pcapPackets = new List<PcapPacket>(yamlFile.packets.Count);
            foreach (YamlPacket yamlPacket in yamlFile.packets)
            {
                if (yamlPacket.timestamp.StartsWith('0'))
                {
                    Logger.Error($"Skipping broken packet {yamlPacket.packet} with invalid timestamp!");
                    continue;
                }

                PcapPacket pcapPacket = new PcapPacket();
                if (yamlPacket.peer == serverPeer.peer)
                {
                    pcapPacket.Source = PacketSource.Server;
                }
                else if (yamlPacket.peer == clientPeer.peer)
                {
                    pcapPacket.Source = PacketSource.Client;
                }
                else
                {
                    pcapPacket.Source = PacketSource.Unknown;
                    Logger.Error("Failed to identify packet peer owner");
                }

                pcapPacket.PacketServerType = packetServerType;
                pcapPacket.Index = yamlPacket.index;
                pcapPacket.Data = Convert.FromBase64String(yamlPacket.data);
                pcapPacket.TimeStamp = ToReadableTimestamp(yamlPacket.timestamp);
                pcapPacket.Packet = yamlPacket.packet;
                pcapPackets.Add(pcapPacket);
            }

            return pcapPackets;
        }

        private static string ToReadableTimestamp(string fractionalTimestamp)
        {
            // epoch -> UTC -> Mountain Time
            double epochSeconds = double.Parse(fractionalTimestamp.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
            DateTimeOffset readableTimestamp = DateTimeOffset.UnixEpoch.AddSeconds(epochSeconds);
            // If we ever get other pcap files, the time zone should instead be provided via the YAML files 
            readableTimestamp = TimeZoneInfo.ConvertTime(readableTimestamp, TimeZoneInfo.FindSystemTimeZoneById("America/Dawson"));
            return readableTimestamp.ToString("o");
        }

        public void Shutdown()
        {
        }
    }
}
