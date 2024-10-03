using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SStageUnisonAreaChangeReadyReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_STAGE_UNISON_AREA_CHANGE_READY_REQ;

        public C2SStageUnisonAreaChangeReadyReq()
        {
            EntryFeeList = new List<CDataStageTicketDungeonItemInfo>();
        }

        public List<CDataStageTicketDungeonItemInfo> EntryFeeList { get; set; }

        public class Serializer : PacketEntitySerializer<C2SStageUnisonAreaChangeReadyReq>
        {
            public override void Write(IBuffer buffer, C2SStageUnisonAreaChangeReadyReq obj)
            {
                WriteEntityList(buffer, obj.EntryFeeList);
            }

            public override C2SStageUnisonAreaChangeReadyReq Read(IBuffer buffer)
            {
                C2SStageUnisonAreaChangeReadyReq obj = new C2SStageUnisonAreaChangeReadyReq();
                obj.EntryFeeList = ReadEntityList<CDataStageTicketDungeonItemInfo>(buffer);
                return obj;
            }
        }
    }
}
