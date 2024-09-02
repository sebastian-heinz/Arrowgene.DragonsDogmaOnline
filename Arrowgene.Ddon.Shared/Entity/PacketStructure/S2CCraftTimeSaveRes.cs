using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCraftTimeSaveRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CRAFT_CRAFT_TIME_SAVE_RES;

        public uint PawnID { get; set; }
        public uint RemainTime { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCraftTimeSaveRes>
        {
            public override void Write(IBuffer buffer, S2CCraftTimeSaveRes obj)
            {
                WriteServerResponse(buffer, obj);

                WriteUInt32(buffer, obj.PawnID);
                WriteUInt32(buffer, obj.RemainTime);
            }

            public override S2CCraftTimeSaveRes Read(IBuffer buffer)
            {
                S2CCraftTimeSaveRes obj = new S2CCraftTimeSaveRes();

                ReadServerResponse(buffer, obj);

                obj.PawnID = ReadUInt32(buffer);
                obj.RemainTime = ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
