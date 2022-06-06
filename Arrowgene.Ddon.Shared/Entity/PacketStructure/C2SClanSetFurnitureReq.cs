using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanSetFurnitureReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_SET_FURNITURE_REQ;

        public uint Length { get; set; }
        public uint FurnitureId { get; set; }
        public byte LocationId { get; set; }

        public C2SClanSetFurnitureReq()
        {
            Length = 0;
            FurnitureId = 0;
            LocationId = 0;
        }

        public class Serializer : PacketEntitySerializer<C2SClanSetFurnitureReq>
        {
            public override void Write(IBuffer buffer, C2SClanSetFurnitureReq obj)
            {
                WriteUInt32(buffer, obj.Length);
                WriteUInt32(buffer, obj.FurnitureId);
                WriteByte(buffer, obj.LocationId);
            }

            public override C2SClanSetFurnitureReq Read(IBuffer buffer)
            {
                C2SClanSetFurnitureReq obj = new C2SClanSetFurnitureReq();
                obj.Length = ReadUInt32(buffer);
                obj.FurnitureId = ReadUInt32(buffer);
                obj.LocationId = ReadByte(buffer);
                return obj;
            }
        }
    }
}
