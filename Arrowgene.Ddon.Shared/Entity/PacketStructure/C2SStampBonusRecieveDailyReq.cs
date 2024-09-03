using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SStampBonusRecieveDailyReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_STAMP_BONUS_RECIEVE_DAILY_REQ;

        public uint Unk0 {  get; set; }
        public uint Unk1 { get; set; }

        public class Serializer : PacketEntitySerializer<C2SStampBonusRecieveDailyReq>
        {
            public override void Write(IBuffer buffer, C2SStampBonusRecieveDailyReq obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
            }

            public override C2SStampBonusRecieveDailyReq Read(IBuffer buffer)
            {
                C2SStampBonusRecieveDailyReq obj = new C2SStampBonusRecieveDailyReq();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
