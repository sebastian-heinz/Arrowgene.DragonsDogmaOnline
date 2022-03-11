using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2STrainingRoomSetEnemyReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INSTANCE_TRANING_ROOM_SET_ENEMY_REQ;

        public uint ID { get; set; } // TODO is this enemyId ?
        public uint Lv { get; set; }

        public C2STrainingRoomSetEnemyReq()
        {
            ID = 0;
            Lv = 0;
        }

        public class Serializer : PacketEntitySerializer<C2STrainingRoomSetEnemyReq>
        {

            public override void Write(IBuffer buffer, C2STrainingRoomSetEnemyReq obj)
            {
                WriteUInt32(buffer, obj.ID);
                WriteUInt32(buffer, obj.Lv);
            }

            public override C2STrainingRoomSetEnemyReq Read(IBuffer buffer)
            {
                C2STrainingRoomSetEnemyReq obj = new C2STrainingRoomSetEnemyReq();
                obj.ID = ReadUInt32(buffer);
                obj.Lv = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
