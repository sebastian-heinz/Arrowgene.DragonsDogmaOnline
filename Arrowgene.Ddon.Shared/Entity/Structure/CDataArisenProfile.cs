using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataArisenProfile
    {
        public CDataArisenProfile()
        {
            BackgroundID = 0;
            Title = new CDataAchievementIdentifier();
            MotionID = 0;
            MotionFrameNo = 0;
        }

        public byte BackgroundID;
        public CDataAchievementIdentifier Title;
        public ushort MotionID;
        public uint MotionFrameNo;
    }

    public class CDataArisenProfileSerializer : EntitySerializer<CDataArisenProfile>
    {
        public override void Write(IBuffer buffer, CDataArisenProfile obj)
        {
            WriteByte(buffer, obj.BackgroundID);
            WriteEntity(buffer, obj.Title);
            WriteUInt16(buffer, obj.MotionID);
            WriteUInt32(buffer, obj.MotionFrameNo);
        }

        public override CDataArisenProfile Read(IBuffer buffer)
        {
            CDataArisenProfile obj = new CDataArisenProfile();
            obj.BackgroundID = ReadByte(buffer);
            obj.Title = ReadEntity<CDataAchievementIdentifier>(buffer);
            obj.MotionID = ReadUInt16(buffer);
            obj.MotionFrameNo = ReadUInt32(buffer);
            return obj;
        }
    }
}
