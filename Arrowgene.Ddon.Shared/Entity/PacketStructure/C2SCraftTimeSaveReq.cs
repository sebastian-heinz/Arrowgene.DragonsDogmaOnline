using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCraftTimeSaveReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CRAFT_CRAFT_TIME_SAVE_REQ;

        public C2SCraftTimeSaveReq()
        {
        }

        public uint PawnID { get; set; }
        public byte ID { get; set; }
        public byte Num { get; set; }
        public bool IsInit { get; set; }

        public class Serializer : PacketEntitySerializer<C2SCraftTimeSaveReq>
        {
            public override void Write(IBuffer buffer, C2SCraftTimeSaveReq obj)
            {
                WriteUInt32(buffer, obj.PawnID);
                WriteByte(buffer, obj.ID);
                WriteByte(buffer, obj.Num);
                WriteBool(buffer, obj.IsInit);
            }

            public override C2SCraftTimeSaveReq Read(IBuffer buffer)
            {
                C2SCraftTimeSaveReq obj = new C2SCraftTimeSaveReq();

                obj.PawnID = ReadUInt32(buffer);
                obj.ID = ReadByte(buffer);
                obj.Num = ReadByte(buffer);
                obj.IsInit = ReadBool(buffer);

                return obj;
            }
        }
    }
}
