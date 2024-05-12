using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBazaarGetExhibitPossibleNumRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BAZAAR_GET_EXHIBIT_POSSIBLE_NUM_RES;

        public uint Num { get; set; }
        public uint Add { get; set; }

        public class Serializer : PacketEntitySerializer<S2CBazaarGetExhibitPossibleNumRes>
        {
            public override void Write(IBuffer buffer, S2CBazaarGetExhibitPossibleNumRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.Num);
                WriteUInt32(buffer, obj.Add);
            }

            public override S2CBazaarGetExhibitPossibleNumRes Read(IBuffer buffer)
            {
                S2CBazaarGetExhibitPossibleNumRes obj = new S2CBazaarGetExhibitPossibleNumRes();
                ReadServerResponse(buffer, obj);
                obj.Num = ReadUInt32(buffer);
                obj.Add = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}