using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CUserListJoinNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_USER_LIST_JOIN_NTC;

        public S2CUserListJoinNtc()
        {
            UserList=new List<CDataLobbyMemberInfo>();
        }

        public List<CDataLobbyMemberInfo> UserList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CUserListJoinNtc>
        {
            public override void Write(IBuffer buffer, S2CUserListJoinNtc obj)
            {
                WriteEntityList<CDataLobbyMemberInfo>(buffer, obj.UserList);
            }

            public override S2CUserListJoinNtc Read(IBuffer buffer)
            {
                S2CUserListJoinNtc obj = new S2CUserListJoinNtc();
                obj.UserList = ReadEntityList<CDataLobbyMemberInfo>(buffer);
                return obj;
            }
        }
    }
}