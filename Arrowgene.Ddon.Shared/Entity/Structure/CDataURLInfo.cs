using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataURLInfo
    {
        public CDataURLInfo()
        {
            Type = 0;
            URL = "";
        }

        public uint Type;
        public string URL;

        public class Serializer : EntitySerializer<CDataURLInfo>
        {
            public override void Write(IBuffer buffer, CDataURLInfo obj)
            {
                WriteUInt32(buffer, obj.Type);
                WriteMtString(buffer, obj.URL);
            }

            public override CDataURLInfo Read(IBuffer buffer)
            {
                CDataURLInfo obj = new CDataURLInfo();
                obj.Type = ReadUInt32(buffer);
                obj.URL = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
