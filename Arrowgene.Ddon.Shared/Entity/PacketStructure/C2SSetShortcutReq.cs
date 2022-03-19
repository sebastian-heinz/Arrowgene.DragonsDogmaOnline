using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSetShortcutReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PROFILE_SET_SHORTCUT_LIST_REQ;

        public uint PageNo { get; set; }
        public uint ButtonNo { get; set; }
        public uint ShortcutID { get; set; }
        public uint U32Data { get; set; }
        public uint F32Data { get; set; }
        public byte ExexType { get; set; }

        public C2SSetShortcutReq() {
            PageNo = 0;
            ButtonNo = 0;
            ShortcutID = 0;
            U32Data = 0;
            F32Data = 0;
            ExexType = 0x00;
        }

        public class Serializer : PacketEntitySerializer<C2SSetShortcutReq> {
            public override void Write(IBuffer buffer, C2SSetShortcutReq obj)
            {
                WriteUInt32(buffer, obj.PageNo);
                WriteUInt32(buffer, obj.ButtonNo);
                WriteUInt32(buffer, obj.ShortcutID);
                WriteUInt32(buffer, obj.U32Data);
                WriteUInt32(buffer, obj.F32Data);
                WriteByte(buffer, obj.ExexType);
            }

            public override C2SSetShortcutReq Read(IBuffer buffer)
            {
                C2SSetShortcutReq obj = new C2SSetShortcutReq();
                obj.PageNo = ReadUInt32(buffer);
                obj.ButtonNo = ReadUInt32(buffer);
                obj.ShortcutID = ReadUInt32(buffer);
                obj.U32Data = ReadUInt32(buffer);
                obj.F32Data = ReadUInt32(buffer);
                obj.ExexType = ReadByte(buffer);
                return obj;
            }
        }

    }

}
