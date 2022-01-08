using System;
using System.Collections.Generic;
using System.Text;
using Arrowgene.Ddon.Server.Network;

namespace Arrowgene.Ddon.LoginServer.Dump
{
    public static class PacketDump
    {
        public static string DumpCSharpStruc(List<Packet> packets, string name)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"using Arrowgene.Ddon.Server.Network;");
            sb.Append(Environment.NewLine);
            sb.Append($"namespace Arrowgene.Ddon.LoginServer.Dump");
            sb.Append(Environment.NewLine);
            sb.Append("{");
            sb.Append(Environment.NewLine);
            sb.Append($"public static class {name}");
            sb.Append(Environment.NewLine);
            sb.Append("{");
            sb.Append(Environment.NewLine);
            for (int i = 0; i < packets.Count; i++)
            {
                sb.Append(DumpCSharpStruc(packets[i], $"Dump_{i}"));
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
            }

            sb.Append(Environment.NewLine);
            sb.Append("}");
            sb.Append(Environment.NewLine);
            sb.Append("}");
            return sb.ToString();
        }

        public static string DumpCSharpStruc(Packet packet, string name)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(
                $"public static byte[] data_{name} = new byte[] /* {packet.Id.GroupId}.{packet.Id.HandlerId}.{packet.Id.HandlerSubId} */");
            sb.Append(Environment.NewLine);
            sb.Append("{");
            sb.Append(Environment.NewLine);
            for (int i = 0; i < packet.Data.Length; i++)
            {
                byte b = packet.Data[i];
                sb.Append($"0x{b:X}");
                if (i < packet.Data.Length - 1)
                {
                    sb.Append(",");
                }

                if ((i + 1) % 16 == 0)
                {
                    sb.Append(Environment.NewLine);
                }
            }
            sb.Append("};");
            sb.Append(Environment.NewLine);
            sb.Append(
                $"public static Packet {name} = new Packet(new PacketId({packet.Id.GroupId}, {packet.Id.HandlerId}, {packet.Id.HandlerSubId}, \"{name}\"), data_{name});");
            sb.Append(Environment.NewLine);
            
            return sb.ToString();
        }
    }
}
