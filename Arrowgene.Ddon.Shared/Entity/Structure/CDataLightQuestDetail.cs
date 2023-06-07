using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataLightQuestDetail
    {
        public uint AreaId { get; set; }
        public uint BaseAreaPoint { get; set; }
        public uint GetCP { get; set; }
        public uint OrderLimit { get; set; }
        public uint ClearNum { get; set; }
        public uint BoardType { get; set; }

        public class Serializer : EntitySerializer<CDataLightQuestDetail>
        {
            public override void Write(IBuffer buffer, CDataLightQuestDetail obj)
            {
                WriteUInt32(buffer, obj.AreaId);
                WriteUInt32(buffer, obj.BaseAreaPoint);
                WriteUInt32(buffer, obj.GetCP);
                WriteUInt32(buffer, obj.OrderLimit);
                WriteUInt32(buffer, obj.ClearNum);
                WriteUInt32(buffer, obj.BoardType);
            }
        
            public override CDataLightQuestDetail Read(IBuffer buffer)
            {
                CDataLightQuestDetail obj = new CDataLightQuestDetail();
                obj.AreaId = ReadUInt32(buffer);
                obj.BaseAreaPoint = ReadUInt32(buffer);
                obj.GetCP = ReadUInt32(buffer);
                obj.OrderLimit = ReadUInt32(buffer);
                obj.ClearNum = ReadUInt32(buffer);
                obj.BoardType = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}