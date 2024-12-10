using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataClanParam
    {
        public CDataClanParam() {
            ClanUserParam = new CDataClanUserParam();
            ClanServerParam = new CDataClanServerParam();
        }
    
        public CDataClanUserParam ClanUserParam { get; set; }
        public CDataClanServerParam ClanServerParam { get; set; }
    
        public class Serializer : EntitySerializer<CDataClanParam>
        {
            public override void Write(IBuffer buffer, CDataClanParam obj)
            {
                WriteEntity<CDataClanUserParam>(buffer, obj.ClanUserParam);
                WriteEntity<CDataClanServerParam>(buffer, obj.ClanServerParam);
            }
        
            public override CDataClanParam Read(IBuffer buffer)
            {
                CDataClanParam obj = new CDataClanParam();
                obj.ClanUserParam = ReadEntity<CDataClanUserParam>(buffer);
                obj.ClanServerParam = ReadEntity<CDataClanServerParam>(buffer);
                return obj;
            }
        }
    }
}
