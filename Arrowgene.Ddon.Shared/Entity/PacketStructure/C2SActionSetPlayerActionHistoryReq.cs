using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SActionSetPlayerActionHistoryReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ACTION_SET_PLAYER_ACTION_HISTORY_REQ;

        public List<CDataC2SActionSetPlayerActionHistoryReqElement> Unk0 { get; set; }

        public C2SActionSetPlayerActionHistoryReq()
        {
            Unk0 = new List<CDataC2SActionSetPlayerActionHistoryReqElement>();
        }

        public class Serializer : PacketEntitySerializer<C2SActionSetPlayerActionHistoryReq>
        {
            public override void Write(IBuffer buffer, C2SActionSetPlayerActionHistoryReq obj)
            {
                WriteEntityList<CDataC2SActionSetPlayerActionHistoryReqElement>(buffer, obj.Unk0);
            }

            public override C2SActionSetPlayerActionHistoryReq Read(IBuffer buffer)
            {
                C2SActionSetPlayerActionHistoryReq obj = new C2SActionSetPlayerActionHistoryReq();
                obj.Unk0 = ReadEntityList<CDataC2SActionSetPlayerActionHistoryReqElement>(buffer);
                return obj;
            }
        }
    }
}
