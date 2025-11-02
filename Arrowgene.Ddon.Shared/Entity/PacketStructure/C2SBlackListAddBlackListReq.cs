using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBlackListAddBlackListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BLACK_LIST_ADD_BLACK_LIST_REQ;

        public CDataCommunityCharacterBaseInfo CharacterInfo { get; set; } = new();

        public class Serializer : PacketEntitySerializer<C2SBlackListAddBlackListReq>
        {
            public override void Write(IBuffer buffer, C2SBlackListAddBlackListReq obj)
            {
                WriteEntity(buffer, obj.CharacterInfo);
            }

            public override C2SBlackListAddBlackListReq Read(IBuffer buffer)
            {
                C2SBlackListAddBlackListReq obj = new C2SBlackListAddBlackListReq();

                obj.CharacterInfo = ReadEntity<CDataCommunityCharacterBaseInfo>(buffer);

                return obj;
            }
        }
    }
}
