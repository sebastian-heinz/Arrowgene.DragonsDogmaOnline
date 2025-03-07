using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CStageGetSpAreaChangeInfoRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_STAGE_GET_SP_AREA_CHANGE_INFO_RES;


        public S2CStageGetSpAreaChangeInfoRes()
        {
            Unk1 = string.Empty;
            EntryCostList = new List<CDataStageDungeonItem>();
        }

        public uint Unk0 { get; set; }
        public string Unk1 { get; set; }
        public uint StageId { get; set; }
        public uint StartPos { get; set; }
        public bool Unk4 { get; set; }
        public List<CDataStageDungeonItem> EntryCostList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CStageGetSpAreaChangeInfoRes>
        {

            public override void Write(IBuffer buffer, S2CStageGetSpAreaChangeInfoRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.Unk0);
                WriteMtString(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.StageId);
                WriteUInt32(buffer, obj.StartPos);
                WriteBool(buffer, obj.Unk4);
                WriteEntityList(buffer, obj.EntryCostList);
            }

            public override S2CStageGetSpAreaChangeInfoRes Read(IBuffer buffer)
            {
                S2CStageGetSpAreaChangeInfoRes obj = new S2CStageGetSpAreaChangeInfoRes();
                ReadServerResponse(buffer, obj);
                obj.Unk0 = ReadUInt32(buffer);
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
