namespace Arrowgene.Ddon.Shared.Model
{
    public class MyPawnCsv
    {
        public uint PawnId { get; set; }
        public uint MemberIndex { get; set; }
        public int StageNo { get; set; }
        public int StartPosNo { get; set; }
        public double PosX { get; set; }
        public float PosY { get; set; }
        public double PosZ { get; set; }
        public float AngleY { get; set; }
        public byte UcColor { get; set; }
        public string Name { get; set; }
        //Equip
        public ushort CountEquip { get; set; }
        public ushort Primary { get; set; }
        public ushort Secondary { get; set; }
        public ushort Head { get; set; }
        public ushort Body { get; set; }
        public ushort BodyClothing { get; set; }
        public ushort Arm { get; set; }
        public ushort Leg { get; set; }
        public ushort LegWear { get; set; }
        public ushort OverWear { get; set; }
        //Jewelry
        public ushort JewelrySlot1 { get; set; }
        public ushort JewelrySlot2 { get; set; }
        public ushort JewelrySlot3 { get; set; }
        public ushort JewelrySlot4 { get; set; }
        public ushort JewelrySlot5 { get; set; }
        //Lantern
        public ushort Lantern { get; set; }
        //VisualEquip
        public uint CountVisualEquip { get; set; }
        public ushort VPrimary { get; set; }
        public ushort VSecondary { get; set; }
        public ushort VHead { get; set; }
        public ushort VBody { get; set; }
        public ushort VBodyClothing { get; set; }
        public ushort VArm { get; set; }
        public ushort VLeg { get; set; }
        public ushort VLegWear { get; set; }
        public ushort VOverWear { get; set; }
        //Jewelry
        public ushort VJewelrySlot1 { get; set; }
        public ushort VJewelrySlot2 { get; set; }
        public ushort VJewelrySlot3 { get; set; }
        public ushort VJewelrySlot4 { get; set; }
        public ushort VJewelrySlot5 { get; set; }
        //Lantern
        public ushort VLantern { get; set; }

        //JobItems
        public uint CountJobItem { get; set; }
        public ushort JobItem1 { get; set; }
        public byte JobItemSlot1 { get; set; }
        public ushort JobItem2 { get; set; }
        public byte JobItemSlot2 { get; set; }
        public bool HideEquipHead { get; set; }
        public bool HideEquipLantern { get; set; }

        public byte HmType { get; set; }
        public byte PawnType { get; set; }
        public uint CharacterId { get; set; }
        public bool SetWaitFlag { get; set; }
        //NormalSkills
        public byte CountNormalSkill { get; set; }
        public byte NormalSkill1 { get; set; }
        public byte NormalSkill2 { get; set; }
        public byte NormalSkill3 { get; set; }
        //CustomSkills
        public uint CountCustomSkill { get; set; }
        public byte CustomSkillSlot1 { get; set; }
        public uint CustomSkillId1 { get; set; }
        public byte CustomSkillLv1 { get; set; }
        public byte CustomSkillSlot2 { get; set; }
        public uint CustomSkillId2 { get; set; }
        public byte CustomSkillLv2 { get; set; }
        public byte CustomSkillSlot3 { get; set; }
        public uint CustomSkillId3 { get; set; }
        public byte CustomSkillLv3 { get; set; }
        public byte CustomSkillSlot4 { get; set; }
        public uint CustomSkillId4 { get; set; }
        public byte CustomSkillLv4 { get; set; }
        //Abilities
        public uint CountAbility { get; set; }
        public byte AbilityJob1 { get; set; }
        public uint AbilityId1 { get; set; }
        public byte AbilityLv1 { get; set; }

        public byte AbilityJob2 { get; set; }
        public uint AbilityId2 { get; set; }
        public byte AbilityLv2 { get; set; }

        public byte AbilityJob3 { get; set; }
        public uint AbilityId3 { get; set; }
        public byte AbilityLv3 { get; set; }

        public byte AbilityJob4 { get; set; }
        public uint AbilityId4 { get; set; }
        public byte AbilityLv4 { get; set; }

        public byte AbilityJob5 { get; set; }
        public uint AbilityId5 { get; set; }
        public byte AbilityLv5 { get; set; }

        public byte AbilityJob6 { get; set; }
        public uint AbilityId6 { get; set; }
        public byte AbilityLv6 { get; set; }

        public byte AbilityJob7 { get; set; }
        public uint AbilityId7 { get; set; }
        public byte AbilityLv7 { get; set; }

        public byte AbilityJob8 { get; set; }
        public uint AbilityId8 { get; set; }
        public byte AbilityLv8 { get; set; }

        public byte AbilityJob9 { get; set; }
        public uint AbilityId9 { get; set; }
        public byte AbilityLv9 { get; set; }

