using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataQuestProcessState
{
    public CDataQuestProcessState()
    {
        WorkList = new List<CDataQuestProgressWork>();
        CheckCommandList = new List<MtTypedArrayCDataQuestCommand>();
        ResultCommandList = new List<CDataQuestCommand>();
    }

    public ushort ProcessNo { get; set; }
    public ushort SequenceNo { get; set; }
    public ushort BlockNo { get; set; }
    public List<CDataQuestProgressWork> WorkList { get; set; }
    public List<MtTypedArrayCDataQuestCommand> CheckCommandList { get; set; }
    public List<CDataQuestCommand> ResultCommandList { get; set; }
    
    public class Serializer : EntitySerializer<CDataQuestProcessState>
    {
        public override void Write(IBuffer buffer, CDataQuestProcessState obj)
        {
            WriteUInt16(buffer, obj.ProcessNo);
            WriteUInt16(buffer, obj.SequenceNo);
            WriteUInt16(buffer, obj.BlockNo);
            WriteEntityList<CDataQuestProgressWork>(buffer, obj.WorkList);
            WriteEntityList<MtTypedArrayCDataQuestCommand>(buffer, obj.CheckCommandList);
            WriteEntityList<CDataQuestCommand>(buffer, obj.ResultCommandList);
        }

        public override CDataQuestProcessState Read(IBuffer buffer)
        {
            CDataQuestProcessState obj = new CDataQuestProcessState();
            obj.ProcessNo = ReadUInt16(buffer);
            obj.SequenceNo = ReadUInt16(buffer);
            obj.BlockNo = ReadUInt16(buffer);
            obj.WorkList = ReadEntityList<CDataQuestProgressWork>(buffer);
            obj.CheckCommandList = ReadEntityList<MtTypedArrayCDataQuestCommand>(buffer);
            obj.ResultCommandList = ReadEntityList<CDataQuestCommand>(buffer);
            return obj;
        }
    }

    // Capcom messed up the types here and serialized a list that contains a list of CDataQuestCommand
    public class MtTypedArrayCDataQuestCommand {
        public List<CDataQuestCommand> ResultCommandList { get; set; }

        public MtTypedArrayCDataQuestCommand()
        {
            ResultCommandList = new List<CDataQuestCommand>();
        }

        public class Serializer : EntitySerializer<MtTypedArrayCDataQuestCommand>
        {
            public override void Write(IBuffer buffer, MtTypedArrayCDataQuestCommand obj)
            {
                WriteEntityList<CDataQuestCommand>(buffer, obj.ResultCommandList);
            }

            public override MtTypedArrayCDataQuestCommand Read(IBuffer buffer)
            {
                MtTypedArrayCDataQuestCommand obj = new MtTypedArrayCDataQuestCommand();
                obj.ResultCommandList = ReadEntityList<CDataQuestCommand>(buffer);
                return obj;
            }
        }
    }
}

