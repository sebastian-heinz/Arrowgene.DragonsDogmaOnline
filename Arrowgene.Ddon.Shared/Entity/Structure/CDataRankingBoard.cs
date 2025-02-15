using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using System;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataRankingBoard
    {
        /// <summary>
        /// Passed back to the server in C2SRankingRankListReq and C2SRankingRankListByCharacterIdReq. 
        /// </summary>
        public uint BoardId { get; set; }
        /// <summary>
        /// The name of this quest is used as the display name of the board.
        /// </summary>
        public uint QuestId { get; set; }
        public RankingBoardState State { get; set; }

        /// <summary>
        /// Not used?
        /// </summary>
        public uint RegisteredNum { get; set; }

        /// <summary>
        /// The client expects a uint here, but there are only actual two values for the string, so we just expose a bool instead.
        /// </summary>
        public bool IsWarMission { get; set; }
        public DateTimeOffset Begin { get; set; }
        public DateTimeOffset End { get; set; }
        /// <summary>
        /// Not used?
        /// </summary>
        public DateTimeOffset Expire { get; set; }
        public DateTimeOffset Tallied { get; set; }

        public class Serializer : EntitySerializer<CDataRankingBoard>
        {
            public override void Write(IBuffer buffer, CDataRankingBoard obj)
            {
                WriteUInt32(buffer, obj.BoardId);
                WriteUInt32(buffer, obj.QuestId);
                WriteByte(buffer, (byte)obj.State);
                WriteUInt32(buffer, obj.RegisteredNum);
                WriteUInt32(buffer, obj.IsWarMission ? 2u : 1u); 

                WriteUInt64(buffer, (ulong)obj.Begin.ToUnixTimeSeconds());
                WriteUInt64(buffer, (ulong)obj.End.ToUnixTimeSeconds());
                WriteUInt64(buffer, (ulong)obj.Expire.ToUnixTimeSeconds());
                WriteUInt64(buffer, (ulong)obj.Tallied.ToUnixTimeSeconds());
            }

            public override CDataRankingBoard Read(IBuffer buffer)
            {
                CDataRankingBoard obj = new CDataRankingBoard();
                obj.BoardId = ReadUInt32(buffer);
                obj.QuestId = ReadUInt32(buffer);
                obj.State = (RankingBoardState)ReadByte(buffer);
                obj.RegisteredNum = ReadUInt32(buffer);
                obj.IsWarMission = ReadUInt32(buffer) >= 2;

                obj.Begin = DateTimeOffset.FromUnixTimeSeconds((long)ReadUInt64(buffer));
                obj.End = DateTimeOffset.FromUnixTimeSeconds((long)ReadUInt64(buffer));
                obj.Expire = DateTimeOffset.FromUnixTimeSeconds((long)ReadUInt64(buffer));
                obj.Tallied = DateTimeOffset.FromUnixTimeSeconds((long)ReadUInt64(buffer));
                return obj;
            }
        }
    }
}
