using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnUpdatePawnReactionListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_UPDATE_PAWN_REACTION_LIST_RES;

        public S2CPawnUpdatePawnReactionListRes()
        {
            PawnReactionList = new();
        }

        public uint PawnId { get; set; }
        public List<CDataPawnReaction> PawnReactionList { get; set; }


        public class Serializer : PacketEntitySerializer<S2CPawnUpdatePawnReactionListRes>
        {
            public override void Write(IBuffer buffer, S2CPawnUpdatePawnReactionListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntityList<CDataPawnReaction>(buffer, obj.PawnReactionList);

            }

            public override S2CPawnUpdatePawnReactionListRes Read(IBuffer buffer)
            {
                S2CPawnUpdatePawnReactionListRes obj = new S2CPawnUpdatePawnReactionListRes();
                ReadServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntityList<CDataPawnReaction>(buffer, obj.PawnReactionList);
                return obj;
            }
        }
    }
}
