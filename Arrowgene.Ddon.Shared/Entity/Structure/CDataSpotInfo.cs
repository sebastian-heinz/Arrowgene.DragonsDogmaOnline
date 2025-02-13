using Arrowgene.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSpotInfo
    {
        public CDataSpotInfo()
        {
            SpotEnemyInfoList = new();
            SpotItemInfoList = new();
        }

        public uint SpotId { get; set; }
        public uint TextIndex { get; set; }
        public uint StageId { get; set; }
        public bool IsRelease { get; set; }
        public bool IsNew { get; set; }
        public List<CDataSpotEnemyInfo> SpotEnemyInfoList { get; set; }
        public List<CDataSpotItemInfo> SpotItemInfoList { get; set; }
        public uint QuickPartyPopularity { get; set; }

        public class Serializer : EntitySerializer<CDataSpotInfo>
        {
            public override void Write(IBuffer buffer, CDataSpotInfo obj)
            {
                WriteUInt32(buffer, obj.SpotId);
                WriteUInt32(buffer, obj.TextIndex);
                WriteUInt32(buffer, obj.StageId);
                WriteBool(buffer, obj.IsRelease);
                WriteBool(buffer, obj.IsNew);
                WriteEntityList(buffer, obj.SpotEnemyInfoList);
                WriteEntityList(buffer, obj.SpotItemInfoList);
                WriteUInt32(buffer, obj.QuickPartyPopularity);
            }

            public override CDataSpotInfo Read(IBuffer buffer)
            {
                CDataSpotInfo obj = new CDataSpotInfo();
                obj.SpotId = ReadUInt32(buffer);
                obj.TextIndex = ReadUInt32(buffer);
                obj.StageId = ReadUInt32(buffer);
                obj.IsRelease = ReadBool(buffer);
                obj.IsNew = ReadBool(buffer);
                obj.SpotEnemyInfoList = ReadEntityList<CDataSpotEnemyInfo>(buffer);
                obj.SpotItemInfoList = ReadEntityList<CDataSpotItemInfo>(buffer);
                obj.QuickPartyPopularity = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
