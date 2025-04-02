using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCharacterReleaseElement
    {
        public CDataCharacterReleaseElement()
        {
            ReleaseElementNo = 0;
        }

        public CDataCharacterReleaseElement(ContentsRelease ReleaseElement)
        {
            ReleaseElementNo = (uint) ReleaseElement;
        }

        public uint ReleaseElementNo { get; set; }

        public class Serializer : EntitySerializer<CDataCharacterReleaseElement>
        {
            public override void Write(IBuffer buffer, CDataCharacterReleaseElement obj)
            {
                WriteUInt32(buffer, obj.ReleaseElementNo);
            }

            public override CDataCharacterReleaseElement Read(IBuffer buffer)
            {
                CDataCharacterReleaseElement obj = new CDataCharacterReleaseElement();
                obj.ReleaseElementNo = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
