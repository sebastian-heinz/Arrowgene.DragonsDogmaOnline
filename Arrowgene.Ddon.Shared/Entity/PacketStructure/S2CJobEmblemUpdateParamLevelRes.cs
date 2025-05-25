using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobEmblemUpdateParamLevelRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_JOB_EMBLEM_UPDATE_PARAM_LEVEL_RES;

        public S2CJobEmblemUpdateParamLevelRes()
        {
            EmblemPoints = new();
            EmblemStatParamList = new();
            Unk0List = new();
            EquipStatParamList = new();
            Unk2List = new();
            Unk3List = new();
        }

        public JobId JobId { get; set; }
        public List<CDataJobEmblemStatParam> EmblemStatParamList { get; set; }
        public List<CDataJobEmblemPlayPoint> Unk0List { get; set; }
        public CDataJobEmblemPoints EmblemPoints { get; set; }
        public List<CDataEquipStatParam> EquipStatParamList { get; set; }
        public uint Unk1 { get; set; }
        public List<CDataCommonU32> Unk2List { get; set; }
        public List<CDataJobEmblemUnk0> Unk3List { get; set; }

        public class Serializer : PacketEntitySerializer<S2CJobEmblemUpdateParamLevelRes>
        {
            public override void Write(IBuffer buffer, S2CJobEmblemUpdateParamLevelRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, (byte)obj.JobId);
                WriteEntityList(buffer, obj.EmblemStatParamList);
                WriteEntityList(buffer, obj.Unk0List);
                WriteEntity(buffer, obj.EmblemPoints);
                WriteEntityList(buffer, obj.EquipStatParamList);
                WriteUInt32(buffer, obj.Unk1);
                WriteEntityList(buffer, obj.Unk2List);
                WriteEntityList(buffer, obj.Unk3List);
            }

            public override S2CJobEmblemUpdateParamLevelRes Read(IBuffer buffer)
            {
                S2CJobEmblemUpdateParamLevelRes obj = new S2CJobEmblemUpdateParamLevelRes();
                ReadServerResponse(buffer, obj);
                obj.JobId = (JobId)ReadByte(buffer);
                obj.EmblemStatParamList = ReadEntityList<CDataJobEmblemStatParam>(buffer);
                obj.Unk0List = ReadEntityList<CDataJobEmblemPlayPoint>(buffer);
                obj.EmblemPoints = ReadEntity<CDataJobEmblemPoints>(buffer);
                obj.EquipStatParamList = ReadEntityList<CDataEquipStatParam>(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Unk2List = ReadEntityList<CDataCommonU32>(buffer);
                obj.Unk3List = ReadEntityList<CDataJobEmblemUnk0>(buffer);
                return obj;
            }
        }
    }
}
