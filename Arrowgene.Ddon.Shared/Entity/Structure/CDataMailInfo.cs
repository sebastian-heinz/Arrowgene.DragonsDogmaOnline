using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMailInfo
    {
        public CDataMailInfo()
        {
            BaseInfo = new CDataCommunityCharacterBaseInfo();
        }

        public ulong Id { get; set; }
        public MailState State { get; set; }
        public byte ItemState { get; set; }
        public CDataCommunityCharacterBaseInfo BaseInfo { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string MailText { get; set; } = string.Empty;
        public ulong SenderDate { get; set; }

        public class Serializer : EntitySerializer<CDataMailInfo>
        {
            public override void Write(IBuffer buffer, CDataMailInfo obj)
            {
                WriteUInt64(buffer, obj.Id);
                WriteByte(buffer, (byte) obj.State);
                WriteByte(buffer, obj.ItemState);
                WriteEntity(buffer, obj.BaseInfo);
                WriteMtString(buffer, obj.SenderName);
                WriteMtString(buffer, obj.MailText);
                WriteUInt64(buffer, obj.SenderDate);
            }

            public override CDataMailInfo Read(IBuffer buffer)
            {
                CDataMailInfo obj = new CDataMailInfo();
                obj.Id = ReadUInt64(buffer);
                obj.State = (MailState) ReadByte(buffer);
                obj.ItemState = ReadByte(buffer);
                obj.BaseInfo = ReadEntity<CDataCommunityCharacterBaseInfo>(buffer);
                obj.SenderName = ReadMtString(buffer);
                obj.MailText = ReadMtString(buffer);
                obj.SenderDate = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}
