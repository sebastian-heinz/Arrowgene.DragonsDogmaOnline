namespace Arrowgene.Ddon.Shared.Model
{
    public class NamedParam
    {
        public static readonly NamedParam DEFAULT_NAMED_PARAM = new NamedParam()
        {
            Id = 2298,
            Type = 2,
            HpRate = 100,
            Experience = 100,
            AttackBasePhys = 100,
            AttackWepPhys = 100,
            DefenceBasePhys = 100,
            DefenceWepPhys = 100,
            AttackBaseMagic = 100,
            AttackWepMagic = 100,
            DefenceBaseMagic = 100,
            DefenceWepMagic = 100,
            Power = 100,
            GuardDefenceBase = 100,
            GuardDefenceWep = 100,
            ShrinkEnduranceMain = 100,
            BlowEnduranceMain = 100,
            DownEnduranceMain = 100,
            ShakeEnduranceMain = 100,
            HpSub = 100,
            ShrinkEnduranceSub = 100,
            BlowEnduranceSub = 100,
            OcdEndurance = 100,
            AilmentDamage = 10,
        };

        public uint Id { get; set; }
        public uint Type { get; set; }
        public uint HpRate { get; set; }
        public uint Experience { get; set; }
        public uint AttackBasePhys { get; set; }
        public uint AttackWepPhys { get; set; }
        public uint DefenceBasePhys { get; set; }
        public uint DefenceWepPhys { get; set; }
        public uint AttackBaseMagic { get; set; }
        public uint AttackWepMagic { get; set; }
        public uint DefenceBaseMagic { get; set; }
        public uint DefenceWepMagic { get; set; }
        public uint Power { get; set; }
        public uint GuardDefenceBase { get; set; }
        public uint GuardDefenceWep { get; set; }
        public uint ShrinkEnduranceMain { get; set; }
        public uint BlowEnduranceMain { get; set; }
        public uint DownEnduranceMain { get; set; }
        public uint ShakeEnduranceMain { get; set; }
        public uint HpSub { get; set; }
        public uint ShrinkEnduranceSub { get; set; }
        public uint BlowEnduranceSub { get; set; }
        public uint OcdEndurance { get; set; }
        public uint AilmentDamage { get; set; }
    }
}
