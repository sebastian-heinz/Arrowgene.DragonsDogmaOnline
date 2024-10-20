using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnUpdatePawnReactionListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_UPDATE_PAWN_REACTION_LIST_REQ;

        public C2SPawnUpdatePawnReactionListReq()
        {
            PawnReactionList = new();
        }

        public uint PawnId { get; set; }
        public List<CDataPawnReaction> PawnReactionList { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPawnUpdatePawnReactionListReq>
        {
            public override void Write(IBuffer buffer, C2SPawnUpdatePawnReactionListReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteEntityList<CDataPawnReaction>(buffer, obj.PawnReactionList);
            }

            public override C2SPawnUpdatePawnReactionListReq Read(IBuffer buffer)
            {
                C2SPawnUpdatePawnReactionListReq obj = new C2SPawnUpdatePawnReactionListReq();
                obj.PawnId = ReadUInt32(buffer);
                obj.PawnReactionList = ReadEntityList<CDataPawnReaction>(buffer);
                return obj;
            }
        }
    }
}
