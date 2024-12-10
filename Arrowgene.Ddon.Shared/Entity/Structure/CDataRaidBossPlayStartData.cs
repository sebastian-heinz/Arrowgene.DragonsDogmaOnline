using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataRaidBossPlayStartData
{
    public CDataRaidBossPlayStartData()
    {
        CommonData = new CDataContentsPlayStartData();
        ClearTimePointBonusList = new List<CDataClearTimePointBonus>();
        RaidBossEnemyParam = new CDataRaidBossEnemyParam();
        RegionBreakRewardList = new List<CDataHasRegionBreakReward>();
    }

    public CDataContentsPlayStartData CommonData { get; set; }
    public List<CDataClearTimePointBonus> ClearTimePointBonusList { get; set; }
    public CDataRaidBossEnemyParam RaidBossEnemyParam { get; set; }
    public List<CDataHasRegionBreakReward> RegionBreakRewardList { get; set; }

    public class Serializer : EntitySerializer<CDataRaidBossPlayStartData>
    {
        public override void Write(IBuffer buffer, CDataRaidBossPlayStartData obj)
        {
            WriteEntity(buffer, obj.CommonData);
            WriteEntityList(buffer, obj.ClearTimePointBonusList);
            WriteEntity(buffer, obj.RaidBossEnemyParam);
            WriteEntityList(buffer, obj.RegionBreakRewardList);
        }

        public override CDataRaidBossPlayStartData Read(IBuffer buffer)
        {
            CDataRaidBossPlayStartData obj = new CDataRaidBossPlayStartData();
            obj.CommonData = ReadEntity<CDataContentsPlayStartData>(buffer);
            obj.ClearTimePointBonusList = ReadEntityList<CDataClearTimePointBonus>(buffer);
            obj.RaidBossEnemyParam = ReadEntity<CDataRaidBossEnemyParam>(buffer);
            obj.RegionBreakRewardList = ReadEntityList<CDataHasRegionBreakReward>(buffer);
            return obj;
        }
    }
}
