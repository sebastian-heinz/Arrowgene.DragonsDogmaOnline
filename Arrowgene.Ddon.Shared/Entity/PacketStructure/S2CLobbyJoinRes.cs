using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CLobbyJoinRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_LOBBY_LOBBY_JOIN_RES;

        public S2CLobbyJoinRes()
        {
            CharacterId = 0;
            LobbyMemberInfoList = new List<CDataLobbyMemberInfo>();
        }

        public uint CharacterId;
        public List<CDataLobbyMemberInfo> LobbyMemberInfoList;


        public class Serializer : PacketEntitySerializer<S2CLobbyJoinRes>
        {
            public override void Write(IBuffer buffer, S2CLobbyJoinRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.CharacterId);
                WriteEntityList(buffer, obj.LobbyMemberInfoList);
            }

            public override S2CLobbyJoinRes Read(IBuffer buffer)
            {
                S2CLobbyJoinRes obj = new S2CLobbyJoinRes();
                ReadServerResponse(buffer, obj);
                obj.CharacterId = ReadUInt32(buffer);
                obj.LobbyMemberInfoList = ReadEntityList<CDataLobbyMemberInfo>(buffer);
                return obj;
            }
        }
    }
}
