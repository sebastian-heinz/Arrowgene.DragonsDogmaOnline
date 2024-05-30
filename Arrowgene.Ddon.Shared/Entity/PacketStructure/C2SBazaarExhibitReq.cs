using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBazaarExhibitReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BAZAAR_EXHIBIT_REQ;

        public C2SBazaarExhibitReq()
        {
            ItemUID = string.Empty;
        }

        public StorageType StorageType { get; set; }
        public string ItemUID { get; set; }
        public uint Num { get; set; }
        public uint Price { get; set; }
        public byte Flag { get; set; }

        public class Serializer : PacketEntitySerializer<C2SBazaarExhibitReq>
        {
            public override void Write(IBuffer buffer, C2SBazaarExhibitReq obj)
            {
                WriteByte(buffer, (byte) obj.StorageType);
                WriteMtString(buffer, obj.ItemUID);
                WriteUInt32(buffer, obj.Num);
                WriteUInt32(buffer, obj.Price);
                WriteByte(buffer, obj.Flag);
            }

            public override C2SBazaarExhibitReq Read(IBuffer buffer)
            {
                C2SBazaarExhibitReq obj = new C2SBazaarExhibitReq();
                obj.StorageType = (StorageType) ReadByte(buffer);
                obj.ItemUID = ReadMtString(buffer);
                obj.Num = ReadUInt32(buffer);
                obj.Price = ReadUInt32(buffer);
                obj.Flag = ReadByte(buffer);
                return obj;
            }
        }

    }
}