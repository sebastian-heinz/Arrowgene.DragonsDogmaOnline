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

        public string Description => "Usage: `packet \"E:\\dumps\\58_9.yaml` J2g4pE2_heyqIAengWy0N6D1SEklxz8I";


        public CommandResultType Run(CommandParameter parameter)
        {
            string yamlPath = parameter.Arguments[0];
            string camelliaKey = parameter.SwitchMap.GetValueOrDefault("--key", null) ?? parameter.Arguments[1];
            byte[] camelliaKeyBytes = Encoding.UTF8.GetBytes(camelliaKey);
            bool addUtf8EncodedByteDump = parameter.Switches.Contains("--utf8-dump");

            List<PcapPacket> decryptedPcapPackets = DecryptPackets(yamlPath, camelliaKeyBytes);
            string annotatedDump = GetAnnotatedPacketDump(decryptedPcapPackets, addUtf8EncodedByteDump);
            string outputPath = yamlPath + ".annotated.txt";
            File.WriteAllText(outputPath, annotatedDump);

            return CommandResultType.Exit;
        }

        public List<PcapPacket> DecryptPackets(string yamlPath, byte[] camelliaKeyBytes)
        {
            string yamlPcap = File.ReadAllText(yamlPath);
            List<PcapPacket> pcapPackets = ReadYamlPcap(yamlPcap);
            return DecryptPackets(pcapPackets, camelliaKeyBytes);
        }

        public List<PcapPacket> DecryptPackets(List<PcapPacket> pcapPackets, byte[] camelliaKeyBytes)
        {
            if (pcapPackets == null || pcapPackets.Count <= 0)
            {
                throw new PacketCommandException("No packets found to annotate.");
            }

            ServerType serverType = pcapPackets[0].ServerType;
            IPacketIdResolver packetIdResolver;
            if (serverType == ServerType.Login)
            {
                packetIdResolver = PacketIdResolver.LoginPacketIdResolver;
            }
            else if (serverType == ServerType.Game)
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
                }
                catch (Exception ex)
                {
                    throw new PacketCommandException("Could not parse more packets, closing prematurely", ex);
                }
            }

            return pcapPackets;
        }

        public string GetAnnotatedPacketDump(List<PcapPacket> decryptedPcapPackets, bool addUtf8EncodedByteDump)
        {
            StringBuilder annotated = new StringBuilder();
            {
                foreach (PcapPacket pcapPacket in decryptedPcapPackets)
                {
                    foreach (IPacket resolvedPacket in pcapPacket.ResolvedPackets)
                    {
                        annotated.Append(resolvedPacket.PrintHeader());
                        annotated.Append($" Pcap(No:{pcapPacket.Packet} Ts:{pcapPacket.TimeStamp})");
                        annotated.Append(Environment.NewLine);
                        annotated.Append(resolvedPacket.PrintData());
                        annotated.Append(string.Join(", ", resolvedPacket.Data.Select(dataByte => String.Format("0x{0:X}", dataByte))));
                        if (addUtf8EncodedByteDump)
                        {
                            annotated.Append(Environment.NewLine);
                            annotated.Append(string.Concat(Encoding.UTF8.GetString(resolvedPacket.Data, 0, resolvedPacket.Data.Length).Select(c =>
                                char.IsLetterOrDigit(c) || char.IsPunctuation(c) || char.IsSeparator(c) || char.IsSymbol(c) ? c : '·')));
                        }

                        annotated.Append(Environment.NewLine);
                        annotated.Append(Environment.NewLine);
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
            if (yamlFile.peers[0].port is (ushort)ServerType.Login or (ushort)ServerType.Game)
            {
                serverPeer = yamlFile.peers[0];
                clientPeer = yamlFile.peers[1];
            }
            else if (yamlFile.peers[1].port is (ushort)ServerType.Login or (ushort)ServerType.Game)
            {
                serverPeer = yamlFile.peers[1];
                clientPeer = yamlFile.peers[0];
            }
            else
            {
                throw new PacketCommandException("Could not identify peer roles");
            }

            ServerType serverType;
            if (serverPeer.port == (ushort)ServerType.Login)
            {
                serverType = ServerType.Login;
            }
            else if (serverPeer.port == (ushort)ServerType.Game)
            {
                serverType = ServerType.Game;
            }
            else
            {
                throw new PacketCommandException("Could not identify server type");
            }

            List<PcapPacket> pcapPackets = new List<PcapPacket>(yamlFile.packets.Count);
            foreach (YamlPacket yamlPacket in yamlFile.packets)
            {
                if (yamlPacket.timestamp.StartsWith("0"))
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

                pcapPacket.ServerType = serverType;
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
