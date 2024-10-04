using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataClanShopBuffInfo
    {
        public uint BuffId { get; set; }
        public byte BuffType { get; set; }

        public class Serializer : EntitySerializer<CDataClanShopBuffInfo>
        {
            public override void Write(IBuffer buffer, CDataClanShopBuffInfo obj)
            {
                WriteUInt32(buffer, obj.BuffId);
                WriteByte(buffer, obj.BuffType);
            }

            public override CDataClanShopBuffInfo Read(IBuffer buffer)
            {
                CDataClanShopBuffInfo obj = new CDataClanShopBuffInfo();
                obj.BuffId = ReadUInt32(buffer);
                obj.BuffType = ReadByte(buffer);
                return obj;
            }
        }
    }
}
