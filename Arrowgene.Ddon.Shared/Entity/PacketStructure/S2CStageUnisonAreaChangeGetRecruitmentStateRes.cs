using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CStageUnisonAreaChangeGetRecruitmentStateRes : ServerResponse
    {
        public S2CStageUnisonAreaChangeGetRecruitmentStateRes()
        {
            EntryCostList = new List<CDataStageDungeonItem>();
            Unk1 = string.Empty;
        }

        public override PacketId Id => PacketId.S2C_STAGE_UNISON_AREA_CHANGE_GET_RECRUITMENT_STATE_RES;

        // Same as S2CStageTicketDungeonStartNtc
        public uint ContentId { get; set; }
        public string Unk1 { get; set; }
        public uint StageId { get; set; }
        public uint StartPos { get; set; }
        public bool Unk4 { get; set; }
        public List<CDataStageDungeonItem> EntryCostList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CStageUnisonAreaChangeGetRecruitmentStateRes>
        {
            public override void Write(IBuffer buffer, S2CStageUnisonAreaChangeGetRecruitmentStateRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.ContentId);
                WriteMtString(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.StageId);
                WriteUInt32(buffer, obj.StartPos);
                WriteBool(buffer, obj.Unk4);
                WriteEntityList(buffer, obj.EntryCostList);
            }

            public override S2CStageUnisonAreaChangeGetRecruitmentStateRes Read(IBuffer buffer)
            {
                S2CStageUnisonAreaChangeGetRecruitmentStateRes obj = new S2CStageUnisonAreaChangeGetRecruitmentStateRes();
                ReadServerResponse(buffer, obj);
                obj.ContentId = ReadUInt32(buffer);
                obj.Unk1 = ReadMtString(buffer);
                obj.StageId = ReadUInt32(buffer);
                obj.StartPos = ReadUInt32(buffer);
                obj.Unk4 = ReadBool(buffer);
                obj.EntryCostList = ReadEntityList<CDataStageDungeonItem>(buffer);
                return obj;
            }
        }
    }
}
