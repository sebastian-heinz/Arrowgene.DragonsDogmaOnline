using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model.Quest;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataReleaseAreaInfoSet
{
    public CDataReleaseAreaInfoSet()
    {
    }

    public QuestAreaId AreaId {  get; set; }
    public List<CDataCommonU32> ReleaseList { get; set; }

    public class Serializer : EntitySerializer<CDataReleaseAreaInfoSet>
    {
        public override void Write(IBuffer buffer, CDataReleaseAreaInfoSet obj)
        {
            WriteUInt32(buffer, (uint)obj.AreaId);
            WriteEntityList<CDataCommonU32>(buffer, obj.ReleaseList);
        }

        public override CDataReleaseAreaInfoSet Read(IBuffer buffer)
        {
            CDataReleaseAreaInfoSet obj = new CDataReleaseAreaInfoSet();
            obj.AreaId = (QuestAreaId)ReadUInt32(buffer);
            obj.ReleaseList = ReadEntityList<CDataCommonU32>(buffer);
            return obj;
        }
    }
}
