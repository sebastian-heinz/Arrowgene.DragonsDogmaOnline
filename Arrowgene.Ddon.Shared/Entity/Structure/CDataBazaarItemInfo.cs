using System;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataBazaarItemInfo
    {
        public CDataBazaarItemInfo() {
            ItemBaseInfo = new CDataBazaarItemBaseInfo();
            ExhibitionTime = DateTimeOffset.UnixEpoch;
        }

        public ulong BazaarId { get; set; }
        public ushort Sequence { get; set; }
        public CDataBazaarItemBaseInfo ItemBaseInfo { get; set; }
        public DateTimeOffset ExhibitionTime { get; set; }
    
        public class Serializer : EntitySerializer<CDataBazaarItemInfo>
        {
            public override void Write(IBuffer buffer, CDataBazaarItemInfo obj)
            {
                WriteUInt64(buffer, obj.BazaarId);
                WriteUInt16(buffer, obj.Sequence);
                WriteEntity<CDataBazaarItemBaseInfo>(buffer, obj.ItemBaseInfo);
                WriteInt64(buffer, obj.ExhibitionTime.ToUnixTimeSeconds());
            }
        
            public override CDataBazaarItemInfo Read(IBuffer buffer)
            {
                CDataBazaarItemInfo obj = new CDataBazaarItemInfo();
                obj.BazaarId = ReadUInt64(buffer);
                obj.Sequence = ReadUInt16(buffer);
                obj.ItemBaseInfo = ReadEntity<CDataBazaarItemBaseInfo>(buffer);
                obj.ExhibitionTime = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));
                return obj;
            }
        }
    }
}