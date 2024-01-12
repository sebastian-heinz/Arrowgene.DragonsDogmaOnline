using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataQuestDefine
    {
        public byte OrderMaxNum { get; set; }
        public byte ChargeAddOrderNum { get; set; }
        public byte RewardBoxMaxNum { get; set; }
        public ushort CycleContentsPlaydataRemainDay { get; set; }
    
        public class Serializer : EntitySerializer<CDataQuestDefine>
        {
            public override void Write(IBuffer buffer, CDataQuestDefine obj)
            {
                WriteByte(buffer, obj.OrderMaxNum);
                WriteByte(buffer, obj.ChargeAddOrderNum);
                WriteByte(buffer, obj.RewardBoxMaxNum);
                WriteUInt16(buffer, obj.CycleContentsPlaydataRemainDay);
            }
        
            public override CDataQuestDefine Read(IBuffer buffer)
            {
                CDataQuestDefine obj = new CDataQuestDefine();
                obj.OrderMaxNum = ReadByte(buffer);
                obj.ChargeAddOrderNum = ReadByte(buffer);
                obj.RewardBoxMaxNum = ReadByte(buffer);
                obj.CycleContentsPlaydataRemainDay = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}