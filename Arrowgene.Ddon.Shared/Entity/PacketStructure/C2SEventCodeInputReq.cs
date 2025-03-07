using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEventCodeInputReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_EVENT_CODE_EVENT_CODE_INPUT_REQ;

        public string Code { get; set; } = string.Empty;

        public C2SEventCodeInputReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SEventCodeInputReq>
        {
            public override void Write(IBuffer buffer, C2SEventCodeInputReq obj)
            {
                WriteMtString(buffer, obj.Code);
            }

            public override C2SEventCodeInputReq Read(IBuffer buffer)
            {
                C2SEventCodeInputReq obj = new C2SEventCodeInputReq();

                obj.Code = ReadMtString(buffer);

                return obj;
            }
        }
    }
}
