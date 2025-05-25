using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobEmblemResetParamLevelRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_JOB_EMBLEM_RESET_PARAM_LEVEL_RES;

        public S2CJobEmblemResetParamLevelRes()
        {
            EmblemStatParamList = new();
            Unk0List = new();
            EmblemPoints = new();
            EquipStatParamList = new();
            Unk2List = new();
            Unk3List = new();
            UpdateWalletPointList = new();
            Unk4List = new();
        }

        public JobId JobId { get; set; }
        public List<CDataJobEmblemStatParam> EmblemStatParamList { get; set; }
        public List<CDataJobEmblemPlayPoint> Unk0List { get; set; }
        public CDataJobEmblemPoints EmblemPoints { get; set; }
        public List<CDataEquipStatParam> EquipStatParamList { get; set; }
        public uint Unk1 { get; set; }
        public List<CDataCommonU32> Unk2List { get; set; }
        public List<CDataJobEmblemUnk0> Unk3List { get; set; }
        public List<CDataUpdateWalletPoint> UpdateWalletPointList { get; set; } // Related to the wallet
        public List<CDataJobEmblemPlayPoint> Unk4List { get; set; } // Related to play points?

        public class Serializer : PacketEntitySerializer<S2CJobEmblemResetParamLevelRes>
        {
            public override void Write(IBuffer buffer, S2CJobEmblemResetParamLevelRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, (byte) obj.JobId);
                WriteEntityList(buffer, obj.EmblemStatParamList);
                WriteEntityList(buffer, obj.Unk0List);
                WriteEntity(buffer, obj.EmblemPoints);
                WriteEntityList(buffer, obj.EquipStatParamList);
                WriteUInt32(buffer, obj.Unk1);
                WriteEntityList(buffer, obj.Unk2List);
                WriteEntityList(buffer, obj.Unk3List);
                WriteEntityList(buffer, obj.UpdateWalletPointList);
                WriteEntityList(buffer, obj.Unk4List);
            }

            public override S2CJobEmblemResetParamLevelRes Read(IBuffer buffer)
            {
                S2CJobEmblemResetParamLevelRes obj = new S2CJobEmblemResetParamLevelRes();
                ReadServerResponse(buffer, obj);
                obj.JobId = (JobId)ReadByte(buffer);
                obj.EmblemStatParamList = ReadEntityList<CDataJobEmblemStatParam>(buffer);
                obj.Unk0List = ReadEntityList<CDataJobEmblemPlayPoint>(buffer);
                obj.EmblemPoints = ReadEntity<CDataJobEmblemPoints>(buffer);
                obj.EquipStatParamList = ReadEntityList<CDataEquipStatParam>(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Unk2List = ReadEntityList<CDataCommonU32>(buffer);
                obj.Unk3List = ReadEntityList<CDataJobEmblemUnk0>(buffer);
                obj.UpdateWalletPointList = ReadEntityList<CDataUpdateWalletPoint>(buffer);
                obj.Unk4List = ReadEntityList<CDataJobEmblemPlayPoint>(buffer);
                return obj;
            }
        }
    }
}
