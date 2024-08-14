using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CGpChangeCapToGpRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_GP_CHANGE_CAP_TO_GP_RES;

        public uint ChangeListID { get; set; }
        public uint GP { get; set; }
        public uint CAP { get; set; }

        public S2CGpChangeCapToGpRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CGpChangeCapToGpRes>
        {
            public override void Write(IBuffer buffer, S2CGpChangeCapToGpRes obj)
            {
                WriteServerResponse(buffer, obj);

                WriteUInt32(buffer, obj.ChangeListID);
                WriteUInt32(buffer, obj.GP);
                WriteUInt32(buffer, obj.CAP);
            }

            public override S2CGpChangeCapToGpRes Read(IBuffer buffer)
            {
                S2CGpChangeCapToGpRes obj = new S2CGpChangeCapToGpRes();

                ReadServerResponse(buffer, obj);

                obj.ChangeListID = ReadUInt32(buffer);
                obj.GP = ReadUInt32(buffer);
                obj.CAP = ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
