using System;
using Arrowgene.Services.Buffers;

namespace Ddo.Server.Packet
{
    public class DdoPacket
    {
        public static string GetPacketIdName(ushort id)
        {
            if (Enum.IsDefined(typeof(PacketId), id))
            {
                PacketId authPacketId = (PacketId) id;
                return authPacketId.ToString();
            }

            return null;
        }

        private string _packetIdName;

        public DdoPacket(ushort id, IBuffer buffer)
        {
            Header = new PacketHeader(id);
            Data = buffer;
        }

        public DdoPacket(PacketHeader header, IBuffer buffer)
        {
            Header = header;
            Data = buffer;
        }

        public IBuffer Data { get; }
        public ushort Id => Header.Id;
        public PacketHeader Header { get; }

        public string PacketIdName
        {
            get
            {
                if (_packetIdName != null)
                {
                    return _packetIdName;
                }

                _packetIdName = GetPacketIdName(Id);
                if (_packetIdName == null)
                {
                    _packetIdName = $"ID_NOT_DEFINED_{Id}";
                }

                return _packetIdName;
            }
        }
    }
}
