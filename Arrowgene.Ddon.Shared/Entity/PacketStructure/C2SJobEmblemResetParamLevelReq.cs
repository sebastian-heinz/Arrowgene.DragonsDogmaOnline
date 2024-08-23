using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SJobEmblemResetParamLevelReq : IPacketStructure
    {
        public C2SJobEmblemResetParamLevelReq()
        {
            EmblemPointResetGGCostList = new();
            EmblemPointResetPPCostList = new();
            EmblemUIDs = new();
        }

        public PacketId Id => PacketId.C2S_JOB_EMBLEM_RESET_PARAM_LEVEL_REQ;

        public JobId JobId { get; set; }
        public List<CDataJobEmblemActionCostParam> EmblemPointResetGGCostList { get; set; }
        public List<CDataJobEmblemActionCostParam> EmblemPointResetPPCostList { get; set; }
        public List<string> EmblemUIDs { get; set; }

        public class Serializer : PacketEntitySerializer<C2SJobEmblemResetParamLevelReq>
        {
            public override void Write(IBuffer buffer, C2SJobEmblemResetParamLevelReq obj)
            {
                WriteByte(buffer, (byte) obj.JobId);
                WriteEntityList(buffer, obj.EmblemPointResetGGCostList);
                WriteEntityList(buffer, obj.EmblemPointResetPPCostList);
                WriteMtStringList(buffer, obj.EmblemUIDs);
            }

            public override C2SJobEmblemResetParamLevelReq Read(IBuffer buffer)
            {
                C2SJobEmblemResetParamLevelReq obj = new C2SJobEmblemResetParamLevelReq();
                obj.JobId = (JobId)ReadByte(buffer);
                obj.EmblemPointResetGGCostList = ReadEntityList<CDataJobEmblemActionCostParam>(buffer);
                obj.EmblemPointResetPPCostList = ReadEntityList<CDataJobEmblemActionCostParam>(buffer);
                obj.EmblemUIDs = ReadMtStringList(buffer);
                return obj;
            }
        }

    }
}

