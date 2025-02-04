using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPartyMemberMaxNum
    {
        public uint ContentType { get; set; }
        public uint MaxNum { get; set; }

        public class Serializer : EntitySerializer<CDataPartyMemberMaxNum>
        {
            public override void Write(IBuffer buffer, CDataPartyMemberMaxNum obj)
            {
                WriteUInt32(buffer, obj.ContentType);
                WriteUInt32(buffer, obj.MaxNum);
            }

            public override CDataPartyMemberMaxNum Read(IBuffer buffer)
            {
                CDataPartyMemberMaxNum obj = new CDataPartyMemberMaxNum();
                obj.ContentType = ReadUInt32(buffer);
                obj.MaxNum = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
