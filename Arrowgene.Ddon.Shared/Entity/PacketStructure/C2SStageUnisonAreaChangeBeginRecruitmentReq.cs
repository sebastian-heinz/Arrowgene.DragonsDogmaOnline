using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SStageUnisonAreaChangeBeginRecruitmentReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_STAGE_UNISON_AREA_CHANGE_BEGIN_RECRUITMENT_REQ;

        public C2SStageUnisonAreaChangeBeginRecruitmentReq()
        {
            EntryFeeList = new List<CDataStageTicketDungeonItemInfo>();
            Unk2String = string.Empty;
        }

        public uint ContentId { get; set; }
        public List<CDataStageTicketDungeonItemInfo> EntryFeeList { get; set; }
        public uint EntranceId { get; set; }
        public string Unk2String { get; set; }

        public class Serializer : PacketEntitySerializer<C2SStageUnisonAreaChangeBeginRecruitmentReq>
        {
            public override void Write(IBuffer buffer, C2SStageUnisonAreaChangeBeginRecruitmentReq obj)
            {
                WriteUInt32(buffer, obj.ContentId);
                WriteEntityList(buffer, obj.EntryFeeList);
                WriteUInt32(buffer, obj.EntranceId);
                WriteMtString(buffer, obj.Unk2String);
            }

            public override C2SStageUnisonAreaChangeBeginRecruitmentReq Read(IBuffer buffer)
            {
                C2SStageUnisonAreaChangeBeginRecruitmentReq obj = new C2SStageUnisonAreaChangeBeginRecruitmentReq();
                obj.ContentId = ReadUInt32(buffer);
                obj.EntryFeeList = ReadEntityList<CDataStageTicketDungeonItemInfo>(buffer);
                obj.EntranceId = ReadUInt32(buffer);
                obj.Unk2String = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
