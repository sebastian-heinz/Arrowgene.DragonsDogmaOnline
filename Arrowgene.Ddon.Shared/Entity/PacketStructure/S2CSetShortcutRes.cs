using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure {
    public class S2CSetShortcutRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PROFILE_SET_SHORTCUT_LIST_RES;

     //   public uint unk1 { get; set; }
     //   public uint unk2 { get; set; }
        public S2CSetShortcutRes() {
      //      unk1 = 0;
      //      unk2 = 0;
        }

        public class Serializer : PacketEntitySerializer<S2CSetShortcutRes> {
            public override void Write(IBuffer buffer, S2CSetShortcutRes obj)
            {
                WriteServerResponse(buffer, obj);
       //        WriteUInt32(buffer, obj.unk1);
       //        WriteUInt32(buffer, obj.unk2);
            }

            public override S2CSetShortcutRes Read(IBuffer buffer)
            {
                S2CSetShortcutRes obj = new S2CSetShortcutRes();
                ReadServerResponse(buffer, obj);
             //   obj.unk1 = ReadUInt32(buffer);
            //  obj.unk2 = ReadUInt32(buffer);
                return obj;
            }
        }

    }

}
