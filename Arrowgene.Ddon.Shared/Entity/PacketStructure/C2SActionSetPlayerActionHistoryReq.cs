using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SActionSetPlayerActionHistoryReq
    {
        public C2SActionSetPlayerActionHistoryReq()
        {
            unk0 = new List<C2SActionSetPlayerActionHistoryReqElement>();
        }

        public List<C2SActionSetPlayerActionHistoryReqElement> unk0;

    }

    public class C2SActionSetPlayerActionHistoryReqElement
    {
        public C2SActionSetPlayerActionHistoryReqElement()
        {
            unk0=0;
            unk1=0;
        }

        public byte unk0;
        public uint unk1;
    }

    public class C2SActionSetPlayerActionHistoryReqSerializer : EntitySerializer<C2SActionSetPlayerActionHistoryReq>
    {
        public override void Write(IBuffer buffer, C2SActionSetPlayerActionHistoryReq obj)
        {
            WriteEntityList<C2SActionSetPlayerActionHistoryReqElement>(buffer, obj.unk0);
        }

        public override C2SActionSetPlayerActionHistoryReq Read(IBuffer buffer)
        {
            C2SActionSetPlayerActionHistoryReq obj = new C2SActionSetPlayerActionHistoryReq();
            obj.unk0 = ReadEntityList<C2SActionSetPlayerActionHistoryReqElement>(buffer);
            return obj;
        }
    }

    public class C2SActionSetPlayerActionHistoryReqElementSerializer : EntitySerializer<C2SActionSetPlayerActionHistoryReqElement>
    {
        public override void Write(IBuffer buffer, C2SActionSetPlayerActionHistoryReqElement obj)
        {
            WriteByte(buffer, obj.unk0);
            WriteUInt32(buffer, obj.unk1);
        }

        public override C2SActionSetPlayerActionHistoryReqElement Read(IBuffer buffer)
        {
            C2SActionSetPlayerActionHistoryReqElement obj = new C2SActionSetPlayerActionHistoryReqElement();
            obj.unk0 = ReadByte(buffer);
            obj.unk1 = ReadUInt32(buffer);
            return obj;
        }
    }
}