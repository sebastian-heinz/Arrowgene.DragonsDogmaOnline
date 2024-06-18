using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataQuestProgressWork
    {
        public CDataQuestProgressWork()
        {
        }

        public CDataQuestProgressWork(CDataQuestProgressWork obj)
        {
            CommandNo = obj.CommandNo;
            Work01 = obj.Work01;
            Work02 = obj.Work02;
            Work03 = obj.Work03;
            Work04 = obj.Work04;
        }

        public uint CommandNo { get; set; }
        public int Work01 { get; set; }
        public int Work02 { get; set; }
        public int Work03 { get; set; }
        public int Work04 { get; set; }
    
        public class Serializer : EntitySerializer<CDataQuestProgressWork>
        {
            public override void Write(IBuffer buffer, CDataQuestProgressWork obj)
            {
                WriteUInt32(buffer, obj.CommandNo);
                WriteInt32(buffer, obj.Work01);
                WriteInt32(buffer, obj.Work02);
                WriteInt32(buffer, obj.Work03);
                WriteInt32(buffer, obj.Work04);
            }
        
            public override CDataQuestProgressWork Read(IBuffer buffer)
            {
                CDataQuestProgressWork obj = new CDataQuestProgressWork();
                obj.CommandNo = ReadUInt32(buffer);
                obj.Work01 = ReadInt32(buffer);
                obj.Work02 = ReadInt32(buffer);
                obj.Work03 = ReadInt32(buffer);
                obj.Work04 = ReadInt32(buffer);
                return obj;
            }
        }
    }
}
