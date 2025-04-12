using System.Collections.Generic;
using System.Linq;
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

    public CDataQuestProcessState(CDataQuestProcessState obj)
    {
        ProcessNo = obj.ProcessNo;
        SequenceNo = obj.SequenceNo;
        BlockNo = obj.BlockNo;
        WorkList = obj.WorkList.Select(x => new CDataQuestProgressWork(x)).ToList();
        ResultCommandList = obj.ResultCommandList.Select(x => new CDataQuestCommand(x)).ToList();
        CheckCommandList = obj.CheckCommandList.Select(x => new MtTypedArrayCDataQuestCommand(x)).ToList();
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

        public MtTypedArrayCDataQuestCommand(MtTypedArrayCDataQuestCommand obj)
        {
            ResultCommandList = obj.ResultCommandList.Select(x => new CDataQuestCommand(x)).ToList();
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

    public override string ToString()
    {
        return $"{ProcessNo}.{SequenceNo}.{BlockNo}";
    }
}

