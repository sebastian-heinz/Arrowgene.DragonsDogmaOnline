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
        public List<object> Unk0 { get; set; }
        public List<object> Unk1 { get; set; }

        public S2CStageAreaChangeRes()
        {
            StageNo=0;
            IsBase=false;
            StageFeatureList=new List<CDataCommonU32>();
            Unk0 = new List<object>();
            Unk1 = new List<object>();
        }

        public class Serializer : PacketEntitySerializer<S2CStageAreaChangeRes>
        {

            public override void Write(IBuffer buffer, S2CStageAreaChangeRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.StageNo);
                WriteBool(buffer, obj.IsBase);
                WriteEntityList(buffer, obj.StageFeatureList);
                // TODO: Unk0 and Unk1
                WriteMtArray(buffer, obj.Unk0, (buf, objEntry) => { });
                WriteMtArray(buffer, obj.Unk1, (buf, objEntry) => { });
            }

            public override S2CStageAreaChangeRes Read(IBuffer buffer)
            {
                S2CStageAreaChangeRes obj = new S2CStageAreaChangeRes();
                ReadServerResponse(buffer, obj);
                obj.StageNo = ReadUInt32(buffer);
                obj.IsBase = ReadBool(buffer);
                obj.StageFeatureList = ReadEntityList<CDataCommonU32>(buffer);
                // TODO: Unk0 and Unk1
                obj.Unk0 = new List<object>();
                obj.Unk1 = new List<object>();
                return obj;
            }
        }
    }
}
