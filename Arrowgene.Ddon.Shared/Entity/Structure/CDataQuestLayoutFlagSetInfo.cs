using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataQuestLayoutFlagSetInfo
{
    public CDataQuestLayoutFlagSetInfo()
    {
        SetInfoList = new List<CDataQuestSetInfo>();
    }

    public uint LayoutFlagNo { get; set; }
    public List<CDataQuestSetInfo> SetInfoList { get; set; }
    
    public class Serializer : EntitySerializer<CDataQuestLayoutFlagSetInfo>
    {
        public override void Write(IBuffer buffer, CDataQuestLayoutFlagSetInfo obj)
        {
            WriteUInt32(buffer, obj.LayoutFlagNo);
            WriteEntityList<CDataQuestSetInfo>(buffer, obj.SetInfoList);
        }

        public override CDataQuestLayoutFlagSetInfo Read(IBuffer buffer)
        {
            CDataQuestLayoutFlagSetInfo obj = new CDataQuestLayoutFlagSetInfo();
            obj.LayoutFlagNo = ReadUInt32(buffer);
            obj.SetInfoList = ReadEntityList<CDataQuestSetInfo>(buffer);
            return obj;
        }
    }
}
