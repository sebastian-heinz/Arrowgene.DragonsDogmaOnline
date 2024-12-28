using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataC2SActionSetPlayerActionHistoryReqElement
    {
        public byte Unk0 { get; set; }
        public uint Unk1 { get; set; }

        public CDataC2SActionSetPlayerActionHistoryReqElement()
        {
            Unk0 = 0;
            Unk1 = 0;
        }

        public class Serializer : EntitySerializer<CDataC2SActionSetPlayerActionHistoryReqElement>
        {
            public override void Write(IBuffer buffer, CDataC2SActionSetPlayerActionHistoryReqElement obj)
            {
                WriteByte(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
            }

            public override CDataC2SActionSetPlayerActionHistoryReqElement Read(IBuffer buffer)
            {
                CDataC2SActionSetPlayerActionHistoryReqElement obj = new CDataC2SActionSetPlayerActionHistoryReqElement();
                obj.Unk0 = ReadByte(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
