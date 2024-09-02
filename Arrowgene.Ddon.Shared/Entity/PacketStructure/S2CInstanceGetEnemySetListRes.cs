using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInstanceGetEnemySetListRes : ServerResponse {

        public override PacketId Id => PacketId.S2C_INSTANCE_GET_ENEMY_SET_LIST_RES;

        public S2CInstanceGetEnemySetListRes() {
            LayoutId=new CDataStageLayoutId();
            SubGroupId=0;
            RandomSeed=61235;
            QuestId=0;
            EnemyList=new List<CDataLayoutEnemyData>();
            DropItemSetList=new List<CDataDropItemSetInfo>();
            NamedParamList=new List<CDataNamedEnemyParamClient>();
        }

        public CDataStageLayoutId LayoutId { get; set; }
        public byte SubGroupId { get; set; }
        public uint RandomSeed { get; set; }
        public uint QuestId { get; set; }
        public List<CDataLayoutEnemyData> EnemyList { get; set; }
        public List<CDataDropItemSetInfo> DropItemSetList { get; set; }
        public List<CDataNamedEnemyParamClient> NamedParamList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CInstanceGetEnemySetListRes> {
            

            public override void Write(IBuffer buffer, S2CInstanceGetEnemySetListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity<CDataStageLayoutId>(buffer, obj.LayoutId);
                WriteByte(buffer, obj.SubGroupId);
                WriteUInt32(buffer, obj.RandomSeed);
                WriteUInt32(buffer, obj.QuestId);
                WriteEntityList<CDataLayoutEnemyData>(buffer, obj.EnemyList);
                WriteEntityList<CDataDropItemSetInfo>(buffer, obj.DropItemSetList);
                WriteEntityList<CDataNamedEnemyParamClient>(buffer, obj.NamedParamList);
            }

            public override S2CInstanceGetEnemySetListRes Read(IBuffer buffer)
            {
                S2CInstanceGetEnemySetListRes obj = new S2CInstanceGetEnemySetListRes();
                ReadServerResponse(buffer, obj);
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.SubGroupId = ReadByte(buffer);
                obj.RandomSeed = ReadUInt32(buffer);
                obj.QuestId = ReadUInt32(buffer);
                obj.EnemyList = ReadEntityList<CDataLayoutEnemyData>(buffer);
                obj.DropItemSetList = ReadEntityList<CDataDropItemSetInfo>(buffer);
                obj.NamedParamList = ReadEntityList<CDataNamedEnemyParamClient>(buffer);
                return obj;
            }
        }
    }

}
