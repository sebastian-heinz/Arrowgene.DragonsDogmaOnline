using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnGetMypawnDataRes : IPacketStructure
    {
        public S2CPawnGetMypawnDataRes()
        {
            MyPawnCsvData = new List<MyPawnCsv>();
            Req = new C2SPawnGetMypawnDataReq();
        }

        public S2CPawnGetMypawnDataRes(List<MyPawnCsv> myPawnCsvData, C2SPawnGetMypawnDataReq req)
        {
            MyPawnCsvData = myPawnCsvData;
            Req = req;
        }

        public List<MyPawnCsv> MyPawnCsvData { get; set; }
        public C2SPawnGetMypawnDataReq Req { get; set; }
        public PacketId Id => PacketId.S2C_PAWN_GET_MYPAWN_DATA_RES;


        public class Serializer : PacketEntitySerializer<S2CPawnGetMypawnDataRes>
        {

            public override void Write(IBuffer buffer, S2CPawnGetMypawnDataRes obj)
            {
                C2SPawnGetMypawnDataReq req = obj.Req;
                int n = req.PawnNumber;
                n--;
                MyPawnCsv myPawnCsvData = obj.MyPawnCsvData[n];
                WriteByteArray(buffer, obj.Pad8);
                WriteUInt32(buffer, myPawnCsvData.PawnId);
                WriteByteArray(buffer, obj.Pad4);
                WriteMtString(buffer, myPawnCsvData.Name);
                //Edit
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
                //MaxHp
                WriteByte(buffer, 0);
                WriteUInt32(buffer, 767);
                //MaxStamina
                WriteUInt32(buffer, 451);
                //Job
                WriteByte(buffer, myPawnCsvData.Job);
                //Count(JobArray - 11)
                WriteUInt32(buffer, 1);
                //WriteByteArray(buffer, obj.JobArray);
                WriteByte(buffer, myPawnCsvData.Job);
                WriteByteArray(buffer, obj.JobDummy1);
                WriteByte(buffer, myPawnCsvData.JobLv);
                WriteByteArray(buffer, obj.JobDummy2);
                //Equip
                WriteUInt32(buffer, 1);
                //Count(UInt32)(Dummy)
                WriteUInt32(buffer, 15);
                WriteUInt32(buffer, myPawnCsvData.Primary);
                //DummyData
                WriteUInt32(buffer, 65537);
                WriteByteArray(buffer, obj.Pad14);
                WriteUInt32(buffer, myPawnCsvData.Secondary);
                WriteUInt32(buffer, 65538);
                WriteByteArray(buffer, obj.Pad14);
                WriteUInt32(buffer, myPawnCsvData.Head);
                WriteUInt32(buffer, 65539);
                WriteByteArray(buffer, obj.Pad14);
                WriteUInt32(buffer, myPawnCsvData.Body);
                WriteUInt32(buffer, 65540);
                WriteByteArray(buffer, obj.Pad14);
                WriteUInt32(buffer, myPawnCsvData.BodyClothing);
                WriteUInt32(buffer, 65541);
                WriteByteArray(buffer, obj.Pad14);
                WriteUInt32(buffer, myPawnCsvData.Arm);
                WriteUInt32(buffer, 65542);
                WriteByteArray(buffer, obj.Pad14);
                WriteUInt32(buffer, myPawnCsvData.Leg);
                WriteUInt32(buffer, 65543);
                WriteByteArray(buffer, obj.Pad14);
                WriteUInt32(buffer, myPawnCsvData.LegWear);
                WriteUInt32(buffer, 65544);
                WriteByteArray(buffer, obj.Pad14);
                WriteUInt32(buffer, myPawnCsvData.OverWear);
                WriteUInt32(buffer, 65545);
                WriteByteArray(buffer, obj.Pad14);
                WriteUInt32(buffer, myPawnCsvData.JewelrySlot1);
                WriteUInt32(buffer, 65546);
                WriteByteArray(buffer, obj.Pad14);
                WriteUInt32(buffer, myPawnCsvData.JewelrySlot2);
                WriteUInt32(buffer, 65547);
                WriteByteArray(buffer, obj.Pad14);
                WriteUInt32(buffer, myPawnCsvData.JewelrySlot3);
                WriteUInt32(buffer, 65548);
                WriteByteArray(buffer, obj.Pad14);
                WriteUInt32(buffer, myPawnCsvData.JewelrySlot4);
                WriteUInt32(buffer, 65549);
                WriteByteArray(buffer, obj.Pad14);
                WriteUInt32(buffer, myPawnCsvData.JewelrySlot5);
                WriteUInt32(buffer, 65550);
                WriteByteArray(buffer, obj.Pad14);
                WriteUInt32(buffer, myPawnCsvData.Lantern);
                WriteUInt32(buffer, 65551);
                WriteByteArray(buffer, obj.Pad14);
                //Visual
                WriteUInt32(buffer, 1);
                //Count(Dummy)
                WriteUInt32(buffer, 15);
                WriteUInt32(buffer, myPawnCsvData.VPrimary);
                WriteUInt32(buffer, 131073);
                WriteByteArray(buffer, obj.Pad14);
                WriteUInt32(buffer, myPawnCsvData.VSecondary);
                WriteUInt32(buffer, 131074);
                WriteByteArray(buffer, obj.Pad14);
                WriteUInt32(buffer, myPawnCsvData.VHead);
                WriteUInt32(buffer, 131075);
                WriteByteArray(buffer, obj.Pad14);
                WriteUInt32(buffer, myPawnCsvData.VBody);
                WriteUInt32(buffer, 131076);
                WriteByteArray(buffer, obj.Pad14);
                WriteUInt32(buffer, myPawnCsvData.VBodyClothing);
                WriteUInt32(buffer, 131077);
                WriteByteArray(buffer, obj.Pad14);
                WriteUInt32(buffer, myPawnCsvData.VArm);
                WriteUInt32(buffer, 131078);
                WriteByteArray(buffer, obj.Pad14);
                WriteUInt32(buffer, myPawnCsvData.VLeg);
                WriteUInt32(buffer, 131079);
                WriteByteArray(buffer, obj.Pad14);
                WriteUInt32(buffer, myPawnCsvData.VLegWear);
                WriteUInt32(buffer, 131080);
                WriteByteArray(buffer, obj.Pad14);
                WriteUInt32(buffer, myPawnCsvData.VOverWear);
                WriteUInt32(buffer, 131081);
                WriteByteArray(buffer, obj.Pad14);
                WriteUInt32(buffer, myPawnCsvData.VJewelrySlot1);
                WriteUInt32(buffer, 131082);
                WriteByteArray(buffer, obj.Pad14);
                WriteUInt32(buffer, myPawnCsvData.VJewelrySlot2);
                WriteUInt32(buffer, 131083);
                WriteByteArray(buffer, obj.Pad14);
                WriteUInt32(buffer, myPawnCsvData.VJewelrySlot3);
                WriteUInt32(buffer, 131084);
                WriteByteArray(buffer, obj.Pad14);
                WriteUInt32(buffer, myPawnCsvData.VJewelrySlot4);
                WriteUInt32(buffer, 131085);
                WriteByteArray(buffer, obj.Pad14);
                WriteUInt32(buffer, myPawnCsvData.VJewelrySlot5);
                WriteUInt32(buffer, 131086);
                WriteByteArray(buffer, obj.Pad14);
                WriteUInt32(buffer, myPawnCsvData.VLantern);
                WriteUInt32(buffer, 131087);
                WriteByteArray(buffer, obj.Pad14);
                //JobItem
                WriteUInt32(buffer, 2);
                //WriteUInt32(buffer, myPawnCsvData.CountJobItem);  //CSV
                WriteUInt32(buffer, myPawnCsvData.JobItem1);
                WriteByte(buffer, 1);
                //WriteByte(buffer, myPawnCsvData.JobItemSlot1);    //CSV
                WriteUInt32(buffer, myPawnCsvData.JobItem2);
                WriteByte(buffer, 2);
                //WriteByte(buffer, myPawnCsvData.JobItemSlot2);    //CSV
                //RingSlot
                WriteByte(buffer, 5);
                //
                WriteUInt32(buffer, 0);
                WriteUInt32(buffer, 391);   //CraftExp
                WriteUInt32(buffer, 4);     //CraftRank?
                WriteUInt32(buffer, 8);     //CraftRankLimit?
                WriteUInt32(buffer, 0);     //CraftPoint?
                WriteUInt32(buffer, 10);
                WriteByte(buffer, 1);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 2);
                WriteUInt32(buffer, 3);
                WriteByte(buffer, 3);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 4);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 5);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 6);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 7);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 8);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 9);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 10);
                WriteUInt32(buffer, 0);
                WriteUInt32(buffer, 9);       //PawnReaction
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

                WriteBool(buffer, myPawnCsvData.HideEquipHead);
                WriteBool(buffer, myPawnCsvData.HideEquipLantern);

                WriteByte(buffer, 5);       //AdventureCount?
                WriteByte(buffer, 10);      //CraftCount?
                WriteByte(buffer, 5);       //MaxAdventureCount?
                WriteByte(buffer, 10);      //MaxCraftCount?

                WriteUInt32(buffer, 3);
                //WriteUInt32(buffer, myPawnCsvData.CountNormalSkill);      //NormalSkill
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
                WriteUInt32(buffer, 10);
                //WriteUInt32(buffer, myPawnCsvData.CountAbility);        //Ability
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
                WriteUInt32(buffer, 15);        //AbilityCostMax?
                WriteByteArray(buffer, obj.OrbDummy);      //Orb(Dummy)
                WriteByteArray(buffer, obj.OrbDummy2);
                WriteByte(buffer, 3);
                //WriteByte(buffer, myPawnCsvData.CountSpSkill);      //SpSkill-CSV
                WriteByte(buffer, myPawnCsvData.SpSkillSlot1Id);
                WriteByte(buffer, myPawnCsvData.SpSkillSlot1Lv);
                WriteByte(buffer, myPawnCsvData.SpSkillSlot2Id);
                WriteByte(buffer, myPawnCsvData.SpSkillSlot2Lv);
                WriteByte(buffer, myPawnCsvData.SpSkillSlot3Id);
                WriteByte(buffer, myPawnCsvData.SpSkillSlot3Lv);
                WriteByteArray(buffer, obj.BaseDataArray);
            }

            public override S2CPawnGetMypawnDataRes Read(IBuffer buffer)
            {
                S2CPawnGetMypawnDataRes obj = new S2CPawnGetMypawnDataRes();
                MyPawnCsv myPawnCsvData = new MyPawnCsv();

                return obj;
            }

        }


        private readonly byte[] Pad4 = { 0x0, 0x0, 0x0, 0x0 };
        private readonly byte[] Pad5 = { 0x0, 0x0, 0x0, 0x0, 0x0 };
        private readonly byte[] Pad8 = { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
        //Dummy - JobArray
        private readonly byte[] JobArray =
        {
            0x1, 0x7, 0x6E, 0x9, 0x4B, 0x0, 0x5, 0xF6, 0x90, 0x0, 0x0, 0x0, 0x63, 0x0, 0xD8, 0x0,
            0x57, 0x1, 0x44, 0x0, 0xC6, 0x0, 0x1E, 0x0, 0x32, 0x0, 0x32, 0x0, 0x0, 0x0, 0x9, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,

            0x2, 0x7, 0x6E, 0x9, 0x4B, 0x0, 0x5, 0xF6, 0x90, 0x0, 0x0, 0x0, 0x63, 0x0, 0xD8, 0x0,
            0x57, 0x1, 0x44, 0x0, 0xC6, 0x0, 0x1E, 0x0, 0x32, 0x0, 0x32, 0x0, 0x0, 0x0, 0x9, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,

            0x3, 0x7, 0x6E, 0x9, 0x4B, 0x0, 0x5, 0xF6, 0x90, 0x0, 0x0, 0x0, 0x63, 0x0, 0xD8, 0x0,
            0x57, 0x1, 0x44, 0x0, 0xC6, 0x0, 0x1E, 0x0, 0x32, 0x0, 0x32, 0x0, 0x0, 0x0, 0x9, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,

            0x4, 0x7, 0x6E, 0x9, 0x4B, 0x0, 0x5, 0xF6, 0x90, 0x0, 0x0, 0x0, 0x63, 0x0, 0xD8, 0x0,
            0x57, 0x1, 0x44, 0x0, 0xC6, 0x0, 0x1E, 0x0, 0x32, 0x0, 0x32, 0x0, 0x0, 0x0, 0x9, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,

            0x5, 0x7, 0x6E, 0x9, 0x4B, 0x0, 0x5, 0xF6, 0x90, 0x0, 0x0, 0x0, 0x63, 0x0, 0xD8, 0x0,
            0x57, 0x1, 0x44, 0x0, 0xC6, 0x0, 0x1E, 0x0, 0x32, 0x0, 0x32, 0x0, 0x0, 0x0, 0x9, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,

            0x6, 0x7, 0x6E, 0x9, 0x4B, 0x0, 0x5, 0xF6, 0x90, 0x0, 0x0, 0x0, 0x63, 0x0, 0xD8, 0x0,
            0x57, 0x1, 0x44, 0x0, 0xC6, 0x0, 0x1E, 0x0, 0x32, 0x0, 0x32, 0x0, 0x0, 0x0, 0x9, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,

            0x7, 0x7, 0x6E, 0x9, 0x4B, 0x0, 0x5, 0xF6, 0x90, 0x0, 0x0, 0x0, 0x63, 0x0, 0xD8, 0x0,
            0x57, 0x1, 0x44, 0x0, 0xC6, 0x0, 0x1E, 0x0, 0x32, 0x0, 0x32, 0x0, 0x0, 0x0, 0x9, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,

            0x8, 0x7, 0x6E, 0x9, 0x4B, 0x0, 0x5, 0xF6, 0x90, 0x0, 0x0, 0x0, 0x63, 0x0, 0xD8, 0x0,
            0x57, 0x1, 0x44, 0x0, 0xC6, 0x0, 0x1E, 0x0, 0x32, 0x0, 0x32, 0x0, 0x0, 0x0, 0x9, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,

            0x9, 0x7, 0x6E, 0x9, 0x4B, 0x0, 0x5, 0xF6, 0x90, 0x0, 0x0, 0x0, 0x63, 0x0, 0xD8, 0x0,
            0x57, 0x1, 0x44, 0x0, 0xC6, 0x0, 0x1E, 0x0, 0x32, 0x0, 0x32, 0x0, 0x0, 0x0, 0x9, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,

            0xA, 0x7, 0x6E, 0x9, 0x4B, 0x0, 0x5, 0xF6, 0x90, 0x0, 0x0, 0x0, 0x63, 0x0, 0xD8, 0x0,
            0x57, 0x1, 0x44, 0x0, 0xC6, 0x0, 0x1E, 0x0, 0x32, 0x0, 0x32, 0x0, 0x0, 0x0, 0x9, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,

            0xB, 0x7, 0x6E, 0x9, 0x4B, 0x0, 0x5, 0xF6, 0x90, 0x0, 0x0, 0x0, 0x63, 0x0, 0xD8, 0x0,
            0x57, 0x1, 0x44, 0x0, 0xC6, 0x0, 0x1E, 0x0, 0x32, 0x0, 0x32, 0x0, 0x0, 0x0, 0x9, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0
        };
        private readonly byte[] JobDummy1 = { 0x7, 0x6E, 0x9, 0x4B, 0x0, 0x5, 0xF6, 0x90, 0x0, 0x0, 0x0 };
        private readonly byte[] JobDummy2 =
        {
            0x0, 0xD8, 0x0, 0x57, 0x1, 0x44, 0x0, 0xC6, 0x0, 0x1E, 0x0, 0x32, 0x0, 0x32, 0x0, 0x0,
            0x0, 0x9, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0
        };
        private readonly byte[] Pad14 = { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
        private readonly byte[] OrbDummy =
        {
            0x2, 0x9E, 0x0, 0x0, 0x0, 0x10, 0x0, 0x10, 0x0, 0xF, 0x0, 0x10, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0
        };
        private readonly byte[] OrbDummy2 =
        {
            0x0, 0x0, 0x0, 0x1, 0x0, 0x0, 0x0, 0x2, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x75, 0x30, 0x0, 0x0, 0x0, 0x3,
            0x0, 0x0, 0x0, 0x3, 0x0, 0x0, 0x0, 0x1, 0x0, 0x0, 0x0, 0x3, 0x0, 0x0, 0x0
        };
        private readonly byte[] BaseDataArray = { 0xD7, 0xE3, 0xDC, 0xCC, 0xA4, 0xEA, 0xEE, 0xE7, 0x78, 0xFE, 0x45, 0x96, 0x93, 0xD1, 0x90 };
        
    }
}
