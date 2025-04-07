using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataOtherRoomLayoutUpdate
    {
        public uint UpdateOmId { get; set; }
        public uint CharacterId { get; set; }

        public class Serializer : EntitySerializer<CDataOtherRoomLayoutUpdate>
        {
            public override void Write(IBuffer buffer, CDataOtherRoomLayoutUpdate obj)
            {
                WriteUInt32(buffer, obj.UpdateOmId);
                WriteUInt32(buffer, obj.CharacterId);
            }

            public override CDataOtherRoomLayoutUpdate Read(IBuffer buffer)
            {
                CDataOtherRoomLayoutUpdate obj = new CDataOtherRoomLayoutUpdate();
                obj.UpdateOmId = ReadUInt32(buffer);
                obj.CharacterId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
