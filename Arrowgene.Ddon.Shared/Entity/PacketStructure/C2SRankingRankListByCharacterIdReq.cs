using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SRankingRankListByCharacterIdReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_RANKING_RANK_LIST_BY_CHARACTER_ID_REQ;

        public uint BoardId { get; set; }
        public List<CDataCommonU32> CharacterIdList { get; set; } = new();

        public class Serializer : PacketEntitySerializer<C2SRankingRankListByCharacterIdReq>
        {
            public override void Write(IBuffer buffer, C2SRankingRankListByCharacterIdReq obj)
            {
                WriteUInt32(buffer, obj.BoardId);
                WriteEntityList(buffer, obj.CharacterIdList);
            }

            public override C2SRankingRankListByCharacterIdReq Read(IBuffer buffer)
            {
                C2SRankingRankListByCharacterIdReq obj = new C2SRankingRankListByCharacterIdReq();
                obj.BoardId = ReadUInt32(buffer);
                obj.CharacterIdList = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}
