using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.GameServer.Network;
using Arrowgene.Ddon.PacketLibrary;
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
         //   File.WriteAllBytes("F://d8.bin", LoginDump.data_Dump_8);

  
            
            
            string pcapPath = parameter.Arguments[0];
            string key = parameter.Arguments[1];
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            PlFactory plFactory = new PlFactory();
            List<PlSession> sessions = plFactory.Create(pcapPath);
            
            List<PlPacket> encrypted = sessions[0].GetPackets();
            PacketFactory pf = new PacketFactory(new GameServerSetting());
            pf.SetCamelliaKey(keyBytes);
            
            // parse ddon packets
            List<Packet> packets = new List<Packet>();
            foreach (PlPacket plPacket in encrypted)
            {
                packets.AddRange(pf.Read(plPacket.Data));
            }

           string dump = PacketDump.DumpCSharpStruc(packets, "LoginDump");
           
           
            
            foreach (Packet packet in packets)
            {
                Console.WriteLine(
                    $"Id:{packet.Id.GroupId}.{packet.Id.HandlerId}.{packet.Id.HandlerSubId}{Environment.NewLine}" +
                    $"Name:{packet.Id.Name}{Environment.NewLine}" +
                    $"{Util.HexDump(packet.Data)}"
                );
            }

            return CommandResultType.Continue;
        }


        public void Shutdown()
        {
        }
    }
}
