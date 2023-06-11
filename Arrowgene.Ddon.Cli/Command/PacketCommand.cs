using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
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
            string camelliaKey = parameter.Arguments[1];
            byte[] camelliaKeyBytes = Encoding.UTF8.GetBytes(camelliaKey);


            string yamlPcap = File.ReadAllText(yamlPath);
            List<PcapPacket> pcapPackets = ReadYamlPcap(yamlPcap);

            if (pcapPackets.Count <= 0)
            {
                Logger.Error("No packets found to annotate");
                return CommandResultType.Continue;
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
                Logger.Error("could not determinate server type");
                return CommandResultType.Continue;
            }

            PacketFactory serverFactory = new PacketFactory(new ServerSetting(), packetIdResolver);
            PacketFactory clientFactory = new PacketFactory(new ServerSetting(), packetIdResolver);
            serverFactory.SetCamelliaKey(camelliaKeyBytes);
            clientFactory.SetCamelliaKey(camelliaKeyBytes);

            StringBuilder annotated = new StringBuilder();
            foreach (PcapPacket pcapPacket in pcapPackets)
            {
                List<IPacket> readPackets = null;
                try
                {
                    if (pcapPacket.Source == PacketSource.Client)
                    {
                        readPackets = clientFactory.Read(pcapPacket.Data);
                    }
                    else if (pcapPacket.Source == PacketSource.Server)
                    {
                        readPackets = serverFactory.Read(pcapPacket.Data);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("Could not parse more packets, closing prematurely");
                    break;
                }

                if (readPackets != null)
                {
                    foreach (IPacket readPacket in readPackets)
                    {
                        annotated.Append(readPacket.PrintHeader());
                        annotated.Append($" Pcap(No:{pcapPacket.Packet} Ts:{pcapPacket.TimeStamp})");
                        annotated.Append(Environment.NewLine);
                        annotated.Append(readPacket.PrintData());
                        annotated.Append(string.Join(", ", readPacket.Data.Select(dataByte => String.Format("0x{0:X}", dataByte))));
                        annotated.Append(Environment.NewLine);
                        annotated.Append(Environment.NewLine);
                    }
                }
            }


            string outputPath = yamlPath + ".annotated.txt";
            File.WriteAllText(outputPath, annotated.ToString());

            return CommandResultType.Continue;
        }


        private List<PcapPacket> ReadYamlPcap(string yaml)
        {
            List<PcapPacket> pcapPackets = new List<PcapPacket>();
            IDeserializer yamlDeserializer = new DeserializerBuilder()
                .WithTagMapping("tag:yaml.org,2002:binary", typeof(string))
                .Build();
            YamlFile yamlFile = yamlDeserializer.Deserialize<YamlFile>(yaml);
            if (yamlFile.peers.Count != 2)
            {
                Logger.Error("Expected two peers");
                return pcapPackets;
            }

            YamlPeer serverPeer = null;
            YamlPeer clientPeer = null;
            if (yamlFile.peers[0].port == 52100 || yamlFile.peers[0].port == 52000)
            {
                serverPeer = yamlFile.peers[0];
                clientPeer = yamlFile.peers[1];
            }
            else if (yamlFile.peers[1].port == 52100 || yamlFile.peers[1].port == 52000)
            {
                serverPeer = yamlFile.peers[1];
                clientPeer = yamlFile.peers[0];
            }
            else
            {
                Logger.Error("Could not identify peer roles");
                return pcapPackets;
            }

            ServerType serverType;
            if (serverPeer.port == 52100)
            {
                serverType = ServerType.Login;
            }
            else if (serverPeer.port == 52000)
            {
                serverType = ServerType.Game;
            }
            else
            {
                Logger.Error("Could not identify server type");
                return pcapPackets;
            }

            foreach (YamlPacket yamlPacket in yamlFile.packets)
            {
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
                pcapPacket.TimeStamp = yamlPacket.timestamp;
                pcapPacket.Packet = yamlPacket.packet;
                pcapPackets.Add(pcapPacket);
            }


            return pcapPackets;
        }

        public void Shutdown()
        {
        }

        private class YamlPeer
        {
            public uint peer;
            public string host;
            public ushort port;
        }

        private class YamlPacket
        {
            public uint packet;
            public uint peer;
            public uint index;
            public double timestamp;
            public string data;
        }

        private class YamlFile
        {
            public List<YamlPeer> peers;
            public List<YamlPacket> packets;
        }

        private class PcapPacket
        {
            public ServerType ServerType { get; set; }
            public PacketSource Source { get; set; }
            public double TimeStamp { get; set; }
            public uint Index { get; set; }
            public uint Packet { get; set; }
            public byte[] Data { get; set; }
        }
    }
}
