using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataClanServerParam
    {
        public CDataClanServerParam()
        {
            MasterInfo = new CDataClanMemberInfo();
        }
        
        public uint ID { get; set; }
        public ushort Lv { get; set; }
        public ushort MemberNum { get; set; }
        public CDataClanMemberInfo MasterInfo { get; set; }
        public bool IsSystemRestriction { get; set; }
        public bool IsClanBaseRelease { get; set; }
        public bool CanClanBaseRelease { get; set; }
        public uint TotalClanPoint { get; set; }
        public uint MoneyClanPoint { get; set; }
        public uint NextClanPoint { get; set; }
    
        public class Serializer : EntitySerializer<CDataClanServerParam>
        {
            public override void Write(IBuffer buffer, CDataClanServerParam obj)
            {
                WriteUInt32(buffer, obj.ID);
                WriteUInt16(buffer, obj.Lv);
                WriteUInt16(buffer, obj.MemberNum);
                WriteEntity<CDataClanMemberInfo>(buffer, obj.MasterInfo);
                WriteBool(buffer, obj.IsSystemRestriction);
                WriteBool(buffer, obj.IsClanBaseRelease);
                WriteBool(buffer, obj.CanClanBaseRelease);
                WriteUInt32(buffer, obj.TotalClanPoint);
                WriteUInt32(buffer, obj.MoneyClanPoint);
                WriteUInt32(buffer, obj.NextClanPoint);
            }
        
            public override CDataClanServerParam Read(IBuffer buffer)
            {
                CDataClanServerParam obj = new CDataClanServerParam();
                obj.ID = ReadUInt32(buffer);
                obj.Lv = ReadUInt16(buffer);
                obj.MemberNum = ReadUInt16(buffer);
                obj.MasterInfo = ReadEntity<CDataClanMemberInfo>(buffer);
                obj.IsSystemRestriction = ReadBool(buffer);
                obj.IsClanBaseRelease = ReadBool(buffer);
                obj.CanClanBaseRelease = ReadBool(buffer);
                obj.TotalClanPoint = ReadUInt32(buffer);
                obj.MoneyClanPoint = ReadUInt32(buffer);
                obj.NextClanPoint = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
