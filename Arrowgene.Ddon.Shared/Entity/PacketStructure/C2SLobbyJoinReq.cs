using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public struct C2SLobbyJoinReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_LOBBY_LOBBY_JOIN_REQ;

        public uint CharacterID;
        public uint UserListMaxNum;

        public class Serializer : EntitySerializer<C2SLobbyJoinReq>
        {
            public override void Write(IBuffer buffer, C2SLobbyJoinReq obj)
            {
                buffer.WriteUInt32(obj.CharacterID);
                buffer.WriteUInt32(obj.UserListMaxNum);
            }

            public override C2SLobbyJoinReq Read(IBuffer buffer)
            {
                C2SLobbyJoinReq obj = new C2SLobbyJoinReq();
                obj.CharacterID = buffer.ReadUInt32();
                obj.UserListMaxNum = buffer.ReadUInt32();
                return obj;
            }
        }
    }
}
