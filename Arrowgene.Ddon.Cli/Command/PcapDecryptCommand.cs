using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arrowgene.Ddon.Cli.Misc;
using Arrowgene.Ddon.PacketLibrary;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared;

namespace Arrowgene.Ddon.Cli.Command
{
    public class PcapDecryptCommand : ICommand
    {
        public string Key => "pcap";

        public string Description =>
            $"Decrypt Pcaps Data";

        private static readonly byte[] CamelliaIv = new byte[]
        {
            0x24, 0x63, 0x62, 0x4D, 0x36, 0x57, 0x50, 0x29, 0x61, 0x58, 0x3D, 0x25, 0x4A, 0x5E, 0x7A, 0x41
        };

        public CommandResultType Run(CommandParameter parameter)
        {
            string pcapPath = parameter.Arguments[0];
            string key = parameter.Arguments[1];
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            PlFactory plFactory = new PlFactory();
            List<PlSession> sessions = plFactory.Create(pcapPath);
            List<PlPacket> encrypted = sessions[2].GetPackets();

            PrintPackets(encrypted, keyBytes);

            return CommandResultType.Continue;
        }

        private void PrintPackets(List<PlPacket> encrypted, byte[] keyBytes)
        {
            PacketFactory pfServer = new PacketFactory(new ServerSetting(), PacketIdResolver.GamePacketIdResolver);
            PacketFactory pfClient = new PacketFactory(new ServerSetting(), PacketIdResolver.GamePacketIdResolver);
            pfServer.SetCamelliaKey(keyBytes);
            pfClient.SetCamelliaKey(keyBytes);

            List<Packet> packets = new List<Packet>();
            try
            {
                foreach (PlPacket encPacket in encrypted)
                {
                    if (encPacket.Direction == "S2C")
                    {
                        List<Packet> decrypted = pfClient.Read(encPacket.Data);
                        packets.AddRange(decrypted);
                    }
                    else if (encPacket.Direction == "C2S")
                    {
                        List<Packet> decrypted = pfServer.Read(encPacket.Data);
                        packets.AddRange(decrypted);
                    }
                }
            }
            catch (Exception ex)
            {
            }

            StringBuilder sb = new StringBuilder();
            foreach (Packet packet in packets)
            {
                string text = $"Id:{packet.Id.GroupId}.{packet.Id.HandlerId}.{packet.Id.HandlerSubId}{Environment.NewLine}" +
                              $"Name:{packet.Id.Name}{Environment.NewLine}" +
                              $"{Util.HexDump(packet.Data)}";
                Console.WriteLine(text);
                sb.Append(text);
            }

           // string dump = PacketDump.DumpCSharpStruc(packets, "InGameDump");
           // File.WriteAllText("F://game2.txt", sb.ToString());
        }

        public void Shutdown()
        {
        }
    }
}
