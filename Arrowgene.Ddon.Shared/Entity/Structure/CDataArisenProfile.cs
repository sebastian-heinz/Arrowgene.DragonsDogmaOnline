using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataArisenProfile
    {
        public CDataArisenProfile()
        {
            BackgroundId = 0;
            Title = new CDataAchievementIdentifier();
            MotionId = 0;
            MotionFrameNo = 0;
        }

        public byte BackgroundId;
        public CDataAchievementIdentifier Title;
        public ushort MotionId;
        public uint MotionFrameNo;

        public class Serializer : EntitySerializer<CDataArisenProfile>
        {
            public override void Write(IBuffer buffer, CDataArisenProfile obj)
            {
                WriteByte(buffer, obj.BackgroundId);
                WriteEntity(buffer, obj.Title);
                WriteUInt16(buffer, obj.MotionId);
                WriteUInt32(buffer, obj.MotionFrameNo);
            }

            public override CDataArisenProfile Read(IBuffer buffer)
            {
                CDataArisenProfile obj = new CDataArisenProfile();
                obj.BackgroundId = ReadByte(buffer);
                obj.Title = ReadEntity<CDataAchievementIdentifier>(buffer);
                obj.MotionId = ReadUInt16(buffer);
                obj.MotionFrameNo = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
