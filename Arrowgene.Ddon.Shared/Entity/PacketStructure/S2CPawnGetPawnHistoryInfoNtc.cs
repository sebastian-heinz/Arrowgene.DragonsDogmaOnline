using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnGetPawnHistoryInfoNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PAWN_GET_PAWN_HISTORY_INFO_NTC;

        public S2CPawnGetPawnHistoryInfoNtc()
        {
            PawnHistoryList = new List<CDataPawnHistory>();
        }

        public uint CharacterId { get; set; }
        public uint PawnId { get; set; }
        public List<CDataPawnHistory> PawnHistoryList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnGetPawnHistoryInfoNtc>
        {
            public override void Write(IBuffer buffer, S2CPawnGetPawnHistoryInfoNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntityList<CDataPawnHistory>(buffer, obj.PawnHistoryList);
            }

            public override S2CPawnGetPawnHistoryInfoNtc Read(IBuffer buffer)
            {
                S2CPawnGetPawnHistoryInfoNtc obj = new S2CPawnGetPawnHistoryInfoNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.PawnId = ReadUInt32(buffer);
                obj.PawnHistoryList = ReadEntityList<CDataPawnHistory>(buffer);
                return obj;
            }
        }
    }
}
