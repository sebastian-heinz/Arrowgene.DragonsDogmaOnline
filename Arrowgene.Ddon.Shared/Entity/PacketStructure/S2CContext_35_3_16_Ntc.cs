using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CContext_35_3_16_Ntc : IPacketStructure
    {
        public S2CContext_35_3_16_Ntc()
        {
            MyPawnCsvData = new List<MyPawnCsv>();
            Req = new C2SPawnJoinPartyMypawnReq();
            CharacterId = 0;
        }

        public S2CContext_35_3_16_Ntc(List<MyPawnCsv> myPawnCsvData, C2SPawnJoinPartyMypawnReq req)
        {
            MyPawnCsvData = myPawnCsvData;
            Req = req;
        }

        public List<MyPawnCsv> MyPawnCsvData { get; set; }
        public C2SPawnJoinPartyMypawnReq Req { get; set; }
        public uint CharacterId { get; set; }

        public PacketId Id => PacketId.S2C_CONTEXT_35_3_16_NTC;

        private class MyPawnMemberIndex
        {
            static int index;
            int id;

            static MyPawnMemberIndex()
            {
                index = 1;
            }

            public MyPawnMemberIndex()
            {
                id = index;
                index++;
            }

            public int ID
            {
                get { return id; }
            }

        }

        public class Serializer : PacketEntitySerializer<S2CContext_35_3_16_Ntc>
        {

            public override void Write(IBuffer buffer, S2CContext_35_3_16_Ntc obj)
            {
                C2SPawnJoinPartyMypawnReq req = obj.Req;
                int n = req.PawnNumber;
                n--;
                MyPawnMemberIndex myPawnMemberIndex = new MyPawnMemberIndex();
                MyPawnCsv myPawnCsvData = obj.MyPawnCsvData[n];
                WriteUInt32(buffer, myPawnCsvData.PawnId);
                WriteInt32(buffer, myPawnMemberIndex.ID);
                //WriteUInt32(buffer, myPawnCsvData.MemberIndex);   //CSV
                WriteUInt32(buffer, myPawnCsvData.PawnId);
                WriteInt32(buffer, myPawnCsvData.StageNo);
                WriteInt32(buffer, myPawnCsvData.StartPosNo);
                WriteByteArray(buffer, obj.Top);
                WriteByte(buffer, myPawnCsvData.Sex);
                WriteByte(buffer, myPawnCsvData.UcColor);
                WriteMtString(buffer, myPawnCsvData.Name);
                WriteUInt32(buffer, 0);
                WriteUInt16(buffer, 15);
                //WriteUInt16(buffer, myPawnCsvData.CountEquip);
                WriteUInt16(buffer, myPawnCsvData.Primary);
                WriteByteArray(buffer, obj.Pad13);
                WriteUInt16(buffer, myPawnCsvData.Secondary);
                WriteByteArray(buffer, obj.Pad13);
                WriteUInt16(buffer, myPawnCsvData.Head);
                WriteByteArray(buffer, obj.Pad13);
                WriteUInt16(buffer, myPawnCsvData.Body);
                WriteByteArray(buffer, obj.Pad13);
                WriteUInt16(buffer, myPawnCsvData.BodyClothing);
                WriteByteArray(buffer, obj.Pad13);
                WriteUInt16(buffer, myPawnCsvData.Arm);
                WriteByteArray(buffer, obj.Pad13);
                WriteUInt16(buffer, myPawnCsvData.Leg);
                WriteByteArray(buffer, obj.Pad13);
                WriteUInt16(buffer, myPawnCsvData.LegWear);
                WriteByteArray(buffer, obj.Pad13);
                WriteUInt16(buffer, myPawnCsvData.OverWear);
                WriteByteArray(buffer, obj.Pad13);
                WriteUInt16(buffer, myPawnCsvData.JewelrySlot1);
                WriteByteArray(buffer, obj.Pad13);
                WriteUInt16(buffer, myPawnCsvData.JewelrySlot2);
                WriteByteArray(buffer, obj.Pad13);
                WriteUInt16(buffer, myPawnCsvData.JewelrySlot3);
                WriteByteArray(buffer, obj.Pad13);
                WriteUInt16(buffer, myPawnCsvData.JewelrySlot4);
                WriteByteArray(buffer, obj.Pad13);
                WriteUInt16(buffer, myPawnCsvData.JewelrySlot5);
                WriteByteArray(buffer, obj.Pad13);
                WriteUInt16(buffer, myPawnCsvData.Lantern);
                WriteByteArray(buffer, obj.Pad13);
                //Visual
                WriteUInt32(buffer, 15);
                //WriteUInt32(buffer, myPawnCsvData.CountVisualEquip);
                WriteUInt16(buffer, myPawnCsvData.VPrimary);
                WriteByteArray(buffer, obj.Pad13);
                WriteUInt16(buffer, myPawnCsvData.VSecondary);
                WriteByteArray(buffer, obj.Pad13);
                WriteUInt16(buffer, myPawnCsvData.VHead);
                WriteByteArray(buffer, obj.Pad13);
                WriteUInt16(buffer, myPawnCsvData.VBody);
                WriteByteArray(buffer, obj.Pad13);
                WriteUInt16(buffer, myPawnCsvData.VBodyClothing);
                WriteByteArray(buffer, obj.Pad13);
                WriteUInt16(buffer, myPawnCsvData.VArm);
                WriteByteArray(buffer, obj.Pad13);
                WriteUInt16(buffer, myPawnCsvData.VLeg);
                WriteByteArray(buffer, obj.Pad13);
                WriteUInt16(buffer, myPawnCsvData.VLegWear);
                WriteByteArray(buffer, obj.Pad13);
                WriteUInt16(buffer, myPawnCsvData.VOverWear);
                WriteByteArray(buffer, obj.Pad13);
                WriteUInt16(buffer, myPawnCsvData.VJewelrySlot1);
                WriteByteArray(buffer, obj.Pad13);
                WriteUInt16(buffer, myPawnCsvData.VJewelrySlot2);
                WriteByteArray(buffer, obj.Pad13);
                WriteUInt16(buffer, myPawnCsvData.VJewelrySlot3);
                WriteByteArray(buffer, obj.Pad13);
                WriteUInt16(buffer, myPawnCsvData.VJewelrySlot4);
                WriteByteArray(buffer, obj.Pad13);
                WriteUInt16(buffer, myPawnCsvData.VJewelrySlot5);
                WriteByteArray(buffer, obj.Pad13);
                WriteUInt16(buffer, myPawnCsvData.VLantern);
                WriteByteArray(buffer, obj.Pad13);
                //JobItem
                WriteUInt32(buffer, 2);
                //WriteUInt32(buffer, myPawnCsvData.CountJobItem);
                WriteUInt16(buffer, myPawnCsvData.JobItem1);
                WriteByte(buffer, 1);
                //WriteByte(buffer, myPawnCsvData.JobItemSlot1);
                WriteUInt16(buffer, myPawnCsvData.JobItem2);
                WriteByte(buffer, 2);
                //WriteByte(buffer, myPawnCsvData.JobItemSlot2);
                WriteBool(buffer, myPawnCsvData.HideEquipHead);
                WriteBool(buffer, myPawnCsvData.HideEquipLantern);
                WriteByte(buffer, myPawnCsvData.HmType);
                WriteByte(buffer, myPawnCsvData.PawnType);
                if (myPawnCsvData.CharacterId == 0)
                { WriteUInt32(buffer, obj.CharacterId); }
                else
                { WriteUInt32(buffer, myPawnCsvData.CharacterId); }
                WriteBool(buffer, myPawnCsvData.SetWaitFlag);
                //Null
                WriteUInt16(buffer, 0);
                WriteByte(buffer, 0);
                //NormalSkill
                WriteByte(buffer, 3);
                //WriteByte(buffer, myPawnCsvData.CountNormalSkill);
                WriteByte(buffer, myPawnCsvData.NormalSkill1);
                WriteByte(buffer, myPawnCsvData.NormalSkill2);
                WriteByte(buffer, myPawnCsvData.NormalSkill3);
                WriteUInt32(buffer, 4);
                //WriteUInt32(buffer, myPawnCsvData.CountCustomSkill);
                WriteByte(buffer, 1);
                //WriteByte(buffer, myPawnCsvData.CustomSkillSlot1);
                WriteUInt32(buffer, myPawnCsvData.CustomSkillId1);
                WriteByte(buffer, myPawnCsvData.CustomSkillLv1);
                WriteByte(buffer, 2);
                //WriteByte(buffer, myPawnCsvData.CustomSkillSlot2);
                WriteUInt32(buffer, myPawnCsvData.CustomSkillId2);
                WriteByte(buffer, myPawnCsvData.CustomSkillLv2);
                WriteByte(buffer, 3);
                //WriteByte(buffer, myPawnCsvData.CustomSkillSlot3);
                WriteUInt32(buffer, myPawnCsvData.CustomSkillId3);
                WriteByte(buffer, myPawnCsvData.CustomSkillLv3);
                WriteByte(buffer, 4);
                //WriteByte(buffer, myPawnCsvData.CustomSkillSlot4);
                WriteUInt32(buffer, myPawnCsvData.CustomSkillId4);
                WriteByte(buffer, myPawnCsvData.CustomSkillLv4);
                //Ability
                WriteUInt32(buffer, 10);
                //WriteUInt32(buffer, myPawnCsvData.CountAbility);
                WriteByte(buffer, 1);
                //WriteByte(buffer, myPawnCsvData.AbilitySlot1);
                WriteUInt32(buffer, myPawnCsvData.AbilityId1);
                WriteByte(buffer, myPawnCsvData.AbilityLv1);
                WriteByte(buffer, 2);
                //WriteByte(buffer, myPawnCsvData.AbilitySlot2);
                WriteUInt32(buffer, myPawnCsvData.AbilityId2);
                WriteByte(buffer, myPawnCsvData.AbilityLv2);
                WriteByte(buffer, 3);
                //WriteByte(buffer, myPawnCsvData.AbilitySlot3);
                WriteUInt32(buffer, myPawnCsvData.AbilityId3);
                WriteByte(buffer, myPawnCsvData.AbilityLv3);
                WriteByte(buffer, 4);
                //WriteByte(buffer, myPawnCsvData.AbilitySlot4);
                WriteUInt32(buffer, myPawnCsvData.AbilityId4);
                WriteByte(buffer, myPawnCsvData.AbilityLv4);
                WriteByte(buffer, 5);
                //WriteByte(buffer, myPawnCsvData.AbilitySlot5);
                WriteUInt32(buffer, myPawnCsvData.AbilityId5);
                WriteByte(buffer, myPawnCsvData.AbilityLv5);
                WriteByte(buffer, 6);
                //WriteByte(buffer, myPawnCsvData.AbilitySlot6);
                WriteUInt32(buffer, myPawnCsvData.AbilityId6);
                WriteByte(buffer, myPawnCsvData.AbilityLv6);
                WriteByte(buffer, 7);
                //WriteByte(buffer, myPawnCsvData.AbilitySlot7);
                WriteUInt32(buffer, myPawnCsvData.AbilityId7);
                WriteByte(buffer, myPawnCsvData.AbilityLv7);
                WriteByte(buffer, 8);
                //WriteByte(buffer, myPawnCsvData.AbilitySlot8);
                WriteUInt32(buffer, myPawnCsvData.AbilityId8);
                WriteByte(buffer, myPawnCsvData.AbilityLv8);
                WriteByte(buffer, 9);
                //WriteByte(buffer, myPawnCsvData.AbilitySlot9);
                WriteUInt32(buffer, myPawnCsvData.AbilityId9);
                WriteByte(buffer, myPawnCsvData.AbilityLv9);
                WriteByte(buffer, 10);
                //WriteByte(buffer, myPawnCsvData.AbilitySlot10);
                WriteUInt32(buffer, myPawnCsvData.AbilityId10);
                WriteByte(buffer, myPawnCsvData.AbilityLv10);
                WriteByteArray(buffer, obj.Pad12);
                WriteByte(buffer, myPawnCsvData.Job);
                WriteByteArray(buffer, obj.JobIdLv);
                WriteByte(buffer, myPawnCsvData.JobLv);
                WriteByteArray(buffer, obj.LvReaction);
                WriteByte(buffer, 9);
                WriteByte(buffer, 1);
                //WriteByte(buffer, myPawnCsvData.MetPartyMembers);
                WriteUInt32(buffer, myPawnCsvData.MetPartyMembersId);
                WriteByte(buffer, 2);
                //WriteByte(buffer, myPawnCsvData.QuestClear);
                WriteUInt32(buffer, myPawnCsvData.QuestClearId);
                WriteByte(buffer, 10);
                //WriteByte(buffer, myPawnCsvData.SpecialSkillInspirationMoment);
                WriteUInt32(buffer, myPawnCsvData.SpecialSkillInspirationMomentId);
                WriteByte(buffer, 4);
                //WriteByte(buffer, myPawnCsvData.LevelUp);
                WriteUInt32(buffer, myPawnCsvData.LevelUpId);
                WriteByte(buffer, 11);
                //WriteByte(buffer, myPawnCsvData.SpecialSkillUse);
                WriteUInt32(buffer, myPawnCsvData.SpecialSkillUseId);
                WriteByte(buffer, 6);
                //WriteByte(buffer, myPawnCsvData.PlayerDeath);
                WriteUInt32(buffer, myPawnCsvData.PlayerDeathId);
                WriteByte(buffer, 7);
                //WriteByte(buffer, myPawnCsvData.WaitingOnLobby);
                WriteUInt32(buffer, myPawnCsvData.WaitingOnLobbyId);
                WriteByte(buffer, 8);
                //WriteByte(buffer, myPawnCsvData.WaitingOnAdventure);
                WriteUInt32(buffer, myPawnCsvData.WaitingOnAdventureId);
                WriteByte(buffer, 9);
                //WriteByte(buffer, myPawnCsvData.EndOfCombat);
                WriteUInt32(buffer, myPawnCsvData.EndOfCombatId);
                WriteByteArray(buffer, obj.ReactionSpSkill);
                //SpSkill
                WriteByte(buffer, 3);
                //WriteByte(buffer, myPawnCsvData.CountSpSkill);
                WriteByte(buffer, myPawnCsvData.SpSkillSlot1Id);
                WriteByte(buffer, myPawnCsvData.SpSkillSlot1Lv);
                WriteByte(buffer, myPawnCsvData.SpSkillSlot2Id);
                WriteByte(buffer, myPawnCsvData.SpSkillSlot2Lv);
                WriteByte(buffer, myPawnCsvData.SpSkillSlot3Id);
                WriteByte(buffer, myPawnCsvData.SpSkillSlot3Lv);
                WriteByteArray(buffer, obj.SpSkillEdit);
                //Character
                WriteByte(buffer, myPawnCsvData.Sex);
                WriteByte(buffer, myPawnCsvData.Voice);
                WriteUInt16(buffer, myPawnCsvData.VoicePitch);
                WriteByte(buffer, myPawnCsvData.Personality);
                WriteByte(buffer, myPawnCsvData.SpeechFreq);
                WriteByte(buffer, myPawnCsvData.BodyType);
                WriteByte(buffer, myPawnCsvData.Hair);
                WriteByte(buffer, myPawnCsvData.Beard);
                WriteByte(buffer, myPawnCsvData.Makeup);
                WriteByte(buffer, myPawnCsvData.Scar);
                WriteByte(buffer, myPawnCsvData.EyePresetNo);
                WriteByte(buffer, myPawnCsvData.NosePresetNo);
                WriteByte(buffer, myPawnCsvData.MouthPresetNo);
                WriteByte(buffer, myPawnCsvData.EyebrowTexNo);
                WriteByte(buffer, myPawnCsvData.ColorSkin);
                WriteByte(buffer, myPawnCsvData.ColorHair);
                WriteByte(buffer, myPawnCsvData.ColorBeard);
                WriteByte(buffer, myPawnCsvData.ColorEyebrow);
                WriteByte(buffer, myPawnCsvData.ColorREye);
                WriteByte(buffer, myPawnCsvData.ColorLEye);
                WriteByte(buffer, myPawnCsvData.ColorMakeup);
                WriteUInt16(buffer, myPawnCsvData.Sokutobu);
                WriteUInt16(buffer, myPawnCsvData.Hitai);
                WriteUInt16(buffer, myPawnCsvData.MimiJyouge);
                WriteUInt16(buffer, myPawnCsvData.Kannkaku);
                WriteUInt16(buffer, myPawnCsvData.MabisasiJyouge);
                WriteUInt16(buffer, myPawnCsvData.HanakuchiJyouge);
                WriteUInt16(buffer, myPawnCsvData.AgoSakiHaba);
                WriteUInt16(buffer, myPawnCsvData.AgoZengo);
                WriteUInt16(buffer, myPawnCsvData.AgoSakiJyouge);
                WriteUInt16(buffer, myPawnCsvData.HitomiOokisa);
                WriteUInt16(buffer, myPawnCsvData.MeOokisa);
                WriteUInt16(buffer, myPawnCsvData.MeKaiten);
                WriteUInt16(buffer, myPawnCsvData.MayuKaiten);
                WriteUInt16(buffer, myPawnCsvData.MimiOokisa);
                WriteUInt16(buffer, myPawnCsvData.MimiMuki);
                WriteUInt16(buffer, myPawnCsvData.ElfMimi);
                WriteUInt16(buffer, myPawnCsvData.MikenTakasa);
                WriteUInt16(buffer, myPawnCsvData.MikenHaba);
                WriteUInt16(buffer, myPawnCsvData.HohoboneRyou);
                WriteUInt16(buffer, myPawnCsvData.HohoboneJyouge);
                WriteUInt16(buffer, myPawnCsvData.Hohoniku);
                WriteUInt16(buffer, myPawnCsvData.ErahoneJyouge);
                WriteUInt16(buffer, myPawnCsvData.ErahoneHaba);
                WriteUInt16(buffer, myPawnCsvData.HanaJyouge);
                WriteUInt16(buffer, myPawnCsvData.HanaHaba);
                WriteUInt16(buffer, myPawnCsvData.HanaTakasa);
                WriteUInt16(buffer, myPawnCsvData.HanaKakudo);
                WriteUInt16(buffer, myPawnCsvData.KuchiHaba);
                WriteUInt16(buffer, myPawnCsvData.KuchiAtsusa);
                WriteUInt16(buffer, myPawnCsvData.EyebrowUVOffsetX);
                WriteUInt16(buffer, myPawnCsvData.EyebrowUVOffsetY);
                WriteUInt16(buffer, myPawnCsvData.Wrinkle);
                WriteUInt16(buffer, myPawnCsvData.WrinkleAlbedoBlendRate);
                WriteUInt16(buffer, myPawnCsvData.WrinkleDetailNormalPower);
                WriteUInt16(buffer, myPawnCsvData.MuscleAlbedoBlendRate);
                WriteUInt16(buffer, myPawnCsvData.MuscleDetailNormalPower);
                WriteUInt16(buffer, myPawnCsvData.Height);
                WriteUInt16(buffer, myPawnCsvData.HeadSize);
                WriteUInt16(buffer, myPawnCsvData.NeckOffset);
                WriteUInt16(buffer, myPawnCsvData.NeckScale);
                WriteUInt16(buffer, myPawnCsvData.UpperBodyScaleX);
                WriteUInt16(buffer, myPawnCsvData.BellySize);
                WriteUInt16(buffer, myPawnCsvData.TeatScale);
                WriteUInt16(buffer, myPawnCsvData.TekubiSize);
                WriteUInt16(buffer, myPawnCsvData.KoshiOffset);
                WriteUInt16(buffer, myPawnCsvData.KoshiSize);
                WriteUInt16(buffer, myPawnCsvData.AnkleOffset);
                WriteUInt16(buffer, myPawnCsvData.Fat);
                WriteUInt16(buffer, myPawnCsvData.Muscle);
                WriteUInt16(buffer, myPawnCsvData.MotionFilter);
                WriteByteArray(buffer, obj.Base);
            }

            public override S2CContext_35_3_16_Ntc Read(IBuffer buffer)
            {
                S2CContext_35_3_16_Ntc obj = new S2CContext_35_3_16_Ntc();
                MyPawnCsv myPawnCsvData = new MyPawnCsv();

                return obj;
            }

        }




        private readonly byte[] Top =
        {
            0x40, 0x7C, 0x1A, 0x6F, 0x40, 0x0, 0x0, 0x0, 0x46, 0x40, 0x39, 0xA5, 0x40, 0x86, 0x1F, 0xFB,
            0x0, 0x0, 0x0, 0x0, 0xBF, 0xB0, 0xA3, 0x6E
        };
        private readonly byte[] Pad12 = { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
        private readonly byte[] Pad13 = { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
        private readonly byte[] JobIdLv =
        {
            0x44, 0x3E, 0x0, 0x0, 0x44, 0x3E, 0x0, 0x0, 0x44, 0x3E, 0x0, 0x0, 0x43, 0xE1, 0x0, 0x0,
            0x43, 0xE1, 0x0, 0x0, 0x47, 0x43, 0x50, 0x0, 0x0
        };
        private readonly byte[] LvReaction =
        {
            0x0, 0x0, 0x0, 0x0, 0x7, 0x6E, 0x9, 0x4B, 0x0, 0x0, 0x0, 0xD8, 0x0, 0x0, 0x0, 0x57,
            0x0, 0x0, 0x1, 0x44, 0x0, 0x0, 0x0, 0xC6, 0x0, 0x0, 0x0, 0x1E, 0x0, 0x0, 0x0, 0x32,
            0x0, 0x0, 0x0, 0x32, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x9, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x5, 0xF6, 0x90, 0x0, 0x0, 0x2, 0x9E, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x10, 0x0, 0x0, 0x0, 0x10, 0x0, 0x0, 0x0, 0xF, 0x0, 0x0, 0x0, 0x10,
            0xFF, 0xFF, 0xFF, 0xFF, 0x0, 0x0, 0x0, 0x0, 0x0, 0x5, 0x1, 0x0, 0x33, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x3A, 0x49, 0xE0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x3, 0x8A, 0x40, 0xB, 0x0, 0x1,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x3, 0x0, 0x6, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x1A, 0x71, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x5, 0x78, 0x6, 0x0, 0x63, 0x0, 0x0, 0x0, 0x0, 0x7, 0x6E, 0x9, 0x4B, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x5, 0xF6, 0x90, 0xA, 0x0, 0x5D, 0x0, 0x0, 0x0, 0x0, 0x5, 0xD7, 0x6D,
            0x38, 0x0, 0x0, 0x0, 0x0, 0x0, 0x7, 0x86, 0x90, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x3F, 0x80, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0xC2, 0xC8, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0xC2, 0xC8, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0
        };
        private readonly byte[] ReactionSpSkill =
        {
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0
        };
        private readonly byte[] SpSkillEdit =
        {
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0
        };
        private readonly byte[] Base = { 0x0, 0x2, 0xC5, 0x0, 0x0, 0x3, 0x2E, 0x0, 0x0, 0x2, 0xC9, 0x0, 0x0, 0x2 };

    }
}
