using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CConnectionInformationNtc : ServerResponse
    {
        public S2CConnectionInformationNtc()
        {
            ParagraphList = new();
        }

        public override PacketId Id => PacketId.S2C_CONNECTION_INFORMATION_NTC;

        public List<CDataInformationParagraph> ParagraphList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CConnectionInformationNtc>
        {
            public override void Write(IBuffer buffer, S2CConnectionInformationNtc obj)
            {
                WriteEntityList(buffer, obj.ParagraphList);
            }

            public override S2CConnectionInformationNtc Read(IBuffer buffer)
            {
                S2CConnectionInformationNtc obj = new S2CConnectionInformationNtc();
                obj.ParagraphList = ReadEntityList<CDataInformationParagraph>(buffer);
                return obj;
            }
        }
    }
}
