using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBattleContentGetContentStatusFromOmRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BATTLE_CONTENT_GET_CONTENT_STATUS_FROM_OM_RES;

        public uint Unk0 { get; set; }
        public uint Unk1 { get; set; } // Related to either starting new or continuing?
        public uint Unk2 { get; set; }
        public uint Unk3 { get; set; }
        public uint Unk4 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CBattleContentGetContentStatusFromOmRes>
        {
            public override void Write(IBuffer buffer, S2CBattleContentGetContentStatusFromOmRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.Unk2);
                WriteUInt32(buffer, obj.Unk3);
                WriteUInt32(buffer, obj.Unk4);
            }

            public override S2CBattleContentGetContentStatusFromOmRes Read(IBuffer buffer)
            {
                S2CBattleContentGetContentStatusFromOmRes obj = new S2CBattleContentGetContentStatusFromOmRes();
                ReadServerResponse(buffer, obj);
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                obj.Unk3 = ReadUInt32(buffer);
                obj.Unk4 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
