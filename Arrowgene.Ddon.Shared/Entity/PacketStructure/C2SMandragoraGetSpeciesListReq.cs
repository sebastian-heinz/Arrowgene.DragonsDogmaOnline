using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SMandragoraGetSpeciesListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_MANDRAGORA_GET_SPECIES_LIST_REQ;

        public MandragoraSpeciesCategory SpeciesCategory;

        public C2SMandragoraGetSpeciesListReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SMandragoraGetSpeciesListReq>
        {
            public override void Write(IBuffer buffer, C2SMandragoraGetSpeciesListReq obj)
            {
                WriteByte(buffer, (byte)obj.SpeciesCategory);
            }

            public override C2SMandragoraGetSpeciesListReq Read(IBuffer buffer)
            {
                C2SMandragoraGetSpeciesListReq obj = new C2SMandragoraGetSpeciesListReq();
                obj.SpeciesCategory = (MandragoraSpeciesCategory)ReadByte(buffer);
                return obj;
            }
        }
    }
}
