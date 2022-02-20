using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CTraningRoomGetEnemyListRes : ServerResponse
    {
        public S2CTraningRoomGetEnemyListRes()
        {
            InfoList = new List<CDataTraningRoomEnemyHeader>();
            MaxLv = 100;
        }

        public override PacketId Id => PacketId.S2C_INSTANCE_TRANING_ROOM_GET_ENEMY_LIST_RES;

        public List<CDataTraningRoomEnemyHeader> InfoList { get; set; }
        public uint MaxLv { get; set; }
        
        public class Serializer : PacketEntitySerializer<S2CTraningRoomGetEnemyListRes>
        {

            public override void Write(IBuffer buffer, S2CTraningRoomGetEnemyListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataTraningRoomEnemyHeader>(buffer, obj.InfoList);
                WriteUInt32(buffer, obj.MaxLv);
            }

            public override S2CTraningRoomGetEnemyListRes Read(IBuffer buffer)
            {
                S2CTraningRoomGetEnemyListRes obj = new S2CTraningRoomGetEnemyListRes();
                ReadServerResponse(buffer, obj);
                obj.InfoList = ReadEntityList<CDataTraningRoomEnemyHeader>(buffer);
                obj.MaxLv = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
