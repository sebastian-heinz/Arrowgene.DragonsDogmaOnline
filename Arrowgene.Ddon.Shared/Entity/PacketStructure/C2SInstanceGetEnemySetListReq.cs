using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInstanceGetEnemySetListReq {

        public C2SInstanceGetEnemySetListReq() {
            stageId = 0;
            layerNo = 0;
            groupId = 0;
            subgroupId = 0;
        }

        public uint stageId;
        public byte layerNo;
        public uint groupId;
        public byte subgroupId;
    }

    public class C2SInstanceGetEnemySetListReqSerializer : EntitySerializer<C2SInstanceGetEnemySetListReq> {
        public override void Write(IBuffer buffer, C2SInstanceGetEnemySetListReq obj)
        {
            WriteUInt32(buffer, obj.stageId);
            WriteByte(buffer, obj.layerNo);
            WriteUInt32(buffer, obj.groupId);
            WriteByte(buffer, obj.subgroupId);
        }

        public override C2SInstanceGetEnemySetListReq Read(IBuffer buffer)
        {
            C2SInstanceGetEnemySetListReq obj = new C2SInstanceGetEnemySetListReq();
            obj.stageId = ReadUInt32(buffer);
            obj.layerNo = ReadByte(buffer);
            obj.groupId = ReadUInt32(buffer);
            obj.subgroupId = ReadByte(buffer);
            return obj;
        }
    }
}