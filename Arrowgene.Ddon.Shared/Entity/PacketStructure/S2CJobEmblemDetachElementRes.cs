using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobEmblemDetachElementRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_JOB_EMBLEM_DETACH_ELEMENT_RES;

        public S2CJobEmblemDetachElementRes()
        {
            InheritanceResult = new();
        }

        public CDataJobEmblemInheritanceResult InheritanceResult { get; set; }

        public class Serializer : PacketEntitySerializer<S2CJobEmblemDetachElementRes>
        {
            public override void Write(IBuffer buffer, S2CJobEmblemDetachElementRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity(buffer, obj.InheritanceResult);
            }

            public override S2CJobEmblemDetachElementRes Read(IBuffer buffer)
            {
                S2CJobEmblemDetachElementRes obj = new S2CJobEmblemDetachElementRes();
                ReadServerResponse(buffer, obj);
                obj.InheritanceResult = ReadEntity<CDataJobEmblemInheritanceResult>(buffer);
                return obj;
            }
        }
    }
}
