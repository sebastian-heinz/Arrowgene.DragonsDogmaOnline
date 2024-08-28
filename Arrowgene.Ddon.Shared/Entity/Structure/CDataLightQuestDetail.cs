using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataLightQuestDetail
    {
        /// <summary>
        /// Area associated with the quest reward reputation.
        /// </summary>
        public uint AreaId { get; set; }
        public uint BaseAreaPoint { get; set; }
        /// <summary>
        /// Value of the quest reward reputation.
        /// </summary>
        public uint GetCP { get; set; } 
        public uint OrderLimit { get; set; }
        public uint ClearNum { get; set; }
        /// <summary>
        /// 1 = Regular Board?
        /// 2 = Clan Board?
        /// </summary>
        public uint BoardType { get; set; }
        public uint Unk0 { get; set; } // Possibly BoardId

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
                WriteUInt32(buffer, obj.Unk0);
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
                obj.Unk0 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
