using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CStageAreaChangeRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_STAGE_AREA_CHANGE_RES;

        public uint StageNo { get; set; }
        public bool IsBase { get; set; }
        public List<CDataCommonU32> StageFeatureList { get; set; }
        public List<CDataStageAreaChangeResUnk0> Unk0 { get; set; } // This is stage ID of something
        public List<CDataStageAreaChangeResUnk1> Unk1 { get; set; }

        public S2CStageAreaChangeRes()
        {
            StageNo=0;
            IsBase=false;
            StageFeatureList=new List<CDataCommonU32>();
            Unk0 = new List<CDataStageAreaChangeResUnk0>();
            Unk1 = new List<CDataStageAreaChangeResUnk1>();
        }

        public class Serializer : PacketEntitySerializer<S2CStageAreaChangeRes>
        {

            public override void Write(IBuffer buffer, S2CStageAreaChangeRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.StageNo);
                WriteBool(buffer, obj.IsBase);
                WriteEntityList(buffer, obj.StageFeatureList);
                WriteEntityList(buffer, obj.Unk0);
                WriteEntityList(buffer, obj.Unk1);
            }

            public override S2CStageAreaChangeRes Read(IBuffer buffer)
            {
                S2CStageAreaChangeRes obj = new S2CStageAreaChangeRes();
                ReadServerResponse(buffer, obj);
                obj.StageNo = ReadUInt32(buffer);
                obj.IsBase = ReadBool(buffer);
                obj.StageFeatureList = ReadEntityList<CDataCommonU32>(buffer);
                obj.Unk0 = ReadEntityList<CDataStageAreaChangeResUnk0>(buffer);
                obj.Unk1 = ReadEntityList<CDataStageAreaChangeResUnk1>(buffer);
                return obj;
            }
        }
    }
}
