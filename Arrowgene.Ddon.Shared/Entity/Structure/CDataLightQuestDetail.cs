using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataLightQuestDetail
    {
        /// <summary>
        /// Area associated with the quest reward reputation.
        /// </summary>
        public uint AreaId { get; set; }
        /// <summary>
        /// Which board the quest corresponds to.
        /// </summary>
        public uint BoardId { get; set; }

        /// <summary>
        /// Value of the quest reward reputation.
        /// </summary>
        public uint GetAp { get; set; } 

        /// <summary>
        /// 
        /// </summary>
        public uint GetCp { get; set; }

        /// <summary>
        /// Checked against ClearNum to see if you're eligible for clan point rewards.
        /// </summary>
        public uint OrderLimit { get; set; }
        public uint ClearNum { get; set; }

        /// <summary>
        /// 1 = Normal, 2 = Clan
        /// </summary>
        public uint BoardType { get; set; }

        public class Serializer : EntitySerializer<CDataLightQuestDetail>
        {
            public override void Write(IBuffer buffer, CDataLightQuestDetail obj)
            {
                WriteUInt32(buffer, obj.AreaId);
                WriteUInt32(buffer, obj.BoardId);
                WriteUInt32(buffer, obj.GetAp);
                WriteUInt32(buffer, obj.GetCp);
                WriteUInt32(buffer, obj.OrderLimit);
                WriteUInt32(buffer, obj.ClearNum);
                WriteUInt32(buffer, obj.BoardType);
            }
        
            public override CDataLightQuestDetail Read(IBuffer buffer)
            {
                CDataLightQuestDetail obj = new CDataLightQuestDetail();
                obj.AreaId = ReadUInt32(buffer);
                obj.BoardId = ReadUInt32(buffer);
                obj.GetAp = ReadUInt32(buffer);
                obj.GetCp = ReadUInt32(buffer);
                obj.OrderLimit = ReadUInt32(buffer);
                obj.ClearNum = ReadUInt32(buffer);
                obj.BoardType = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
