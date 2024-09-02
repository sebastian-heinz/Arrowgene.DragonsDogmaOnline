using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInstanceTraningRoomSetEnemyReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INSTANCE_TRANING_ROOM_SET_ENEMY_REQ;

        /// <summary>
        /// Value returned from the TraningRoomGetEnemyList dialog.
        /// </summary>
        public uint OptionId { get; set; } 
        public uint Lv { get; set; }

        public C2SInstanceTraningRoomSetEnemyReq()
        {
            OptionId = 0;
            Lv = 0;
        }

        public class Serializer : PacketEntitySerializer<C2SInstanceTraningRoomSetEnemyReq>
        {

            public override void Write(IBuffer buffer, C2SInstanceTraningRoomSetEnemyReq obj)
            {
                WriteUInt32(buffer, obj.OptionId);
                WriteUInt32(buffer, obj.Lv);
            }

            public override C2SInstanceTraningRoomSetEnemyReq Read(IBuffer buffer)
            {
                C2SInstanceTraningRoomSetEnemyReq obj = new C2SInstanceTraningRoomSetEnemyReq();
                obj.OptionId = ReadUInt32(buffer);
                obj.Lv = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
