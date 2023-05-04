using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPawnHistory
    {
        public CDataPawnHistory() {
            DebtorBaseInfo = new CDataCommunityCharacterBaseInfo();
            PawnFeedback = new CDataPawnFeedback();
        }
    
        public uint PawnId { get; set; }
        public CDataCommunityCharacterBaseInfo DebtorBaseInfo { get; set; }
        public ulong ReturnDate { get; set; }
        public ulong AdventureTime { get; set; }
        public byte AdventureCount { get; set; }
        public byte CraftCount { get; set; }
        public uint KillEnemyNum { get; set; }
        public CDataPawnFeedback PawnFeedback { get; set; }
    
        public class Serializer : EntitySerializer<CDataPawnHistory>
        {
            public override void Write(IBuffer buffer, CDataPawnHistory obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteEntity<CDataCommunityCharacterBaseInfo>(buffer, obj.DebtorBaseInfo);
                WriteUInt64(buffer, obj.ReturnDate);
                WriteUInt64(buffer, obj.AdventureTime);
                WriteByte(buffer, obj.AdventureCount);
                WriteByte(buffer, obj.CraftCount);
                WriteUInt32(buffer, obj.KillEnemyNum);
                WriteEntity<CDataPawnFeedback>(buffer, obj.PawnFeedback);
            }
        
            public override CDataPawnHistory Read(IBuffer buffer)
            {
                CDataPawnHistory obj = new CDataPawnHistory();
                obj.PawnId = ReadUInt32(buffer);
                obj.DebtorBaseInfo = ReadEntity<CDataCommunityCharacterBaseInfo>(buffer);
                obj.ReturnDate = ReadUInt64(buffer);
                obj.AdventureTime = ReadUInt64(buffer);
                obj.AdventureCount = ReadByte(buffer);
                obj.CraftCount = ReadByte(buffer);
                obj.KillEnemyNum = ReadUInt32(buffer);
                obj.PawnFeedback = ReadEntity<CDataPawnFeedback>(buffer);
                return obj;
            }
        }
    }
}