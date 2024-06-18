using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataQuestCommand
    {
        public ushort Command { get; set; }
        public int Param01 { get; set; }
        public int Param02 { get; set; }
        public int Param03 { get; set; }
        public int Param04 { get; set; }

        public CDataQuestCommand()
        {
        }

        public CDataQuestCommand(CDataQuestCommand obj)
        {
            Command = obj.Command;
            Param01 = obj.Param01;
            Param02 = obj.Param02;
            Param03 = obj.Param03;
            Param04 = obj.Param04;
        }
    
        public class Serializer : EntitySerializer<CDataQuestCommand>
        {
            public override void Write(IBuffer buffer, CDataQuestCommand obj)
            {
                WriteUInt16(buffer, obj.Command);
                WriteInt32(buffer, obj.Param01);
                WriteInt32(buffer, obj.Param02);
                WriteInt32(buffer, obj.Param03);
                WriteInt32(buffer, obj.Param04);
            }
        
            public override CDataQuestCommand Read(IBuffer buffer)
            {
                CDataQuestCommand obj = new CDataQuestCommand();
                obj.Command = ReadUInt16(buffer);
                obj.Param01 = ReadInt32(buffer);
                obj.Param02 = ReadInt32(buffer);
                obj.Param03 = ReadInt32(buffer);
                obj.Param04 = ReadInt32(buffer);
                return obj;
            }
        }
    }
}
