using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSetCommunicationShortcutReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PROFILE_SET_COMMUNICATION_SHORTCUT_LIST_REQ;

        public uint PageNo { get; set; }
        public uint ButtonNo { get; set; }
        public byte Type { get; set; }
        public byte Category { get; set; }
        public uint ID { get; set; }


        public C2SSetCommunicationShortcutReq() {
            PageNo = 0;
            ButtonNo = 0;
            Type = 0x00;
            Category = 0x00;
            ID = 0;

        }

        public class Serializer : PacketEntitySerializer<C2SSetCommunicationShortcutReq> {
            public override void Write(IBuffer buffer, C2SSetCommunicationShortcutReq obj)
            {
                WriteUInt32(buffer, obj.PageNo);
                WriteUInt32(buffer, obj.ButtonNo);
                WriteByte(buffer, obj.Type);
                WriteByte(buffer, obj.Category);
                WriteUInt32(buffer, obj.ID);
            }

            public override C2SSetCommunicationShortcutReq Read(IBuffer buffer)
            {
                C2SSetCommunicationShortcutReq obj = new C2SSetCommunicationShortcutReq();
                obj.PageNo = ReadUInt32(buffer);
                obj.ButtonNo = ReadUInt32(buffer);
                obj.Type = ReadByte(buffer);
                obj.Category = ReadByte(buffer);
                obj.ID = ReadUInt32(buffer);
                return obj;
            }
        }

    }

}
