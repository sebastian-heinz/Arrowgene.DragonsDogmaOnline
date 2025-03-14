using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.EpitaphRoad;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Asset
{
    public class EpitaphBuff
    {
        public EpitaphBuff()
        {
            Name = string.Empty;
        }

        public EpitaphBuff(EpitaphBuff that)
        {
            this.Name = that.Name;
            this.Type = that.Type;
            this.BuffId = that.BuffId;
            this.BuffEffect = that.BuffEffect;
            this.Increment = that.Increment;
        }

        public string Name;
        public SoulOrdealBuffType Type;
        public uint BuffId;
        public uint BuffEffect;
        public uint Increment;

        public CDataSeasonDungeonBuffEffectReward AsCDataSeasonDungeonBuffEffectReward()
        {
            string incLbl = (Increment == 1) ? "+Lv.1" : "Lv.Max";
            string label = $"{Name} {incLbl}";
            return new CDataSeasonDungeonBuffEffectReward()
            {
                Level = Increment,
                BuffId = BuffId,
                BuffEffect = BuffEffect,
                BuffName = label
            };
        }

        public CDataSeasonDungeonBuffEffectParam AsCDataSeasonDungeonBuffEffectParam()
        {
            string label = (Increment < 4) ? $"{Name} +Lv.{Increment}" : $"{Name} Lv.Max";
            return new CDataSeasonDungeonBuffEffectParam()
            {
                Level = Increment,
                Unk1 = new CDataSeasonDungeonUnk2()
                {
                    BuffId = BuffId,
                    Label = label
                }
            };
        }
    }

    public abstract class EpitaphObject
    {
        public uint EpitaphId { get; set; }
    }

    public class EpitaphDropTable
    {
        public EpitaphDropTable()
        {
            Items = new List<EpitaphItemReward>();
        }

        public uint TableId { get; set; }
        public List<EpitaphItemReward> Items { get; set; }
    }

    public class EpitaphWeeklyReward : EpitaphObject
    {
        public enum Type
        {
            Single,
            Range
        }

        public EpitaphWeeklyReward()
        {
            DropTablesIds = new List<uint>();
        }

        public StageLayoutId StageId { get; set; }
        public uint PosId { get; set; }
        public List<uint> DropTablesIds { get; set; }
    }

    public class EpitaphGatheringPoint : EpitaphObject
    {
        public EpitaphGatheringPoint()
        {
            Door = new EpitaphDoor();
        }

        public StageLayoutId StageId { get; set; }
        public uint PosId { get; set; }
        public EpitaphDoor Door { get; set; }

        public bool Equals(EpitaphGatheringPoint that)
        {
            return this.StageId.Equals(that.StageId) && this.PosId == that.PosId;
        }
    }

    public class EpitaphDoor : EpitaphObject
    {
        public EpitaphDoor()
        {
            GatheringPoints = new List<EpitaphGatheringPoint>();
        }

        public StageLayoutId StageId { get; set; }
        public uint PosId { get; set; }
        public List<EpitaphGatheringPoint> GatheringPoints { get; set; }
    }

    public class EpitaphStatue : EpitaphObject
    {
        public StageLayoutId StageId { get; set; }
        public uint PosId { get; set; }
    }

    public class EpitaphBarrier : EpitaphObject
    {
        public EpitaphBarrier()
        {
            Name = string.Empty;
            UnlockCost = new List<CDataSoulOrdealItem>();
            DependentSectionIds = new HashSet<uint>();
        }

        public string Name { get; set; }
        public StageLayoutId StageId { get; set; }
        public uint PosId { get; set; }
        public NpcId NpcId;
        public List<CDataSoulOrdealItem> UnlockCost { get; set; }
        public HashSet<uint> DependentSectionIds { get; set; }
    }

    public class EpitaphSection : EpitaphObject
    {
        public EpitaphSection()
        {
            Name = string.Empty;
            UnlockCost = new List<CDataSoulOrdealItem>();
            BarrierOmIds = new List<uint>();
            BarrierDependencies = new HashSet<uint>();
            UnlockDependencies = new HashSet<uint>();
        }

        public string Name { get; set; }
        public uint StageId { get; set; }
        public uint StartingPos { get; set; }
        public uint DungeonId { get; set; }
        public List<CDataSoulOrdealItem> UnlockCost { get; set; }
        public List<uint> BarrierOmIds { get; set; }
        public HashSet<uint> BarrierDependencies { get; set; }
        public HashSet<uint> UnlockDependencies { get; set; }

        public CDataSeasonDungeonSection AsCDataSeasonDungeonSection()
        {
            return new CDataSeasonDungeonSection()
            {
                SectionName = Name,
                Unk0 = 0,
                Unk2 = 0,
                Unk1 = 550,
                Unk3 = EpitaphId,
                Unk4 = new CDataSeasonDungeonUnk0() // TODO: This type got renamed incorrect/confused
                {
                    BuffId = 3,
                    Level = 7
                },
                Unk5 = true
            };
        }
    }

    public class EpitaphPath
    {
        public EpitaphPath()
        {
            Name = string.Empty;
            Sections = new List<EpitaphSection>();
            Barriers = new List<EpitaphBarrier>();
            Statues = new List<EpitaphStatue>();
            Chests = new List<EpitaphWeeklyReward>();
            Doors = new List<EpitaphDoor>();
            StageIds = new HashSet<uint>();
            DropTables = new Dictionary<uint, EpitaphDropTable>();
        }

        public string Name { get; set; }
        public uint DungeonId { get; set; }
        public (StageLayoutId StageId, uint PosId) FinalTrialId { get; set; }
        public uint HubStageId { get; set; }
        public uint SoulItemId { get; set; }
        public bool RewardBuffs { get; set; }
        public NpcId NpcId;
        public List<EpitaphSection> Sections { get; set; }
        public List<EpitaphBarrier> Barriers { get; set; }
        public List<EpitaphStatue> Statues { get; set; }
        public List<EpitaphWeeklyReward> Chests { get; set; }
        public List<EpitaphDoor> Doors { get; set; }
        public Dictionary<uint, EpitaphDropTable> DropTables { get; set; }

        public HashSet<uint> StageIds { get; set; }
    }

    public class EpitaphRoadAsset
    {
        public EpitaphRoadAsset()
        {
            Paths = new Dictionary<uint, EpitaphPath>();
            BuffsByType = new Dictionary<SoulOrdealBuffType, List<EpitaphBuff>>();
            BuffsById = new Dictionary<uint, EpitaphBuff>();
            BarriersByOmId = new Dictionary<StageLayoutId, EpitaphBarrier>();
            BarriersByNpcId = new Dictionary<NpcId, EpitaphBarrier>();
            StatuesByOmId = new Dictionary <(StageLayoutId StageId, uint PosId), EpitaphStatue>();
            ChestsByOmId = new Dictionary<(StageLayoutId StageId, uint PosId), EpitaphWeeklyReward>();
            DoorsByOmId = new Dictionary<(StageLayoutId StageId, uint PosId), EpitaphDoor>();
            GatheringPointsByOmId = new Dictionary<(StageLayoutId StageId, uint PosId), EpitaphGatheringPoint>();
            RandomLootByStageId = new Dictionary<uint, List<EpitaphItemReward>>();
            EpitaphObjects = new Dictionary<uint, EpitaphObject>();
        }

        public Dictionary<uint, EpitaphPath> Paths { get; set; }
        public Dictionary<SoulOrdealBuffType, List<EpitaphBuff>> BuffsByType { get; set; }
        public Dictionary<uint, EpitaphBuff> BuffsById { get; set; }
        public Dictionary<StageLayoutId, EpitaphBarrier> BarriersByOmId { get; set; }
        public Dictionary<NpcId, EpitaphBarrier> BarriersByNpcId { get; set; }
        public Dictionary<(StageLayoutId StageId, uint PosId), EpitaphStatue> StatuesByOmId;
        public Dictionary<(StageLayoutId StageId, uint PosId), EpitaphWeeklyReward> ChestsByOmId;
        public Dictionary<(StageLayoutId StageId, uint PosId), EpitaphDoor> DoorsByOmId;
        public Dictionary<(StageLayoutId StageId, uint PosId), EpitaphGatheringPoint> GatheringPointsByOmId;
        public Dictionary<uint, List<EpitaphItemReward>> RandomLootByStageId;

        public Dictionary<uint, EpitaphObject> EpitaphObjects { get; set; }
    }
}
