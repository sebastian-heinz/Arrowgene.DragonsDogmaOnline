using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Csv
{
    public class MyPawnCsvReader : CsvReaderWriter<MyPawnCsv>
    {
        protected override int NumExpectedItems => 5;

        protected override MyPawnCsv CreateInstance(string[] properties)
        {
            if (!uint.TryParse(properties[0], out uint pawnId)) return null;
            if (!uint.TryParse(properties[1], out uint memberIndex)) return null;
            if (!int.TryParse(properties[2], out int stageNo)) return null;
            if (!int.TryParse(properties[3], out int startPosNo)) return null;
            if (!double.TryParse(properties[4], out double posX)) return null;
            if (!float.TryParse(properties[5], out float posY)) return null;
            if (!double.TryParse(properties[6], out double posZ)) return null;
            if (!float.TryParse(properties[7], out float angleY)) return null;
            if (!byte.TryParse(properties[8], out byte ucColor)) return null;
            string name = properties[9].Trim();
            if (!ushort.TryParse(properties[10], out ushort countEquip)) return null;
            if (!ushort.TryParse(properties[11], out ushort primary)) return null;
            if (!ushort.TryParse(properties[12], out ushort secondary)) return null;
            if (!ushort.TryParse(properties[13], out ushort head)) return null;
            if (!ushort.TryParse(properties[14], out ushort body)) return null;
            if (!ushort.TryParse(properties[15], out ushort bodyClothing)) return null;
            if (!ushort.TryParse(properties[16], out ushort arm)) return null;
            if (!ushort.TryParse(properties[17], out ushort leg)) return null;
            if (!ushort.TryParse(properties[18], out ushort legWear)) return null;
            if (!ushort.TryParse(properties[19], out ushort overWear)) return null;
            if (!ushort.TryParse(properties[20], out ushort jewelrySlot1)) return null;
            if (!ushort.TryParse(properties[21], out ushort jewelrySlot2)) return null;
            if (!ushort.TryParse(properties[22], out ushort jewelrySlot3)) return null;
            if (!ushort.TryParse(properties[23], out ushort jewelrySlot4)) return null;
            if (!ushort.TryParse(properties[24], out ushort jewelrySlot5)) return null;
            if (!ushort.TryParse(properties[25], out ushort lantern)) return null;
            if (!uint.TryParse(properties[26], out uint countVisualEquip)) return null;
            if (!ushort.TryParse(properties[27], out ushort vPrimary)) return null;
            if (!ushort.TryParse(properties[28], out ushort vSecondary)) return null;
            if (!ushort.TryParse(properties[29], out ushort vHead)) return null;
            if (!ushort.TryParse(properties[30], out ushort vBody)) return null;
            if (!ushort.TryParse(properties[31], out ushort vBodyClothing)) return null;
            if (!ushort.TryParse(properties[32], out ushort vArm)) return null;
            if (!ushort.TryParse(properties[33], out ushort vLeg)) return null;
            if (!ushort.TryParse(properties[34], out ushort vLegWear)) return null;
            if (!ushort.TryParse(properties[35], out ushort vOverWear)) return null;
            if (!ushort.TryParse(properties[36], out ushort vJewelrySlot1)) return null;
            if (!ushort.TryParse(properties[37], out ushort vJewelrySlot2)) return null;
            if (!ushort.TryParse(properties[38], out ushort vJewelrySlot3)) return null;
            if (!ushort.TryParse(properties[39], out ushort vJewelrySlot4)) return null;
            if (!ushort.TryParse(properties[40], out ushort vJewelrySlot5)) return null;
            if (!ushort.TryParse(properties[41], out ushort vLantern)) return null;
            if (!uint.TryParse(properties[42], out uint countJobItem)) return null;
            if (!ushort.TryParse(properties[43], out ushort jobItem1)) return null;
            if (!byte.TryParse(properties[44], out byte jobItemSlot1)) return null;
            if (!ushort.TryParse(properties[45], out ushort jobItem2)) return null;
            if (!byte.TryParse(properties[46], out byte jobItemSlot2)) return null;
            if (!bool.TryParse(properties[47], out bool hideEquipHead)) return null;
            if (!bool.TryParse(properties[48], out bool hideEquipLantern)) return null;
            if (!byte.TryParse(properties[49], out byte hmType)) return null;
            if (!byte.TryParse(properties[50], out byte pawnType)) return null;
            if (!uint.TryParse(properties[51], out uint characterId)) return null;
            if (!bool.TryParse(properties[52], out bool setWaitFlag)) return null;
            if (!byte.TryParse(properties[53], out byte countNormalSkill)) return null;
            if (!byte.TryParse(properties[54], out byte normalSkill1)) return null;
            if (!byte.TryParse(properties[55], out byte normalSkill2)) return null;
            if (!byte.TryParse(properties[56], out byte normalSkill3)) return null;
            if (!uint.TryParse(properties[57], out uint countCustomSkill)) return null;
            if (!byte.TryParse(properties[58], out byte customSkillSlot1)) return null;
            if (!uint.TryParse(properties[59], out uint customSkillId1)) return null;
            if (!byte.TryParse(properties[60], out byte customSkillLv1)) return null;
            if (!byte.TryParse(properties[61], out byte customSkillSlot2)) return null;
            if (!uint.TryParse(properties[62], out uint customSkillId2)) return null;
            if (!byte.TryParse(properties[63], out byte customSkillLv2)) return null;
            if (!byte.TryParse(properties[64], out byte customSkillSlot3)) return null;
            if (!uint.TryParse(properties[65], out uint customSkillId3)) return null;
            if (!byte.TryParse(properties[66], out byte customSkillLv3)) return null;
            if (!byte.TryParse(properties[67], out byte customSkillSlot4)) return null;
            if (!uint.TryParse(properties[68], out uint customSkillId4)) return null;
            if (!byte.TryParse(properties[69], out byte customSkillLv4)) return null;
            if (!uint.TryParse(properties[70], out uint countAbility)) return null;
            if (!byte.TryParse(properties[71], out byte abilityJob1)) return null;
            if (!uint.TryParse(properties[72], out uint abilityId1)) return null;
            if (!byte.TryParse(properties[73], out byte abilityLv1)) return null;
            if (!byte.TryParse(properties[74], out byte abilityJob2)) return null;
            if (!uint.TryParse(properties[75], out uint abilityId2)) return null;
            if (!byte.TryParse(properties[76], out byte abilityLv2)) return null;
            if (!byte.TryParse(properties[77], out byte abilityJob3)) return null;
            if (!uint.TryParse(properties[78], out uint abilityId3)) return null;
            if (!byte.TryParse(properties[79], out byte abilityLv3)) return null;
            if (!byte.TryParse(properties[80], out byte abilityJob4)) return null;
            if (!uint.TryParse(properties[81], out uint abilityId4)) return null;
            if (!byte.TryParse(properties[82], out byte abilityLv4)) return null;
            if (!byte.TryParse(properties[83], out byte abilityJob5)) return null;
            if (!uint.TryParse(properties[84], out uint abilityId5)) return null;
            if (!byte.TryParse(properties[85], out byte abilityLv5)) return null;
            if (!byte.TryParse(properties[86], out byte abilityJob6)) return null;
            if (!uint.TryParse(properties[87], out uint abilityId6)) return null;
            if (!byte.TryParse(properties[88], out byte abilityLv6)) return null;
            if (!byte.TryParse(properties[89], out byte abilityJob7)) return null;
            if (!uint.TryParse(properties[90], out uint abilityId7)) return null;
            if (!byte.TryParse(properties[91], out byte abilityLv7)) return null;
            if (!byte.TryParse(properties[92], out byte abilityJob8)) return null;
            if (!uint.TryParse(properties[93], out uint abilityId8)) return null;
            if (!byte.TryParse(properties[94], out byte abilityLv8)) return null;
            if (!byte.TryParse(properties[95], out byte abilityJob9)) return null;
            if (!uint.TryParse(properties[96], out uint abilityId9)) return null;
            if (!byte.TryParse(properties[97], out byte abilityLv9)) return null;
            if (!byte.TryParse(properties[98], out byte abilityJob10)) return null;
            if (!uint.TryParse(properties[99], out uint abilityId10)) return null;
            if (!byte.TryParse(properties[100], out byte abilityLv10)) return null;
            if (!byte.TryParse(properties[101], out byte job)) return null;
            if (!byte.TryParse(properties[102], out byte jobLv)) return null;
            if (!byte.TryParse(properties[103], out byte metPartyMembers)) return null;
            if (!uint.TryParse(properties[104], out uint metPartyMembersId)) return null;
            if (!byte.TryParse(properties[105], out byte questClear)) return null;
            if (!uint.TryParse(properties[106], out uint questClearId)) return null;
            if (!byte.TryParse(properties[107], out byte specialSkillInspirationMoment)) return null;
            if (!uint.TryParse(properties[108], out uint specialSkillInspirationMomentId)) return null;
            if (!byte.TryParse(properties[109], out byte levelUp)) return null;
            if (!uint.TryParse(properties[110], out uint levelUpId)) return null;
            if (!byte.TryParse(properties[111], out byte specialSkillUse)) return null;
            if (!uint.TryParse(properties[112], out uint specialSkillUseId)) return null;
            if (!byte.TryParse(properties[113], out byte playerDeath)) return null;
            if (!uint.TryParse(properties[114], out uint playerDeathId)) return null;
            if (!byte.TryParse(properties[115], out byte waitingOnLobby)) return null;
            if (!uint.TryParse(properties[116], out uint waitingOnLobbyId)) return null;
            if (!byte.TryParse(properties[117], out byte waitingOnAdventure)) return null;
            if (!uint.TryParse(properties[118], out uint waitingOnAdventureId)) return null;
            if (!byte.TryParse(properties[119], out byte endOfCombat)) return null;
            if (!uint.TryParse(properties[120], out uint endOfCombatId)) return null;
            if (!byte.TryParse(properties[121], out byte countSpSkill)) return null;
            if (!byte.TryParse(properties[122], out byte spSkillSlot1Id)) return null;
            if (!byte.TryParse(properties[123], out byte spSkillSlot1Lv)) return null;
            if (!byte.TryParse(properties[124], out byte spSkillSlot2Id)) return null;
            if (!byte.TryParse(properties[125], out byte spSkillSlot2Lv)) return null;
            if (!byte.TryParse(properties[126], out byte spSkillSlot3Id)) return null;
            if (!byte.TryParse(properties[127], out byte spSkillSlot3Lv)) return null;
            if (!byte.TryParse(properties[128], out byte sex)) return null;
            if (!byte.TryParse(properties[129], out byte voice)) return null;
            if (!ushort.TryParse(properties[130], out ushort voicePitch)) return null;
            if (!byte.TryParse(properties[131], out byte personality)) return null;
            if (!byte.TryParse(properties[132], out byte speechFreq)) return null;
            if (!byte.TryParse(properties[133], out byte bodyType)) return null;
            if (!byte.TryParse(properties[134], out byte hair)) return null;
            if (!byte.TryParse(properties[135], out byte beard)) return null;
            if (!byte.TryParse(properties[136], out byte makeup)) return null;
            if (!byte.TryParse(properties[137], out byte scar)) return null;
            if (!byte.TryParse(properties[138], out byte eyePresetNo)) return null;
            if (!byte.TryParse(properties[139], out byte nosePresetNo)) return null;
            if (!byte.TryParse(properties[140], out byte mouthPresetNo)) return null;
            if (!byte.TryParse(properties[141], out byte eyebrowTexNo)) return null;
            if (!byte.TryParse(properties[142], out byte colorSkin)) return null;
            if (!byte.TryParse(properties[143], out byte colorHair)) return null;
            if (!byte.TryParse(properties[144], out byte colorBeard)) return null;
            if (!byte.TryParse(properties[145], out byte colorEyebrow)) return null;
            if (!byte.TryParse(properties[146], out byte colorREye)) return null;
            if (!byte.TryParse(properties[147], out byte colorLEye)) return null;
            if (!byte.TryParse(properties[148], out byte colorMakeup)) return null;
            if (!ushort.TryParse(properties[149], out ushort sokutobu)) return null;
            if (!ushort.TryParse(properties[150], out ushort hitai)) return null;
            if (!ushort.TryParse(properties[151], out ushort mimiJyouge)) return null;
            if (!ushort.TryParse(properties[152], out ushort kannkaku)) return null;
            if (!ushort.TryParse(properties[153], out ushort mabisasiJyouge)) return null;
            if (!ushort.TryParse(properties[154], out ushort hanakuchiJyouge)) return null;
            if (!ushort.TryParse(properties[155], out ushort agoSakiHaba)) return null;
            if (!ushort.TryParse(properties[156], out ushort agoZengo)) return null;
            if (!ushort.TryParse(properties[157], out ushort agoSakiJyouge)) return null;
            if (!ushort.TryParse(properties[158], out ushort hitomiOokisa)) return null;
            if (!ushort.TryParse(properties[159], out ushort meOokisa)) return null;
            if (!ushort.TryParse(properties[160], out ushort meKaiten)) return null;
            if (!ushort.TryParse(properties[161], out ushort mayuKaiten)) return null;
            if (!ushort.TryParse(properties[162], out ushort mimiOokisa)) return null;
            if (!ushort.TryParse(properties[163], out ushort mimiMuki)) return null;
            if (!ushort.TryParse(properties[164], out ushort elfMimi)) return null;
            if (!ushort.TryParse(properties[165], out ushort mikenTakasa)) return null;
            if (!ushort.TryParse(properties[166], out ushort mikenHaba)) return null;
            if (!ushort.TryParse(properties[167], out ushort hohoboneRyou)) return null;
            if (!ushort.TryParse(properties[168], out ushort hohoboneJyouge)) return null;
            if (!ushort.TryParse(properties[169], out ushort hohoniku)) return null;
            if (!ushort.TryParse(properties[170], out ushort erahoneJyouge)) return null;
            if (!ushort.TryParse(properties[171], out ushort erahoneHaba)) return null;
            if (!ushort.TryParse(properties[172], out ushort hanaJyouge)) return null;
            if (!ushort.TryParse(properties[173], out ushort hanaHaba)) return null;
            if (!ushort.TryParse(properties[174], out ushort hanaTakasa)) return null;
            if (!ushort.TryParse(properties[175], out ushort hanaKakudo)) return null;
            if (!ushort.TryParse(properties[176], out ushort kuchiHaba)) return null;
            if (!ushort.TryParse(properties[177], out ushort kuchiAtsusa)) return null;
            if (!ushort.TryParse(properties[178], out ushort eyebrowUVOffsetX)) return null;
            if (!ushort.TryParse(properties[179], out ushort eyebrowUVOffsetY)) return null;
            if (!ushort.TryParse(properties[180], out ushort wrinkle)) return null;
            if (!ushort.TryParse(properties[181], out ushort wrinkleAlbedoBlendRate)) return null;
            if (!ushort.TryParse(properties[182], out ushort wrinkleDetailNormalPower)) return null;
            if (!ushort.TryParse(properties[183], out ushort muscleAlbedoBlendRate)) return null;
            if (!ushort.TryParse(properties[184], out ushort muscleDetailNormalPower)) return null;
            if (!ushort.TryParse(properties[185], out ushort height)) return null;
            if (!ushort.TryParse(properties[186], out ushort headSize)) return null;
            if (!ushort.TryParse(properties[187], out ushort neckOffset)) return null;
            if (!ushort.TryParse(properties[188], out ushort neckScale)) return null;
            if (!ushort.TryParse(properties[189], out ushort upperBodyScaleX)) return null;
            if (!ushort.TryParse(properties[190], out ushort bellySize)) return null;
            if (!ushort.TryParse(properties[191], out ushort teatScale)) return null;
            if (!ushort.TryParse(properties[192], out ushort tekubiSize)) return null;
            if (!ushort.TryParse(properties[193], out ushort koshiOffset)) return null;
            if (!ushort.TryParse(properties[194], out ushort koshiSize)) return null;
            if (!ushort.TryParse(properties[195], out ushort ankleOffset)) return null;
            if (!ushort.TryParse(properties[196], out ushort fat)) return null;
            if (!ushort.TryParse(properties[197], out ushort muscle)) return null;
            if (!ushort.TryParse(properties[198], out ushort motionFilter)) return null;


            return new MyPawnCsv
            {


                PawnId = pawnId,
                MemberIndex = memberIndex,
                StageNo = stageNo,
                StartPosNo = startPosNo,
                PosX = posX,
                PosY = posY,
                PosZ = posZ,
                AngleY = angleY,
                UcColor = ucColor,
                Name = name,
                CountEquip = countEquip,
                Primary = primary,
                Secondary = secondary,
                Head = head,
                Body = body,
                BodyClothing = bodyClothing,
                Arm = arm,
                Leg = leg,
                LegWear = legWear,
                OverWear = overWear,
                JewelrySlot1 = jewelrySlot1,
                JewelrySlot2 = jewelrySlot2,
                JewelrySlot3 = jewelrySlot3,
                JewelrySlot4 = jewelrySlot4,
                JewelrySlot5 = jewelrySlot5,
                Lantern = lantern,
                CountVisualEquip = countVisualEquip,
                VPrimary = vPrimary,
                VSecondary = vSecondary,
                VHead = vHead,
                VBody = vBody,
                VBodyClothing = vBodyClothing,
                VArm = vArm,
                VLeg = vLeg,
                VLegWear = vLegWear,
                VOverWear = vOverWear,
                VJewelrySlot1 = vJewelrySlot1,
                VJewelrySlot2 = vJewelrySlot2,
                VJewelrySlot3 = vJewelrySlot3,
                VJewelrySlot4 = vJewelrySlot4,
                VJewelrySlot5 = vJewelrySlot5,
                VLantern = vLantern,
                CountJobItem = countJobItem,
                JobItem1 = jobItem1,
                JobItemSlot1 = jobItemSlot1,
                JobItem2 = jobItem2,
                JobItemSlot2 = jobItemSlot2,
                HideEquipHead = hideEquipHead,
                HideEquipLantern = hideEquipLantern,
                HmType = hmType,
                PawnType = pawnType,
                CharacterId = characterId,
                SetWaitFlag = setWaitFlag,
                CountNormalSkill = countNormalSkill,
                NormalSkill1 = normalSkill1,
                NormalSkill2 = normalSkill2,
                NormalSkill3 = normalSkill3,
                CountCustomSkill = countCustomSkill,
                CustomSkillSlot1 = customSkillSlot1,
                CustomSkillId1 = customSkillId1,
                CustomSkillLv1 = customSkillLv1,
                CustomSkillSlot2 = customSkillSlot2,
                CustomSkillId2 = customSkillId2,
                CustomSkillLv2 = customSkillLv2,
                CustomSkillSlot3 = customSkillSlot3,
                CustomSkillId3 = customSkillId3,
                CustomSkillLv3 = customSkillLv3,
                CustomSkillSlot4 = customSkillSlot4,
                CustomSkillId4 = customSkillId4,
                CustomSkillLv4 = customSkillLv4,
                CountAbility = countAbility,
                AbilityJob1 = abilityJob1,
                AbilityId1 = abilityId1,
                AbilityLv1 = abilityLv1,
                AbilityJob2 = abilityJob2,
                AbilityId2 = abilityId2,
                AbilityLv2 = abilityLv2,
                AbilityJob3 = abilityJob3,
                AbilityId3 = abilityId3,
                AbilityLv3 = abilityLv3,
                AbilityJob4 = abilityJob4,
                AbilityId4 = abilityId4,
                AbilityLv4 = abilityLv4,
                AbilityJob5 = abilityJob5,
                AbilityId5 = abilityId5,
                AbilityLv5 = abilityLv5,
                AbilityJob6 = abilityJob6,
                AbilityId6 = abilityId6,
                AbilityLv6 = abilityLv6,
                AbilityJob7 = abilityJob7,
                AbilityId7 = abilityId7,
                AbilityLv7 = abilityLv7,
                AbilityJob8 = abilityJob8,
                AbilityId8 = abilityId8,
                AbilityLv8 = abilityLv8,
                AbilityJob9 = abilityJob9,
                AbilityId9 = abilityId9,
                AbilityLv9 = abilityLv9,
                AbilityJob10 = abilityJob10,
                AbilityId10 = abilityId10,
                AbilityLv10 = abilityLv10,
                Job = (JobId) job,
                JobLv = jobLv,
                MetPartyMembers = metPartyMembers,
                MetPartyMembersId = metPartyMembersId,
                QuestClear = questClear,
                QuestClearId = questClearId,
                SpecialSkillInspirationMoment = specialSkillInspirationMoment,
                SpecialSkillInspirationMomentId = specialSkillInspirationMomentId,
                LevelUp = levelUp,
                LevelUpId = levelUpId,
                SpecialSkillUse = specialSkillUse,
                SpecialSkillUseId = specialSkillUseId,
                PlayerDeath = playerDeath,
                PlayerDeathId = playerDeathId,
                WaitingOnLobby = waitingOnLobby,
                WaitingOnLobbyId = waitingOnLobbyId,
                WaitingOnAdventure = waitingOnAdventure,
                WaitingOnAdventureId = waitingOnAdventureId,
                EndOfCombat = endOfCombat,
                EndOfCombatId = endOfCombatId,
                CountSpSkill = countSpSkill,
                SpSkillSlot1Id = spSkillSlot1Id,
                SpSkillSlot1Lv = spSkillSlot1Lv,
                SpSkillSlot2Id = spSkillSlot2Id,
                SpSkillSlot2Lv = spSkillSlot2Lv,
                SpSkillSlot3Id = spSkillSlot3Id,
                SpSkillSlot3Lv = spSkillSlot3Lv,
                Sex = sex,
                Voice = voice,
                VoicePitch = voicePitch,
                Personality = personality,
                SpeechFreq = speechFreq,
                BodyType = bodyType,
                Hair = hair,
                Beard = beard,
                Makeup = makeup,
                Scar = scar,
                EyePresetNo = eyePresetNo,
                NosePresetNo = nosePresetNo,
                MouthPresetNo = mouthPresetNo,
                EyebrowTexNo = eyebrowTexNo,
                ColorSkin = colorSkin,
                ColorHair = colorHair,
                ColorBeard = colorBeard,
                ColorEyebrow = colorEyebrow,
                ColorREye = colorREye,
                ColorLEye = colorLEye,
                ColorMakeup = colorMakeup,
                Sokutobu = sokutobu,
                Hitai = hitai,
                MimiJyouge = mimiJyouge,
                Kannkaku = kannkaku,
                MabisasiJyouge = mabisasiJyouge,
                HanakuchiJyouge = hanakuchiJyouge,
                AgoSakiHaba = agoSakiHaba,
                AgoZengo = agoZengo,
                AgoSakiJyouge = agoSakiJyouge,
                HitomiOokisa = hitomiOokisa,
                MeOokisa = meOokisa,
                MeKaiten = meKaiten,
                MayuKaiten = mayuKaiten,
                MimiOokisa = mimiOokisa,
                MimiMuki = mimiMuki,
                ElfMimi = elfMimi,
                MikenTakasa = mikenTakasa,
                MikenHaba = mikenHaba,
                HohoboneRyou = hohoboneRyou,
                HohoboneJyouge = hohoboneJyouge,
                Hohoniku = hohoniku,
                ErahoneJyouge = erahoneJyouge,
                ErahoneHaba = erahoneHaba,
                HanaJyouge = hanaJyouge,
                HanaHaba = hanaHaba,
                HanaTakasa = hanaTakasa,
                HanaKakudo = hanaKakudo,
                KuchiHaba = kuchiHaba,
                KuchiAtsusa = kuchiAtsusa,
                EyebrowUVOffsetX = eyebrowUVOffsetX,
                EyebrowUVOffsetY = eyebrowUVOffsetY,
                Wrinkle = wrinkle,
                WrinkleAlbedoBlendRate = wrinkleAlbedoBlendRate,
                WrinkleDetailNormalPower = wrinkleDetailNormalPower,
                MuscleAlbedoBlendRate = muscleAlbedoBlendRate,
                MuscleDetailNormalPower = muscleDetailNormalPower,
                Height = height,
                HeadSize = headSize,
                NeckOffset = neckOffset,
                NeckScale = neckScale,
                UpperBodyScaleX = upperBodyScaleX,
                BellySize = bellySize,
                TeatScale = teatScale,
                TekubiSize = tekubiSize,
                KoshiOffset = koshiOffset,
                KoshiSize = koshiSize,
                AnkleOffset = ankleOffset,
                Fat = fat,
                Muscle = muscle,
                MotionFilter = motionFilter,



            };
        }
    }
}
