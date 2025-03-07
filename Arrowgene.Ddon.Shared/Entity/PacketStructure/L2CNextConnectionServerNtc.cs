using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class L2CNextConnectionServerNtc : IPacketStructure
    {
        public PacketId Id => PacketId.L2C_NEXT_CONNECT_SERVER_NTC;

        public uint Error { get; set; }
        public CDataGameServerListInfo ServerList { get; set; } = new();

        public byte Counter { get; set; }

        public class Serializer : PacketEntitySerializer<L2CNextConnectionServerNtc>
        {

            public override void Write(IBuffer buffer, L2CNextConnectionServerNtc obj)
            {
                WriteUInt32(buffer, obj.Error);
                WriteEntity(buffer, obj.ServerList);
                WriteByte(buffer, obj.Counter);
            }

            public override L2CNextConnectionServerNtc Read(IBuffer buffer)
            {
                L2CNextConnectionServerNtc obj = new L2CNextConnectionServerNtc();
                obj.Error = ReadUInt32(buffer);
                obj.ServerList = ReadEntity<CDataGameServerListInfo>(buffer);
                obj.Counter = ReadByte(buffer);
                return obj;
            }
        }
    }
}
