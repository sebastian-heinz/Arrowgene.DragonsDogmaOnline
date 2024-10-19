using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnUpdatePawnReactionListNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PAWN_UPDATE_PAWN_REACTION_LIST_NTC;

        public S2CPawnUpdatePawnReactionListNtc()
        {
            PawnReactionList = new();
        }

        public uint PawnId { get; set; }
        public List<CDataPawnReaction> PawnReactionList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnUpdatePawnReactionListNtc>
        {
            public override void Write(IBuffer buffer, S2CPawnUpdatePawnReactionListNtc obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteEntityList<CDataPawnReaction>(buffer, obj.PawnReactionList);
            }

            public override S2CPawnUpdatePawnReactionListNtc Read(IBuffer buffer)
            {
                S2CPawnUpdatePawnReactionListNtc obj = new S2CPawnUpdatePawnReactionListNtc();
                obj.PawnId = ReadUInt32(buffer);
                obj.PawnReactionList = ReadEntityList<CDataPawnReaction>(buffer);
                return obj;
            }
        }
    }
}
