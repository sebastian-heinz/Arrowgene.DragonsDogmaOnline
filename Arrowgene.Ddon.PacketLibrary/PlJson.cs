using System;
using System.Collections.Generic;
using System.Text;

namespace Arrowgene.Ddon.PacketLibrary
{
    public static class PlJson
    {
        public static string Export(PlSession session)
        {
            List<PlPacket> packets = session.GetPackets();
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append($"\"LogStartTime\": \"{session.StartTime:O}\",");
            sb.Append($"\"ServerType\": \"{session.ServerType}\",");
            sb.Append($"\"ServerIP\": \"{session.ServerIP}\",");
            sb.Append($"\"Encrypted\": \"{session.IsEncrypted}\",");
            sb.Append($"\"Key\": \"{session.Key}\",");
            sb.Append($"\"Packets\": [");
            for (int i = 0; i < packets.Count; i++)
            {
                PlPacket packet = packets[i];
                sb.Append("{");
                sb.Append($"\"Timestamp\": \"{packet.Timestamp:O}\",");
                sb.Append($"\"Direction\": \"{packet.Direction}\",");
                sb.Append($"\"Data\": \"{Convert.ToBase64String(packet.Data)}\",");
                sb.Append("}");
                if (i < packets.Count - 1)
                {
                    sb.Append(",");
                }
            }

            sb.Append("]");
            sb.Append("}");
            return sb.ToString();
        }

        public static PlSession Import(string json)
        {
            PlSession session = new PlSession();
            return session;
        }
        
    }
}
