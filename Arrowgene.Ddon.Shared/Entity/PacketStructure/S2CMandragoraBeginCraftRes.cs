using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMandragoraBeginCraftRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_MANDRAGORA_BEGIN_CRAFT_RES;

        public CDataMyMandragoraBeginCraftResUnk0 Unk0 { get; set; } = new();

        public S2CMandragoraBeginCraftRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CMandragoraBeginCraftRes>
        {
            public override void Write(IBuffer buffer, S2CMandragoraBeginCraftRes obj)
            {
                WriteServerResponse(buffer, obj);

                WriteEntity<CDataMyMandragoraBeginCraftResUnk0>(buffer, obj.Unk0);
            }

            public override S2CMandragoraBeginCraftRes Read(IBuffer buffer)
            {
                S2CMandragoraBeginCraftRes obj = new S2CMandragoraBeginCraftRes();

                ReadServerResponse(buffer, obj);

                obj.Unk0 = ReadEntity<CDataMyMandragoraBeginCraftResUnk0>(buffer);

                return obj;
            }
        }
    }
}
