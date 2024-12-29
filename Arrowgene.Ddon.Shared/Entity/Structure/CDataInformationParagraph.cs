using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataInformationParagraph
    {
        public CDataInformationParagraph()
        {
            Text = string.Empty;
        }

        public uint Index { get; set; }
        public string Text { get; set; }

        public class Serializer : EntitySerializer<CDataInformationParagraph>
        {
            public override void Write(IBuffer buffer, CDataInformationParagraph obj)
            {
                WriteUInt32(buffer, obj.Index);
                WriteMtString(buffer, obj.Text);
            }

            public override CDataInformationParagraph Read(IBuffer buffer)
            {
                CDataInformationParagraph obj = new CDataInformationParagraph();
                obj.Index = ReadUInt32(buffer);
                obj.Text = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
