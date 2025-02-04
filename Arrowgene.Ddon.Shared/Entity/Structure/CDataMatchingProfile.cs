using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMatchingProfile
    {
        public CDataMatchingProfile()
        {
            EntryJob = 0;
            EntryJobLevel = 0;
            CurrentJob = 0;
            CurrentJobLevel = 0;
            ObjectiveType1 = 0;
            ObjectiveType2 = 0;
            PlayStyle = 0;
            Comment = "";
            IsJoinParty = false;
        }

        public JobId EntryJob;
        public uint EntryJobLevel;
        public JobId CurrentJob;
        public uint CurrentJobLevel;
        public uint ObjectiveType1;
        public uint ObjectiveType2;
        public uint PlayStyle;
        public string Comment;
        public bool IsJoinParty;

        public class Serializer : EntitySerializer<CDataMatchingProfile>
        {
            public override void Write(IBuffer buffer, CDataMatchingProfile obj)
            {
                WriteByte(buffer, (byte)obj.EntryJob);
                WriteUInt32(buffer, obj.EntryJobLevel);
                WriteByte(buffer, (byte)obj.CurrentJob);
                WriteUInt32(buffer, obj.CurrentJobLevel);
                WriteUInt32(buffer, obj.ObjectiveType1);
                WriteUInt32(buffer, obj.ObjectiveType2);
                WriteUInt32(buffer, obj.PlayStyle);
                WriteMtString(buffer, obj.Comment);
                WriteBool(buffer, obj.IsJoinParty);
            }

            public override CDataMatchingProfile Read(IBuffer buffer)
            {
                CDataMatchingProfile obj = new CDataMatchingProfile();
                obj.EntryJob = (JobId)ReadByte(buffer);
                obj.EntryJobLevel = ReadUInt32(buffer);
                obj.CurrentJob = (JobId)ReadByte(buffer);
                obj.CurrentJobLevel = ReadUInt32(buffer);
                obj.ObjectiveType1 = ReadUInt32(buffer);
                obj.ObjectiveType2 = ReadUInt32(buffer);
                obj.PlayStyle = ReadUInt32(buffer);
                obj.Comment = ReadMtString(buffer);
                obj.IsJoinParty = ReadBool(buffer);
                return obj;
            }
        }
    }
}
