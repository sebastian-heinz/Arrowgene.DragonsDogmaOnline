using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInstanceTraningRoomGetEnemyListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INSTANCE_TRANING_ROOM_GET_ENEMY_LIST_REQ;
        public class Serializer : PacketEntitySerializer<C2SInstanceTraningRoomGetEnemyListReq>
        {

            public override void Write(IBuffer buffer, C2SInstanceTraningRoomGetEnemyListReq obj)
            {
            }

            public override C2SInstanceTraningRoomGetEnemyListReq Read(IBuffer buffer)
            {
                return new C2SInstanceTraningRoomGetEnemyListReq();
            }
        }
    }

}
