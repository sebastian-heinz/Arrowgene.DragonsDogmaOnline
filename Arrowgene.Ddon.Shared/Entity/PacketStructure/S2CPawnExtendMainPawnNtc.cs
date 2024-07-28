using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnExtendMainPawnNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PAWN_EXTEND_MAIN_PAWN_SLOT_NTC;

        public S2CPawnExtendMainPawnNtc()
        {
        }

        public byte AddNum {  get; set; }
        public byte TotalNum {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnExtendMainPawnNtc>
        {
            public override void Write(IBuffer buffer, S2CPawnExtendMainPawnNtc obj)
            {
                WriteByte(buffer, obj.AddNum);
                WriteByte(buffer, obj.TotalNum);
            }

            public override S2CPawnExtendMainPawnNtc Read(IBuffer buffer)
            {
                S2CPawnExtendMainPawnNtc obj = new S2CPawnExtendMainPawnNtc();
                obj.AddNum = ReadByte(buffer);
                obj.TotalNum = ReadByte(buffer);
                return obj;
            }

        }
    }
}
