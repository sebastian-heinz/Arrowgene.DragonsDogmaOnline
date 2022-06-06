using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanGetFurnitureReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_GET_FURNITURE_REQ;

        public byte Data0 { get; set; }
        public byte Data1 { get; set; }
        public byte Data2 { get; set; }
        public byte Data3 { get; set; }
        public byte Data4 { get; set; }

        public C2SClanGetFurnitureReq()
        {
            Data0 = 0;
            Data1 = 0;
            Data2 = 0;
            Data3 = 0;
            Data4 = 0;
        }

        public class Serializer : PacketEntitySerializer<C2SClanGetFurnitureReq>
        {
            public override void Write(IBuffer buffer, C2SClanGetFurnitureReq obj)
            {
                WriteByte(buffer, obj.Data0);
                WriteByte(buffer, obj.Data1);
                WriteByte(buffer, obj.Data2);
                WriteByte(buffer, obj.Data3);
                WriteByte(buffer, obj.Data4);
            }

            public override C2SClanGetFurnitureReq Read(IBuffer buffer)
            {
                C2SClanGetFurnitureReq obj = new C2SClanGetFurnitureReq();
                obj.Data0 = ReadByte(buffer);
                obj.Data1 = ReadByte(buffer);
                obj.Data2 = ReadByte(buffer);
                obj.Data3 = ReadByte(buffer);
                obj.Data4 = ReadByte(buffer);
                return obj;
            }
        }
    }
}
