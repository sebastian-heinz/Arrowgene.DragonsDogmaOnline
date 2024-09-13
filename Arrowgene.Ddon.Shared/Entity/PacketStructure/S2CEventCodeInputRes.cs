using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEventCodeInputRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_EVENT_CODE_EVENT_CODE_INPUT_RES;

        public string Name { get; set; }

        public S2CEventCodeInputRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CEventCodeInputRes>
        {
            public override void Write(IBuffer buffer, S2CEventCodeInputRes obj)
            {
                WriteServerResponse(buffer, obj);

                WriteMtString(buffer, obj.Name);
            }

            public override S2CEventCodeInputRes Read(IBuffer buffer)
            {
                S2CEventCodeInputRes obj = new S2CEventCodeInputRes();

                ReadServerResponse(buffer, obj);

                obj.Name = ReadMtString(buffer);

                return obj;
            }
        }
    }
}
