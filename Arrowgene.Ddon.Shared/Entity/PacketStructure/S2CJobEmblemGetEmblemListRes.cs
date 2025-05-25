using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobEmblemGetEmblemListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_JOB_EMBLEM_GET_EMBLEM_LIST_RES;

        public S2CJobEmblemGetEmblemListRes()
        {
            JobEmblemList = new();
            EmblemPointList = new();
            EmblemSettings = new();
        }

        public List<CDataJobEmblem> JobEmblemList { get; set; }
        public List<CDataJobEmblemPoints> EmblemPointList { get; set; }
        public CDataJobEmblemSettings EmblemSettings { get; set; }

        public class Serializer : PacketEntitySerializer<S2CJobEmblemGetEmblemListRes>
        {
            public override void Write(IBuffer buffer, S2CJobEmblemGetEmblemListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.JobEmblemList);
                WriteEntityList(buffer, obj.EmblemPointList);
                WriteEntity(buffer, obj.EmblemSettings);
            }

            public override S2CJobEmblemGetEmblemListRes Read(IBuffer buffer)
            {
                S2CJobEmblemGetEmblemListRes obj = new S2CJobEmblemGetEmblemListRes();
                ReadServerResponse(buffer, obj);
                obj.JobEmblemList = ReadEntityList<CDataJobEmblem>(buffer);
                obj.EmblemPointList = ReadEntityList<CDataJobEmblemPoints>(buffer);
                obj.EmblemSettings = ReadEntity<CDataJobEmblemSettings>(buffer);
                return obj;
            }
        }
    }
}
