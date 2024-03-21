using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataBazaarCharacterInfo
    {
        public CDataBazaarCharacterInfo() {
            ItemInfo = new CDataBazaarItemInfo();
        }

        public CDataBazaarItemInfo ItemInfo { get; set; }
        public byte State { get; set; }
        public uint Proceeds { get; set; }
        public long Expire { get; set; } // Probably a timestamp
    
        public class Serializer : EntitySerializer<CDataBazaarCharacterInfo>
        {
            public override void Write(IBuffer buffer, CDataBazaarCharacterInfo obj)
            {
                WriteEntity<CDataBazaarItemInfo>(buffer, obj.ItemInfo);
                WriteByte(buffer, obj.State);
                WriteUInt32(buffer, obj.Proceeds);
                WriteInt64(buffer, obj.Expire);
            }
        
            public override CDataBazaarCharacterInfo Read(IBuffer buffer)
            {
                CDataBazaarCharacterInfo obj = new CDataBazaarCharacterInfo();
                obj.ItemInfo = ReadEntity<CDataBazaarItemInfo>(buffer);
                obj.State = ReadByte(buffer);
                obj.Proceeds = ReadUInt32(buffer);
                obj.Expire = ReadInt64(buffer);
                return obj;
            }
        }
    }
}