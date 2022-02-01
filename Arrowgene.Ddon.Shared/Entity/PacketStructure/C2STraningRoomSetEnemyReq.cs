using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2STraningRoomSetEnemyReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INSTANCE_TRANING_ROOM_SET_ENEMY_REQ;

        public uint ID { get; set; }
        public uint Lv { get; set; }

        public C2STraningRoomSetEnemyReq()
        {
            ID = 0;
            Lv = 0;
        }

        public class Serializer : EntitySerializer<C2STraningRoomSetEnemyReq>
        {
            public override void Write(IBuffer buffer, C2STraningRoomSetEnemyReq obj)
            {
                WriteUInt32(buffer, obj.ID);
                WriteUInt32(buffer, obj.Lv);
            }

            public override C2STraningRoomSetEnemyReq Read(IBuffer buffer)
            {
                C2STraningRoomSetEnemyReq obj = new C2STraningRoomSetEnemyReq();
                obj.ID = ReadUInt32(buffer);
                obj.Lv = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}