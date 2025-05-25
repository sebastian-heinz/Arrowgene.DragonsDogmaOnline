using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobEmblemUpdateLevelRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_JOB_EMBLEM_UPDATE_LEVEL_RES;

        public S2CJobEmblemUpdateLevelRes()
        {
            Unk0List = new();
            EmblemPoints = new();
        }

        public JobId JobId { get; set; }
        public byte Level { get; set; }
        public List<CDataJobEmblemPlayPoint> Unk0List { get; set; } // Impacts leftside menu?
        public CDataJobEmblemPoints EmblemPoints { get; set; }

        public class Serializer : PacketEntitySerializer<S2CJobEmblemUpdateLevelRes>
        {
            public override void Write(IBuffer buffer, S2CJobEmblemUpdateLevelRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, (byte) obj.JobId);
                WriteByte(buffer, obj.Level);
                WriteEntityList(buffer, obj.Unk0List);
                WriteEntity(buffer, obj.EmblemPoints);
            }

            public override S2CJobEmblemUpdateLevelRes Read(IBuffer buffer)
            {
                S2CJobEmblemUpdateLevelRes obj = new S2CJobEmblemUpdateLevelRes();
                ReadServerResponse(buffer, obj);
                obj.JobId = (JobId) ReadByte(buffer);
                obj.Level = ReadByte(buffer);
                obj.Unk0List = ReadEntityList<CDataJobEmblemPlayPoint>(buffer);
                obj.EmblemPoints = ReadEntity<CDataJobEmblemPoints>(buffer);
                return obj;
            }
        }
    }
}
