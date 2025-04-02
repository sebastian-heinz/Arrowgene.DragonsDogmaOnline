namespace Arrowgene.Ddon.Shared.Model
{
    public class InstancedEnemy : Enemy
    {
        public InstancedEnemy()
        {
            QuestProcessInfo = new EnemyQuestProcessInfo();
        }

        public InstancedEnemy(Enemy enemy) : base(enemy)
        {
            IsKilled = false;
            IsRequired = true;
            RepopWaitSecond = 60;
            ExpScheme = EnemyExpScheme.Tool;
            QuestProcessInfo = new EnemyQuestProcessInfo();
        }

        public InstancedEnemy(InstancedEnemy enemy) : base (enemy)
        {
            IsKilled = false;
            Index = enemy.Index;
            IsRequired = enemy.IsRequired;
            RepopWaitSecond = enemy.RepopWaitSecond;
            StageLayoutId = enemy.StageLayoutId;
            QuestScheduleId = enemy.QuestScheduleId;
            QuestProcessInfo = enemy.QuestProcessInfo;
            QuestEnemyGroupId = enemy.QuestEnemyGroupId;
            RequiredAreaRank = enemy.RequiredAreaRank;
            ExpScheme = enemy.ExpScheme;
        }

        public StageLayoutId StageLayoutId { get; set; }
        public byte Index { get; set; }
        public bool IsRequired { get; set; }
        public bool IsKilled { get; set; }
        public uint RepopWaitSecond {  get; set; }

        public uint QuestScheduleId { get; set; }
        public uint QuestEnemyGroupId { get; set; }
        public EnemyQuestProcessInfo QuestProcessInfo { get; set; }

        public uint RequiredAreaRank { get; set; }
        public EnemyExpScheme ExpScheme { get; set; }

        public InstancedEnemy(EnemyId enemyId, ushort lv, uint exp, byte index)
        {
            Id = (uint) enemyId;
            EnemyId = (uint) enemyId;
            Lv = lv;
            Experience = exp;
            Index = index;
            EnemyTargetTypesId = 4;
            Scale = 100;
            IsRequired = true;
            HmPresetNo = (ushort)enemyId.GetHmPresetId();
            QuestProcessInfo = new EnemyQuestProcessInfo();
        }

        public class EnemyQuestProcessInfo
        {
            public ushort ProcessNo { get; set; }
            public ushort SequenceNo { get; set; }
            public ushort BlockNo { get; set; }
        }

        public virtual InstancedEnemy CreateNewInstance()
        {
            return new InstancedEnemy(this);
        }

        public InstancedEnemy SetEnemyId(uint enemyId)
        {
            EnemyId = enemyId;
            Id = enemyId;
            return this;
        }

        public InstancedEnemy SetLevel(ushort level)
        {
            Lv = level;
            return this;
        }

        public InstancedEnemy SetIndex(byte index)
        {
            Index = index;
            return this;
        }

        public InstancedEnemy SetEnemyTargetTypesId(byte targetTypeId)
        {
            EnemyTargetTypesId = targetTypeId;
            return this;
        }

        public InstancedEnemy SetEnemyTargetTypesId(TargetTypesId targetTypeId)
        {
            EnemyTargetTypesId = (byte) targetTypeId;
            return this;
        }

        public InstancedEnemy SetScale(ushort scale)
        {
            Scale = scale;
            return this;
        }

        public InstancedEnemy SetIsRequired(bool isRequired)
        {
            IsRequired = isRequired;
            return this;
        }

        public InstancedEnemy SetRepopWaitSecond(uint repopWaitSecond)
        {
            RepopWaitSecond = repopWaitSecond;
            return this;
        }

        public InstancedEnemy SetNamedEnemyParams(NamedParam namedParam)
        {
            NamedEnemyParams = namedParam;
            return this;
        }

        public InstancedEnemy SetIsBoss(bool isBoss)
        {
            IsBossBGM = isBoss;
            IsBossGauge = isBoss;
            return this;
        }

        public InstancedEnemy SetRaidBossId(uint raidBossId)
        {
            RaidBossId = raidBossId;
            return this;
        }

        public InstancedEnemy SetHmPresetNo(ushort hmPresetNo)
        {
            HmPresetNo = hmPresetNo;
            return this;
        }

        public InstancedEnemy SetHmPresetNo(HmPresetId hmPresetId)
        {
            HmPresetNo = (ushort) hmPresetId;
            return this;
        }

        public InstancedEnemy SetStartThinkTblNo(byte startThinkTblNo)
        {
            StartThinkTblNo = startThinkTblNo;
            return this;
        }

        public InstancedEnemy SetRepopNum(byte repopNum)
        {
            RepopNum = repopNum;
            return this;
        }

        public InstancedEnemy SetRepoCount(byte repopCount)
        {
            RepopCount = repopCount;
            return this;
        }

        public InstancedEnemy SetMontageFixNo(byte montageFixNo)
        {
            MontageFixNo = montageFixNo;
            return this;
        }

        public InstancedEnemy SetSetType(byte setType)
        {
            SetType = setType;
            return this;
        }

        public InstancedEnemy SetInfectionType(byte infectionType)
        {
            InfectionType = infectionType;
            return this;
        }

        public InstancedEnemy SetIsManualSet(bool isManualSet)
        {
            IsManualSet = isManualSet;
            return this;
        }

        public InstancedEnemy SetIsAreaBoss(bool isAreaBoss)
        {
            IsAreaBoss = isAreaBoss;
            return this;
        }

        public InstancedEnemy SetBloodOrbs(uint bloodOrbs, bool isBoEnemy = false)
        {
            BloodOrbs = bloodOrbs;
            IsBloodOrbEnemy = isBoEnemy;
            return this;
        }

        public InstancedEnemy SetHighOrbs(uint highOrbs, bool isHoEnemy = false)
        {
            HighOrbs = highOrbs;
            IsHighOrbEnemy = isHoEnemy;
            return this;
        }

        public InstancedEnemy SetSpawnTimeStart(long spawnTime)
        {
            SpawnTimeStart = spawnTime;
            return this;
        }

        public InstancedEnemy SetSpawnTimeEnd(long spawnTime)
        {
            SpawnTimeEnd = spawnTime;
            return this;
        }

        public InstancedEnemy SetSpawnTime(long spawnTimeStart, long spawnTimeEnd)
        {
            SpawnTimeStart = spawnTimeStart;
            SpawnTimeEnd = spawnTimeEnd;
            return this;
        }

        public InstancedEnemy SetSpawnTime((long Start, long End) time)
        {
            return SetSpawnTime(time.Start, time.End);
        }

        public InstancedEnemy SetDropsTable(DropsTable dropsTable)
        {
            DropsTable = dropsTable;
            return this;
        }

        public InstancedEnemy SetPPDrop(uint ppdrop)
        {
            PPDrop = ppdrop;
            return this;
        }

        public InstancedEnemy SetSubgroup(byte subgroup)
        {
            Subgroup = subgroup;
            return this;
        }

        public InstancedEnemy SetRaidPoints(uint points)
        {
            RaidPoints = points;
            return this;
        }

        public InstancedEnemy SetQuestScheduleId(uint questScheduleId)
        {
            QuestScheduleId = questScheduleId;
            return this;
        }

        public InstancedEnemy SetQuestProcessInfo(ushort procNo, ushort seqNo, ushort blockNo)
        {
            QuestProcessInfo.ProcessNo = procNo;
            QuestProcessInfo.SequenceNo = seqNo;
            QuestProcessInfo.BlockNo = blockNo;
            return this;
        }

        public InstancedEnemy SetQuestProcessInfo(EnemyQuestProcessInfo info)
        {
            QuestProcessInfo = info;
            return this;
        }

        public InstancedEnemy SetRequiredAreaRank(uint requiredAreaRank)
        {
            RequiredAreaRank = requiredAreaRank;
            return this;
        }

        public InstancedEnemy SetExpScheme(EnemyExpScheme scheme)
        {
            ExpScheme = scheme;
            return this;
        }

        public virtual InstancedEnemy AddDrop(ItemId itemId, uint minAmount, uint maxAmount, double chance, uint quality = 0, bool isHidden = false)
        {
            var table = DropsTable.Clone().AddDrop(itemId, minAmount, maxAmount, chance, quality, isHidden);
            SetDropsTable(table);
            return this;
        }
    }
}
