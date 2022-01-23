using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInstanceGetEnemySetListRes {

        public C2SInstanceGetEnemySetListRes() {
            stageId = 0;
            layerNo = 0;
            groupId = 0;
            subgroupId = 0;
            stageId = 0;
            layerNo = 0;
            groupId = 0;
            subgroupId = 0;
            enemyArray = new byte[] {
               0x0, 0x1, 0x0, 0x0, //Array details (?, length, ?, ?)
            //Enemy 1
            0x1, 0x3, 0x14, 0x1, //Enemy ID
            0x0, 0x8, 0xFA, 0x0, //Named Enemy Params
            0x0, 0x0, 0x0, 0x0, //RaidBoss Id
            0x64, 0x0, //Scale
            0x5E, 0x0,  //Level
            0x0, //StartThinkTbl (Start Think Table?)
            0x0, //RepopNum
            0x0, //RepopCount
            0x0, //EnemyTargetTypes
            0x0, //MontageFix (?)
            0x0, //SetType
            0x0, //InfectionType
            0x0, //BossBauge
            0x0, //BossVGM
            0x0, //IsManualSet (?)
            0x0, //Is Area Boss
            0x0, //Is blood enemy
            0x0, //??
            0x0, //??
            0x0, //??
            0x0, //??
            
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 //Packet end
            };
            randomSeed = 61235;
            questId = 0;
        }

        public uint stageId;
        public byte layerNo;
        public uint groupId;
        public byte subgroupId;
         public uint randomSeed;
        public uint questId;
        public byte[] enemyArray;

    }

    public class C2SInstanceGetEnemySetListResSerializer : EntitySerializer<C2SInstanceGetEnemySetListRes> {
        public override void Write(IBuffer buffer, C2SInstanceGetEnemySetListRes obj)
        {
            WriteUInt32(buffer, obj.stageId);
            WriteByte(buffer, obj.layerNo);
            WriteUInt32(buffer, obj.groupId);
            WriteByte(buffer, obj.subgroupId);
            WriteUInt32(buffer, obj.randomSeed);
            WriteUInt32(buffer,obj.questId);
            WriteByteArray(buffer,obj.enemyArray);
        }

        public override C2SInstanceGetEnemySetListRes Read(IBuffer buffer)
        {
            C2SInstanceGetEnemySetListRes obj = new C2SInstanceGetEnemySetListRes();
            obj.stageId = ReadUInt32(buffer);
            obj.layerNo = ReadByte(buffer);
            obj.groupId = ReadUInt32(buffer);
            obj.subgroupId = ReadByte(buffer);
            return obj;
        }
    }
}