        public byte AbilityJob10 { get; set; }
        public uint AbilityId10 { get; set; }
        public byte AbilityLv10 { get; set; }
        //Vocation
        public JobId Job { get; set; }
        public byte JobLv { get; set; }
        //PawnReaction
        public byte MetPartyMembers { get; set; }
        public uint MetPartyMembersId { get; set; }

        public byte QuestClear { get; set; }
        public uint QuestClearId { get; set; }

        public byte SpecialSkillInspirationMoment { get; set; }
        public uint SpecialSkillInspirationMomentId { get; set; }

        public byte LevelUp { get; set; }
        public uint LevelUpId { get; set; }

        public byte SpecialSkillUse { get; set; }
        public uint SpecialSkillUseId { get; set; }

        public byte PlayerDeath { get; set; }
        public uint PlayerDeathId { get; set; }

        public byte WaitingOnLobby { get; set; }
        public uint WaitingOnLobbyId { get; set; }

        public byte WaitingOnAdventure { get; set; }
        public uint WaitingOnAdventureId { get; set; }

        public byte EndOfCombat { get; set; }
        public uint EndOfCombatId { get; set; }
        //SpecialSkill
        public byte CountSpSkill { get; set; }

        public byte SpSkillSlot1Id { get; set; }
        public byte SpSkillSlot1Lv { get; set; }

        public byte SpSkillSlot2Id { get; set; }
        public byte SpSkillSlot2Lv { get; set; }

        public byte SpSkillSlot3Id { get; set; }
        public byte SpSkillSlot3Lv { get; set; }
        //VisualEdit
        public byte Sex { get; set; }
        public byte Voice { get; set; }
        public ushort VoicePitch { get; set; }
        public byte Personality { get; set; }
        public byte SpeechFreq { get; set; }
        public byte BodyType { get; set; }
        public byte Hair { get; set; }
        public byte Beard { get; set; }
        public byte Makeup { get; set; }
        public byte Scar { get; set; }
        public byte EyePresetNo { get; set; }
        public byte NosePresetNo { get; set; }
        public byte MouthPresetNo { get; set; }
        public byte EyebrowTexNo { get; set; }
        public byte ColorSkin { get; set; }
        public byte ColorHair { get; set; }
        public byte ColorBeard { get; set; }
        public byte ColorEyebrow { get; set; }
        public byte ColorREye { get; set; }
        public byte ColorLEye { get; set; }
        public byte ColorMakeup { get; set; }
        public ushort Sokutobu { get; set; }
        public ushort Hitai { get; set; }
        public ushort MimiJyouge { get; set; }
        public ushort Kannkaku { get; set; }
        public ushort MabisasiJyouge { get; set; }
        public ushort HanakuchiJyouge { get; set; }
        public ushort AgoSakiHaba { get; set; }
        public ushort AgoZengo { get; set; }
        public ushort AgoSakiJyouge { get; set; }
        public ushort HitomiOokisa { get; set; }
        public ushort MeOokisa { get; set; }
        public ushort MeKaiten { get; set; }
        public ushort MayuKaiten { get; set; }
        public ushort MimiOokisa { get; set; }
        public ushort MimiMuki { get; set; }
        public ushort ElfMimi { get; set; }
        public ushort MikenTakasa { get; set; }
        public ushort MikenHaba { get; set; }
        public ushort HohoboneRyou { get; set; }
        public ushort HohoboneJyouge { get; set; }
        public ushort Hohoniku { get; set; }
        public ushort ErahoneJyouge { get; set; }
        public ushort ErahoneHaba { get; set; }
        public ushort HanaJyouge { get; set; }
        public ushort HanaHaba { get; set; }
        public ushort HanaTakasa { get; set; }
        public ushort HanaKakudo { get; set; }
        public ushort KuchiHaba { get; set; }
        public ushort KuchiAtsusa { get; set; }
        public ushort EyebrowUVOffsetX { get; set; }
        public ushort EyebrowUVOffsetY { get; set; }
        public ushort Wrinkle { get; set; }
        public ushort WrinkleAlbedoBlendRate { get; set; }
        public ushort WrinkleDetailNormalPower { get; set; }
        public ushort MuscleAlbedoBlendRate { get; set; }
        public ushort MuscleDetailNormalPower { get; set; }
        public ushort Height { get; set; }
        public ushort HeadSize { get; set; }
        public ushort NeckOffset { get; set; }
        public ushort NeckScale { get; set; }
        public ushort UpperBodyScaleX { get; set; }
        public ushort BellySize { get; set; }
        public ushort TeatScale { get; set; }
        public ushort TekubiSize { get; set; }
        public ushort KoshiOffset { get; set; }
        public ushort KoshiSize { get; set; }
        public ushort AnkleOffset { get; set; }
        public ushort Fat { get; set; }
        public ushort Muscle { get; set; }
        public ushort MotionFilter { get; set; }
    }
}
