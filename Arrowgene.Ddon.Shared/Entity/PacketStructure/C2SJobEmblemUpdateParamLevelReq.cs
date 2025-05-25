using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SJobEmblemUpdateParamLevelReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_JOB_EMBLEM_UPDATE_PARAM_LEVEL_REQ;

        public C2SJobEmblemUpdateParamLevelReq()
        {
            UpdatedEmblemParamList = new();
            EmblemUIDs = new();
        }

        public JobId JobId { get; set; }
        public List<CDataJobEmblemStatParam> UpdatedEmblemParamList { get; set; }
        public List<string> EmblemUIDs { get; set; }

        public class Serializer : PacketEntitySerializer<C2SJobEmblemUpdateParamLevelReq>
        {
            public override void Write(IBuffer buffer, C2SJobEmblemUpdateParamLevelReq obj)
            {
                WriteByte(buffer, (byte)obj.JobId);
                WriteEntityList(buffer, obj.UpdatedEmblemParamList);
                WriteMtStringList(buffer, obj.EmblemUIDs);
            }

            public override C2SJobEmblemUpdateParamLevelReq Read(IBuffer buffer)
            {
                C2SJobEmblemUpdateParamLevelReq obj = new C2SJobEmblemUpdateParamLevelReq();
                obj.JobId = (JobId)ReadByte(buffer);
                obj.UpdatedEmblemParamList = ReadEntityList<CDataJobEmblemStatParam>(buffer);
                obj.EmblemUIDs = ReadMtStringList(buffer);
                return obj;
            }
        }
    }
}
