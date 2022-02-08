using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CTraningRoomSetEnemyRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_INSTANCE_TRANING_ROOM_SET_ENEMY_RES;

        public class Serializer : EntitySerializer<S2CTraningRoomSetEnemyRes>
        {
            public override void Write(IBuffer buffer, S2CTraningRoomSetEnemyRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CTraningRoomSetEnemyRes Read(IBuffer buffer)
            {
                S2CTraningRoomSetEnemyRes obj = new S2CTraningRoomSetEnemyRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
        
    }
}