using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CFriendGetFriendListRes : ServerResponse
    {
        public S2CFriendGetFriendListRes()
        {
            FriendInfoList = new();
            ApplyingCharacterList = new();
            ApprovingCharacterList = new();
        }

        public override PacketId Id => PacketId.S2C_FRIEND_GET_FRIEND_LIST_RES;
        
        public List<CDataFriendInfo> FriendInfoList { get; set; }
        public List<CDataCommunityCharacterBaseInfo> ApplyingCharacterList { get; set; }
        public List<CDataCommunityCharacterBaseInfo> ApprovingCharacterList { get; set; }


        public class Serializer : PacketEntitySerializer<S2CFriendGetFriendListRes>
        {
            public override void Write(IBuffer buffer, S2CFriendGetFriendListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.FriendInfoList);
                WriteEntityList(buffer, obj.ApplyingCharacterList);
                WriteEntityList(buffer, obj.ApprovingCharacterList);
                
            }

            public override S2CFriendGetFriendListRes Read(IBuffer buffer)
            {
                S2CFriendGetFriendListRes obj = new S2CFriendGetFriendListRes();
                ReadServerResponse(buffer, obj);
                obj.FriendInfoList = ReadEntityList<CDataFriendInfo>(buffer);
                obj.ApplyingCharacterList = ReadEntityList<CDataCommunityCharacterBaseInfo>(buffer);
                obj.ApprovingCharacterList = ReadEntityList<CDataCommunityCharacterBaseInfo>(buffer);
                return obj;
            }
            
        }
    }
}
