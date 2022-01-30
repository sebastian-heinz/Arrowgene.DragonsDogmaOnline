using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2STraningRoomGetEnemyListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INSTANCE_TRANING_ROOM_GET_ENEMY_LIST_REQ;
        public class Serializer : EntitySerializer<C2STraningRoomGetEnemyListReq>
        {
            public override void Write(IBuffer buffer, C2STraningRoomGetEnemyListReq obj)
            {
            }

            public override C2STraningRoomGetEnemyListReq Read(IBuffer buffer)
            {
                return new C2STraningRoomGetEnemyListReq();
            }
        }
    }

}