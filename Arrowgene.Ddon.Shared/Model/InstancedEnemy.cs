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
        }

        public StageLayoutId StageLayoutId { get; set; }
        public byte Index { get; set; }
        public bool IsRequired { get; set; }
        public bool IsKilled { get; set; }
        public uint RepopWaitSecond {  get; set; }

        public uint QuestScheduleId { get; set; }
        public uint QuestEnemyGroupId { get; set; }
        public EnemyQuestProcessInfo QuestProcessInfo { get; set; }

        public InstancedEnemy(uint enemyId, ushort lv, uint exp, byte index)
        {
            Id = enemyId;
            EnemyId = enemyId;
            Lv = lv;
            Experience = exp;
            Index = index;
            EnemyTargetTypesId = 4;
            Scale = 100;
            IsRequired = true;
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

        public InstancedEnemy SetBloodOrbs(uint bloodOrbs)
        {
            BloodOrbs = bloodOrbs;
            return this;
        }

        public InstancedEnemy SetHighOrbs(uint highOrbs)
        {
            HighOrbs = highOrbs;
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

        public InstancedEnemy SetDropsTable(DropsTable dropsTable)
        {
            DropsTable = dropsTable;
            return this;
        }

        public InstancedEnemy AddDrop(ItemId itemId, uint minAmount, uint maxAmount, double chance, uint quality = 0, bool isHidden = false)
        {
            DropsTable.Items.Add(new GatheringItem()
            {
                ItemId = itemId,
                ItemNum = minAmount,
                MaxItemNum = maxAmount,
                DropChance = chance,
                IsHidden = isHidden,
                Quality = quality
            });
            return this;
        }

        public InstancedEnemy SetNotifyStrongEnemy(bool notifyStrongEnemy)
        {
            NotifyStrongEnemy = notifyStrongEnemy;
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
    }
}
