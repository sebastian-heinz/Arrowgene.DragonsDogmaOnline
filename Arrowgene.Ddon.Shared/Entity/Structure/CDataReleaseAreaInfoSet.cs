using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataReleaseAreaInfoSet
{
    public CDataReleaseAreaInfoSet()
    {
    }

    public uint AreaId {  get; set; }
    public List<CDataCommonU32> ReleaseList { get; set; }

    public class Serializer : EntitySerializer<CDataReleaseAreaInfoSet>
    {
        public override void Write(IBuffer buffer, CDataReleaseAreaInfoSet obj)
        {
            WriteUInt32(buffer, obj.AreaId);
            WriteEntityList<CDataCommonU32>(buffer, obj.ReleaseList);
        }

        public override CDataReleaseAreaInfoSet Read(IBuffer buffer)
        {
            CDataReleaseAreaInfoSet obj = new CDataReleaseAreaInfoSet();
            obj.AreaId = ReadUInt32(buffer);
            obj.ReleaseList = ReadEntityList<CDataCommonU32>(buffer);
            return obj;
        }
    }
}
