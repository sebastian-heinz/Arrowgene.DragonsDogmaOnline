using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataStampCheck
    {
        public uint Unk0 { get; set; }
        public uint Unk1 { get; set; }

        public class Serializer : EntitySerializer<CDataStampCheck>
        {
            public override void Write(IBuffer buffer, CDataStampCheck obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
            }

            public override CDataStampCheck Read(IBuffer buffer)
            {
                CDataStampCheck obj = new CDataStampCheck();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
