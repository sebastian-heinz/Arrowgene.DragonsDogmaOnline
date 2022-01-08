using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public struct CDataMatchingProfile
    {
        public byte EntryJob;
        public uint EntryJobLevel;
        public byte CurrentJob;
        public uint CurrentJobLevel;
        public uint ObjectiveType1;
        public uint ObjectiveType2;

        public uint PlayStyle;

        // length prefix
        public string Comment;
        public byte IsJoinParty;
    }

    public class CDataMatchingProfileSerializer : EntitySerializer<CDataMatchingProfile>
    {
        public override void Write(IBuffer buffer, CDataMatchingProfile obj)
        {
            WriteByte(buffer, obj.EntryJob);
            WriteUInt32(buffer, obj.EntryJobLevel);
            WriteByte(buffer, obj.CurrentJob);
            WriteUInt32(buffer, obj.CurrentJobLevel);
            WriteUInt32(buffer, obj.ObjectiveType1);
            WriteUInt32(buffer, obj.ObjectiveType2);
            WriteUInt32(buffer, obj.PlayStyle);
            WriteMtString(buffer, obj.Comment);
            WriteByte(buffer, obj.IsJoinParty);
        }

        public override CDataMatchingProfile Read(IBuffer buffer)
        {
            CDataMatchingProfile obj = new CDataMatchingProfile();
            obj.EntryJob = ReadByte(buffer);
            obj.EntryJobLevel = ReadUInt32(buffer);
            obj.CurrentJob = ReadByte(buffer);
            obj.CurrentJobLevel = ReadUInt32(buffer);
            obj.ObjectiveType1 = ReadUInt32(buffer);
            obj.ObjectiveType2 = ReadUInt32(buffer);
            obj.PlayStyle = ReadUInt32(buffer);
            obj.Comment = ReadMtString(buffer);
            obj.IsJoinParty = ReadByte(buffer);
            return obj;
        }
    }
}
