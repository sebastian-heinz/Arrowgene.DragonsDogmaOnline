using System;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataBazaarCharacterInfo
    {
        public CDataBazaarCharacterInfo() {
            ItemInfo = new CDataBazaarItemInfo();
            State = BazaarExhibitionState.OnSale;
            Expire = DateTimeOffset.UnixEpoch;
        }

        public CDataBazaarItemInfo ItemInfo { get; set; }
        public BazaarExhibitionState State { get; set; }
        public uint Proceeds { get; set; }
        public DateTimeOffset Expire { get; set; }
    
        public class Serializer : EntitySerializer<CDataBazaarCharacterInfo>
        {
            public override void Write(IBuffer buffer, CDataBazaarCharacterInfo obj)
            {
                WriteEntity<CDataBazaarItemInfo>(buffer, obj.ItemInfo);
                WriteByte(buffer, (byte) obj.State);
                WriteUInt32(buffer, obj.Proceeds);
                WriteInt64(buffer, obj.Expire.ToUnixTimeSeconds());
            }
        
            public override CDataBazaarCharacterInfo Read(IBuffer buffer)
            {
                CDataBazaarCharacterInfo obj = new CDataBazaarCharacterInfo();
                obj.ItemInfo = ReadEntity<CDataBazaarItemInfo>(buffer);
                obj.State = (BazaarExhibitionState) ReadByte(buffer);
                obj.Proceeds = ReadUInt32(buffer);
                obj.Expire = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));
                return obj;
            }
        }
    }
}