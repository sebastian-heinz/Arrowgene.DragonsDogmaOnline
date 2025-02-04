using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPawnExpeditionClanSallySpotInfo
    {
        public uint AreaId { get; set; }
        public uint SpotId { get; set; }
        public uint SallyNum { get; set; }

        public class Serializer : EntitySerializer<CDataPawnExpeditionClanSallySpotInfo>
        {
            public override void Write(IBuffer buffer, CDataPawnExpeditionClanSallySpotInfo obj)
            {
                WriteUInt32(buffer, obj.AreaId);
                WriteUInt32(buffer, obj.SpotId);
                WriteUInt32(buffer, obj.SallyNum);
            }

            public override CDataPawnExpeditionClanSallySpotInfo Read(IBuffer buffer)
            {
                CDataPawnExpeditionClanSallySpotInfo obj = new CDataPawnExpeditionClanSallySpotInfo();
                obj.AreaId = ReadUInt32(buffer);
                obj.SpotId = ReadUInt32(buffer);
                obj.SallyNum = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
