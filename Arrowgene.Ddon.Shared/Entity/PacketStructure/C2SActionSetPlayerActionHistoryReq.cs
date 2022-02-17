using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SActionSetPlayerActionHistoryReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ACTION_SET_PLAYER_ACTION_HISTORY_REQ;

        public List<C2SActionSetPlayerActionHistoryReqElement> Unk0 { get; set; }

        public C2SActionSetPlayerActionHistoryReq()
        {
            Unk0 = new List<C2SActionSetPlayerActionHistoryReqElement>();
        }

        public class Serializer : EntitySerializer<C2SActionSetPlayerActionHistoryReq>
        {
            static Serializer()
            {
                Id = PacketId.C2S_ACTION_SET_PLAYER_ACTION_HISTORY_REQ;
            }
            
            public override void Write(IBuffer buffer, C2SActionSetPlayerActionHistoryReq obj)
            {
                WriteEntityList<C2SActionSetPlayerActionHistoryReqElement>(buffer, obj.Unk0);
            }

            public override C2SActionSetPlayerActionHistoryReq Read(IBuffer buffer)
            {
                C2SActionSetPlayerActionHistoryReq obj = new C2SActionSetPlayerActionHistoryReq();
                obj.Unk0 = ReadEntityList<C2SActionSetPlayerActionHistoryReqElement>(buffer);
                return obj;
            }
        }
    }

    // TODO separate file?
    public class C2SActionSetPlayerActionHistoryReqElement
    {
        public byte Unk0 { get; set; }
        public uint Unk1 { get; set; }

        public C2SActionSetPlayerActionHistoryReqElement()
        {
            Unk0=0;
            Unk1=0;
        }

        public class Serializer : EntitySerializer<C2SActionSetPlayerActionHistoryReqElement>
        {
            public override void Write(IBuffer buffer, C2SActionSetPlayerActionHistoryReqElement obj)
            {
                WriteByte(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
            }

            public override C2SActionSetPlayerActionHistoryReqElement Read(IBuffer buffer)
            {
                C2SActionSetPlayerActionHistoryReqElement obj = new C2SActionSetPlayerActionHistoryReqElement();
                obj.Unk0 = ReadByte(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                return obj;
            }
        }
    }

}
