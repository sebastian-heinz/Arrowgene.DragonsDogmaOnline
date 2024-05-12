using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataBazaarItemHistoryInfo
    {
        public CDataBazaarItemHistoryInfo() {
            ItemBaseInfo = new CDataBazaarItemBaseInfo();
        }
    
        public CDataBazaarItemBaseInfo ItemBaseInfo { get; set; }
        public long BitDate { get; set; } // A timestamp
    
        public class Serializer : EntitySerializer<CDataBazaarItemHistoryInfo>
        {
            public override void Write(IBuffer buffer, CDataBazaarItemHistoryInfo obj)
            {
                WriteEntity<CDataBazaarItemBaseInfo>(buffer, obj.ItemBaseInfo);
                WriteInt64(buffer, obj.BitDate);
            }
        
            public override CDataBazaarItemHistoryInfo Read(IBuffer buffer)
            {
                CDataBazaarItemHistoryInfo obj = new CDataBazaarItemHistoryInfo();
                obj.ItemBaseInfo = ReadEntity<CDataBazaarItemBaseInfo>(buffer);
                obj.BitDate = ReadInt64(buffer);
                return obj;
            }
        }
    }
}