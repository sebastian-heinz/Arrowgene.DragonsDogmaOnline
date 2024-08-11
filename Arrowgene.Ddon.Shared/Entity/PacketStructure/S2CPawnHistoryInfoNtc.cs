using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnHistoryInfoNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PAWN_HISTORY_INFO_NTC;

        public S2CPawnHistoryInfoNtc()
        {
            PawnHistoryList = new List<CDataPawnHistory>();
        }

        public uint CharacterId { get; set; }
        public uint PawnId { get; set; }
        public List<CDataPawnHistory> PawnHistoryList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnHistoryInfoNtc>
        {
            public override void Write(IBuffer buffer, S2CPawnHistoryInfoNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntityList<CDataPawnHistory>(buffer, obj.PawnHistoryList);
            }

            public override S2CPawnHistoryInfoNtc Read(IBuffer buffer)
            {
                S2CPawnHistoryInfoNtc obj = new S2CPawnHistoryInfoNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.PawnId = ReadUInt32(buffer);
                obj.PawnHistoryList = ReadEntityList<CDataPawnHistory>(buffer);
                return obj;
            }
        }

    }
}
