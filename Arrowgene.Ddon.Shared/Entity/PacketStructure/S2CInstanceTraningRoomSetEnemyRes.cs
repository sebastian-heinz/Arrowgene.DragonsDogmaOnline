using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInstanceTraningRoomSetEnemyRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_INSTANCE_TRANING_ROOM_SET_ENEMY_RES;

        public class Serializer : PacketEntitySerializer<S2CInstanceTraningRoomSetEnemyRes>
        {

            public override void Write(IBuffer buffer, S2CInstanceTraningRoomSetEnemyRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CInstanceTraningRoomSetEnemyRes Read(IBuffer buffer)
            {
                S2CInstanceTraningRoomSetEnemyRes obj = new S2CInstanceTraningRoomSetEnemyRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
        
    }
}
