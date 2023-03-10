using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Csv
{
    public class ArisenCsvReader : CsvReaderWriter<ArisenCsv>
    {
        protected override int NumExpectedItems => 5;

        protected override ArisenCsv CreateInstance(string[] properties)
        {
            if (!byte.TryParse(properties[0], out byte job)) return null;
            if (!uint.TryParse(properties[1], out uint lv)) return null;
            if (!uint.TryParse(properties[2], out uint hp)) return null;
            if (!uint.TryParse(properties[3], out uint stamina)) return null;
            if (!byte.TryParse(properties[4], out byte revivePoint)) return null;
            if (!uint.TryParse(properties[5], out uint maxHp)) return null;
            if (!uint.TryParse(properties[6], out uint maxStamina)) return null;
            if (!uint.TryParse(properties[7], out uint whiteHp)) return null;
            if (!uint.TryParse(properties[8], out uint gainHp)) return null;
            if (!uint.TryParse(properties[9], out uint gainStamina)) return null;
            if (!uint.TryParse(properties[10], out uint gainAttack)) return null;
            if (!uint.TryParse(properties[11], out uint gainDefense)) return null;
            if (!uint.TryParse(properties[12], out uint gainMagicAttack)) return null;
            if (!uint.TryParse(properties[13], out uint gainMagicDefense)) return null;
            if (!uint.TryParse(properties[14], out uint cs1MpId)) return null;
            if (!byte.TryParse(properties[15], out byte cs1MpLv)) return null;
            if (!uint.TryParse(properties[16], out uint cs2MpId)) return null;
            if (!byte.TryParse(properties[17], out byte cs2MpLv)) return null;
            if (!uint.TryParse(properties[18], out uint cs3MpId)) return null;
            if (!byte.TryParse(properties[19], out byte cs3MpLv)) return null;
            if (!uint.TryParse(properties[20], out uint cs4MpId)) return null;
            if (!byte.TryParse(properties[21], out byte cs4MpLv)) return null;
            if (!uint.TryParse(properties[22], out uint cs1SpId)) return null;
            if (!byte.TryParse(properties[23], out byte cs1SpLv)) return null;
            if (!uint.TryParse(properties[24], out uint cs2SpId)) return null;
            if (!byte.TryParse(properties[25], out byte cs2SpLv)) return null;
            if (!uint.TryParse(properties[26], out uint cs3SpId)) return null;
            if (!byte.TryParse(properties[27], out byte cs3SpLv)) return null;
            if (!uint.TryParse(properties[28], out uint cs4SpId)) return null;
            if (!byte.TryParse(properties[29], out byte cs4SpLv)) return null;
            if (!byte.TryParse(properties[30], out byte ab1Jb)) return null;
            if (!uint.TryParse(properties[31], out uint ab1Id)) return null;
            if (!byte.TryParse(properties[32], out byte ab1Lv)) return null;
            if (!byte.TryParse(properties[33], out byte ab2Jb)) return null;
            if (!uint.TryParse(properties[34], out uint ab2Id)) return null;
            if (!byte.TryParse(properties[35], out byte ab2Lv)) return null;
            if (!byte.TryParse(properties[36], out byte ab3Jb)) return null;
            if (!uint.TryParse(properties[37], out uint ab3Id)) return null;
            if (!byte.TryParse(properties[38], out byte ab3Lv)) return null;
            if (!byte.TryParse(properties[39], out byte ab4Jb)) return null;
            if (!uint.TryParse(properties[40], out uint ab4Id)) return null;
            if (!byte.TryParse(properties[41], out byte ab4Lv)) return null;
            if (!byte.TryParse(properties[42], out byte ab5Jb)) return null;
            if (!uint.TryParse(properties[43], out uint ab5Id)) return null;
            if (!byte.TryParse(properties[44], out byte ab5Lv)) return null;
            if (!byte.TryParse(properties[45], out byte ab6Jb)) return null;
            if (!uint.TryParse(properties[46], out uint ab6Id)) return null;
            if (!byte.TryParse(properties[47], out byte ab6Lv)) return null;
            if (!byte.TryParse(properties[48], out byte ab7Jb)) return null;
            if (!uint.TryParse(properties[49], out uint ab7Id)) return null;
            if (!byte.TryParse(properties[50], out byte ab7Lv)) return null;
            if (!byte.TryParse(properties[51], out byte ab8Jb)) return null;
            if (!uint.TryParse(properties[52], out uint ab8Id)) return null;
            if (!byte.TryParse(properties[53], out byte ab8Lv)) return null;
            if (!byte.TryParse(properties[54], out byte ab9Jb)) return null;
            if (!uint.TryParse(properties[55], out uint ab9Id)) return null;
            if (!byte.TryParse(properties[56], out byte ab9Lv)) return null;
            if (!byte.TryParse(properties[57], out byte ab10Jb)) return null;
            if (!uint.TryParse(properties[58], out uint ab10Id)) return null;
            if (!byte.TryParse(properties[59], out byte ab10Lv)) return null;
            if (!ushort.TryParse(properties[60], out ushort pAtk)) return null;
            if (!ushort.TryParse(properties[61], out ushort pDef)) return null;
            if (!ushort.TryParse(properties[62], out ushort mAtk)) return null;
            if (!ushort.TryParse(properties[63], out ushort mDef)) return null;
            if (!uint.TryParse(properties[64], out uint primaryWeapon)) return null;
            if (!byte.TryParse(properties[65], out byte primaryWeaponColour)) return null;
            if (!uint.TryParse(properties[66], out uint pWCrest1)) return null;
            if (!byte.TryParse(properties[67], out byte pWC1Add1)) return null;
            if (!byte.TryParse(properties[68], out byte pWC1Add2)) return null;
            if (!uint.TryParse(properties[69], out uint pWCrest2)) return null;
            if (!byte.TryParse(properties[70], out byte pWC2Add1)) return null;
            if (!byte.TryParse(properties[71], out byte pWC2Add2)) return null;
            if (!uint.TryParse(properties[72], out uint pWCrest3)) return null;
            if (!byte.TryParse(properties[73], out byte pWC3Add1)) return null;
            if (!byte.TryParse(properties[74], out byte pWC3Add2)) return null;
            if (!uint.TryParse(properties[75], out uint pWCrest4)) return null;
            if (!byte.TryParse(properties[76], out byte pWC4Add1)) return null;
            if (!byte.TryParse(properties[77], out byte pWC4Add2)) return null;
            if (!uint.TryParse(properties[78], out uint secondaryWeapon)) return null;
            if (!byte.TryParse(properties[79], out byte secondaryWeaponColour)) return null;
            if (!uint.TryParse(properties[80], out uint head)) return null;
            if (!byte.TryParse(properties[81], out byte headColour)) return null;
            if (!uint.TryParse(properties[82], out uint headCrest1)) return null;
            if (!byte.TryParse(properties[83], out byte hC1Add1)) return null;
            if (!byte.TryParse(properties[84], out byte hC1Add2)) return null;
            if (!uint.TryParse(properties[85], out uint headCrest2)) return null;
            if (!byte.TryParse(properties[86], out byte hC2Add1)) return null;
            if (!byte.TryParse(properties[87], out byte hC2Add2)) return null;
            if (!uint.TryParse(properties[88], out uint headCrest3)) return null;
            if (!byte.TryParse(properties[89], out byte hC3Add1)) return null;
            if (!byte.TryParse(properties[90], out byte hC3Add2)) return null;
            if (!uint.TryParse(properties[91], out uint body)) return null;
            if (!byte.TryParse(properties[92], out byte bodyColour)) return null;
            if (!uint.TryParse(properties[93], out uint bodyCrest1)) return null;
            if (!byte.TryParse(properties[94], out byte bC1Add1)) return null;
            if (!byte.TryParse(properties[95], out byte bC1Add2)) return null;
            if (!uint.TryParse(properties[96], out uint bodyCrest2)) return null;
            if (!byte.TryParse(properties[97], out byte bC2Add1)) return null;
            if (!byte.TryParse(properties[98], out byte bC2Add2)) return null;
            if (!uint.TryParse(properties[99], out uint bodyCrest3)) return null;
            if (!byte.TryParse(properties[100], out byte bC3Add1)) return null;
            if (!byte.TryParse(properties[101], out byte bC3Add2)) return null;
            if (!uint.TryParse(properties[102], out uint bodyCrest4)) return null;
            if (!byte.TryParse(properties[103], out byte bC4Add1)) return null;
            if (!byte.TryParse(properties[104], out byte bC4Add2)) return null;
            if (!uint.TryParse(properties[105], out uint clothing)) return null;
            if (!byte.TryParse(properties[106], out byte clothingColour)) return null;
            if (!uint.TryParse(properties[107], out uint arm)) return null;
            if (!byte.TryParse(properties[108], out byte armColour)) return null;
            if (!uint.TryParse(properties[109], out uint armCrest1)) return null;
            if (!byte.TryParse(properties[110], out byte aC1Add1)) return null;
            if (!byte.TryParse(properties[111], out byte aC1Add2)) return null;
            if (!uint.TryParse(properties[112], out uint armCrest2)) return null;
            if (!byte.TryParse(properties[113], out byte aC2Add1)) return null;
            if (!byte.TryParse(properties[114], out byte aC2Add2)) return null;
            if (!uint.TryParse(properties[115], out uint armCrest3)) return null;
            if (!byte.TryParse(properties[116], out byte aC3Add1)) return null;
            if (!byte.TryParse(properties[117], out byte aC3Add2)) return null;
            if (!uint.TryParse(properties[118], out uint leg)) return null;
            if (!byte.TryParse(properties[119], out byte legColour)) return null;
            if (!uint.TryParse(properties[120], out uint legCrest1)) return null;
            if (!byte.TryParse(properties[121], out byte lC1Add1)) return null;
            if (!byte.TryParse(properties[122], out byte lC1Add2)) return null;
            if (!uint.TryParse(properties[123], out uint legCrest2)) return null;
            if (!byte.TryParse(properties[124], out byte lC2Add1)) return null;
            if (!byte.TryParse(properties[125], out byte lC2Add2)) return null;
            if (!uint.TryParse(properties[126], out uint legCrest3)) return null;
            if (!byte.TryParse(properties[127], out byte lC3Add1)) return null;
            if (!byte.TryParse(properties[128], out byte lC3Add2)) return null;
            if (!uint.TryParse(properties[129], out uint legwear)) return null;
            if (!byte.TryParse(properties[130], out byte legwearColour)) return null;
            if (!uint.TryParse(properties[131], out uint overwear)) return null;
            if (!byte.TryParse(properties[132], out byte overwearColour)) return null;
            if (!uint.TryParse(properties[133], out uint lantern)) return null;
            if (!bool.TryParse(properties[134], out bool displayHelmet)) return null;
            if (!bool.TryParse(properties[135], out bool displayLantern)) return null;
            if (!uint.TryParse(properties[136], out uint vPrimaryWeapon)) return null;
            if (!byte.TryParse(properties[137], out byte vPrimaryWeaponColour)) return null;
            if (!uint.TryParse(properties[138], out uint vSecondaryWeapon)) return null;
            if (!byte.TryParse(properties[139], out byte vSecondaryWeaponColour)) return null;
            if (!uint.TryParse(properties[140], out uint vHead)) return null;
            if (!byte.TryParse(properties[141], out byte vHeadColour)) return null;
            if (!uint.TryParse(properties[142], out uint vBody)) return null;
            if (!byte.TryParse(properties[143], out byte vBodyColour)) return null;
            if (!uint.TryParse(properties[144], out uint vClothing)) return null;
            if (!byte.TryParse(properties[145], out byte vClothingColour)) return null;
            if (!uint.TryParse(properties[146], out uint vArm)) return null;
            if (!byte.TryParse(properties[147], out byte vArmColour)) return null;
            if (!uint.TryParse(properties[148], out uint vLeg)) return null;
            if (!byte.TryParse(properties[149], out byte vLegColour)) return null;
            if (!uint.TryParse(properties[150], out uint vLegwear)) return null;
            if (!byte.TryParse(properties[151], out byte vLegwearColour)) return null;
            if (!uint.TryParse(properties[152], out uint vOverwear)) return null;
            if (!byte.TryParse(properties[153], out byte vOverwearColour)) return null;
            if (!uint.TryParse(properties[154], out uint classItem1)) return null;
            if (!uint.TryParse(properties[155], out uint classItem2)) return null;
            if (!uint.TryParse(properties[156], out uint jewelry1)) return null;
            if (!uint.TryParse(properties[157], out uint j1Crest1)) return null;
            if (!byte.TryParse(properties[158], out byte j1C1Add1)) return null;
            if (!byte.TryParse(properties[159], out byte j1C1Add2)) return null;
            if (!uint.TryParse(properties[160], out uint j1Crest2)) return null;
            if (!byte.TryParse(properties[161], out byte j1C2Add1)) return null;
            if (!byte.TryParse(properties[162], out byte j1C2Add2)) return null;
            if (!uint.TryParse(properties[163], out uint j1Crest3)) return null;
            if (!byte.TryParse(properties[164], out byte j1C3Add1)) return null;
            if (!byte.TryParse(properties[165], out byte j1C3Add2)) return null;
            if (!uint.TryParse(properties[166], out uint j1Crest4)) return null;
            if (!byte.TryParse(properties[167], out byte j1C4Add1)) return null;
            if (!byte.TryParse(properties[168], out byte j1C4Add2)) return null;
            if (!uint.TryParse(properties[169], out uint jewelry2)) return null;
            if (!uint.TryParse(properties[170], out uint j2Crest1)) return null;
            if (!byte.TryParse(properties[171], out byte j2C1Add1)) return null;
            if (!byte.TryParse(properties[172], out byte j2C1Add2)) return null;
            if (!uint.TryParse(properties[173], out uint j2Crest2)) return null;
            if (!byte.TryParse(properties[174], out byte j2C2Add1)) return null;
            if (!byte.TryParse(properties[175], out byte j2C2Add2)) return null;
            if (!uint.TryParse(properties[176], out uint j2Crest3)) return null;
            if (!byte.TryParse(properties[177], out byte j2C3Add1)) return null;
            if (!byte.TryParse(properties[178], out byte j2C3Add2)) return null;
            if (!uint.TryParse(properties[179], out uint j2Crest4)) return null;
            if (!byte.TryParse(properties[180], out byte j2C4Add1)) return null;
            if (!byte.TryParse(properties[181], out byte j2C4Add2)) return null;
            if (!uint.TryParse(properties[182], out uint jewelry3)) return null;
            if (!uint.TryParse(properties[183], out uint j3Crest1)) return null;
            if (!byte.TryParse(properties[184], out byte j3C1Add1)) return null;
            if (!byte.TryParse(properties[185], out byte j3C1Add2)) return null;
            if (!uint.TryParse(properties[186], out uint j3Crest2)) return null;
            if (!byte.TryParse(properties[187], out byte j3C2Add1)) return null;
            if (!byte.TryParse(properties[188], out byte j3C2Add2)) return null;
            if (!uint.TryParse(properties[189], out uint j3Crest3)) return null;
            if (!byte.TryParse(properties[190], out byte j3C3Add1)) return null;
            if (!byte.TryParse(properties[191], out byte j3C3Add2)) return null;
            if (!uint.TryParse(properties[192], out uint j3Crest4)) return null;
            if (!byte.TryParse(properties[193], out byte j3C4Add1)) return null;
            if (!byte.TryParse(properties[194], out byte j3C4Add2)) return null;
            if (!uint.TryParse(properties[195], out uint jewelry4)) return null;
            if (!uint.TryParse(properties[196], out uint j4Crest1)) return null;
            if (!byte.TryParse(properties[197], out byte j4C1Add1)) return null;
            if (!byte.TryParse(properties[198], out byte j4C1Add2)) return null;
            if (!uint.TryParse(properties[199], out uint j4Crest2)) return null;
            if (!byte.TryParse(properties[200], out byte j4C2Add1)) return null;
            if (!byte.TryParse(properties[201], out byte j4C2Add2)) return null;
            if (!uint.TryParse(properties[202], out uint j4Crest3)) return null;
            if (!byte.TryParse(properties[203], out byte j4C3Add1)) return null;
            if (!byte.TryParse(properties[204], out byte j4C3Add2)) return null;
            if (!uint.TryParse(properties[205], out uint j4Crest4)) return null;
            if (!byte.TryParse(properties[206], out byte j4C4Add1)) return null;
            if (!byte.TryParse(properties[207], out byte j4C4Add2)) return null;
            if (!uint.TryParse(properties[208], out uint jewelry5)) return null;
            if (!uint.TryParse(properties[209], out uint j5Crest1)) return null;
            if (!byte.TryParse(properties[210], out byte j5C1Add1)) return null;
            if (!byte.TryParse(properties[211], out byte j5C1Add2)) return null;
            if (!uint.TryParse(properties[212], out uint j5Crest2)) return null;
            if (!byte.TryParse(properties[213], out byte j5C2Add1)) return null;
            if (!byte.TryParse(properties[214], out byte j5C2Add2)) return null;
            if (!uint.TryParse(properties[215], out uint j5Crest3)) return null;
            if (!byte.TryParse(properties[216], out byte j5C3Add1)) return null;
            if (!byte.TryParse(properties[217], out byte j5C3Add2)) return null;
            if (!uint.TryParse(properties[218], out uint j5Crest4)) return null;
            if (!byte.TryParse(properties[219], out byte j5C4Add1)) return null;
            if (!byte.TryParse(properties[220], out byte j5C4Add2)) return null;
            if (!uint.TryParse(properties[221], out uint exp)) return null;
            if (!uint.TryParse(properties[222], out uint jobPoint)) return null;
            if (!ushort.TryParse(properties[223], out ushort strength)) return null;
            if (!ushort.TryParse(properties[224], out ushort downPower)) return null;
            if (!ushort.TryParse(properties[225], out ushort shakePower)) return null;
            if (!ushort.TryParse(properties[226], out ushort stunPower)) return null;
            if (!ushort.TryParse(properties[227], out ushort consitution)) return null;
            if (!ushort.TryParse(properties[228], out ushort guts)) return null;
            if (!byte.TryParse(properties[229], out byte fireResist)) return null;
            if (!byte.TryParse(properties[230], out byte iceResist)) return null;
            if (!byte.TryParse(properties[231], out byte thunderResist)) return null;
            if (!byte.TryParse(properties[232], out byte holyResist)) return null;
            if (!byte.TryParse(properties[233], out byte darkResist)) return null;
            if (!byte.TryParse(properties[234], out byte spreadResist)) return null;
            if (!byte.TryParse(properties[235], out byte freezeResist)) return null;
            if (!byte.TryParse(properties[236], out byte shockResist)) return null;
            if (!byte.TryParse(properties[237], out byte absorbResist)) return null;
            if (!byte.TryParse(properties[238], out byte darkElmResist)) return null;
            if (!byte.TryParse(properties[239], out byte poisonResist)) return null;
            if (!byte.TryParse(properties[240], out byte slowResist)) return null;
            if (!byte.TryParse(properties[241], out byte sleepResist)) return null;
            if (!byte.TryParse(properties[242], out byte stunResist)) return null;
            if (!byte.TryParse(properties[243], out byte wetResist)) return null;
            if (!byte.TryParse(properties[244], out byte oilResist)) return null;
            if (!byte.TryParse(properties[245], out byte sealResist)) return null;
            if (!byte.TryParse(properties[246], out byte curseResist)) return null;
            if (!byte.TryParse(properties[247], out byte softResist)) return null;
            if (!byte.TryParse(properties[248], out byte stoneResist)) return null;
            if (!byte.TryParse(properties[249], out byte goldResist)) return null;
            if (!byte.TryParse(properties[250], out byte fireReduceResist)) return null;
            if (!byte.TryParse(properties[251], out byte iceReduceResist)) return null;
            if (!byte.TryParse(properties[252], out byte thunderReduceResist)) return null;
            if (!byte.TryParse(properties[253], out byte holyReduceResist)) return null;
            if (!byte.TryParse(properties[254], out byte darkReduceResist)) return null;
            if (!byte.TryParse(properties[255], out byte atkDownResist)) return null;
            if (!byte.TryParse(properties[256], out byte defDownResist)) return null;
            if (!byte.TryParse(properties[257], out byte mAtkDownResist)) return null;
            if (!byte.TryParse(properties[258], out byte mDefDownResist)) return null;
            if (!uint.TryParse(properties[259], out uint normalSkill1A)) return null;
            if (!uint.TryParse(properties[260], out uint normalSkill1B)) return null;
            if (!uint.TryParse(properties[261], out uint normalSkill2A)) return null;
            if (!uint.TryParse(properties[262], out uint normalSkill2B)) return null;
            if (!uint.TryParse(properties[263], out uint normalSkill3A)) return null;
            if (!uint.TryParse(properties[264], out uint normalSkill3B)) return null;

            return new ArisenCsv
            {
                Job = (JobId) job,
                Lv = lv,
                HP = hp,
                Stamina = stamina,
                RevivePoint = revivePoint,
                MaxHP = maxHp,
                MaxStamina = maxStamina,
                WhiteHP = whiteHp,
                GainHP = gainHp,
                GainStamina = gainStamina,
                GainAttack = gainAttack,
                GainDefense = gainDefense,
                GainMagicAttack = gainMagicAttack,
                GainMagicDefense = gainMagicDefense,
                Cs1MpId = cs1MpId,
                Cs1MpLv = cs1MpLv,
                Cs2MpId = cs2MpId,
                Cs2MpLv = cs2MpLv,
                Cs3MpId = cs3MpId,
                Cs3MpLv = cs3MpLv,
                Cs4MpId = cs4MpId,
                Cs4MpLv = cs4MpLv,
                Cs1SpId = cs1SpId,
                Cs1SpLv = cs1SpLv,
                Cs2SpId = cs2SpId,
                Cs2SpLv = cs2SpLv,
                Cs3SpId = cs3SpId,
                Cs3SpLv = cs3SpLv,
                Cs4SpId = cs4SpId,
                Cs4SpLv = cs4SpLv,
                Ab1Jb = (JobId) ab1Jb,
                Ab1Id = ab1Id,
                Ab1Lv = ab1Lv,
                Ab2Jb = (JobId) ab2Jb,
                Ab2Id = ab2Id,
                Ab2Lv = ab2Lv,
                Ab3Jb = (JobId) ab3Jb,
                Ab3Id = ab3Id,
                Ab3Lv = ab3Lv,
                Ab4Jb = (JobId) ab4Jb,
                Ab4Id = ab4Id,
                Ab4Lv = ab4Lv,
                Ab5Jb = (JobId) ab5Jb,
                Ab5Id = ab5Id,
                Ab5Lv = ab5Lv,
                Ab6Jb = (JobId) ab6Jb,
                Ab6Id = ab6Id,
                Ab6Lv = ab6Lv,
                Ab7Jb = (JobId) ab7Jb,
                Ab7Id = ab7Id,
                Ab7Lv = ab7Lv,
                Ab8Jb = (JobId) ab8Jb,
                Ab8Id = ab8Id,
                Ab8Lv = ab8Lv,
                Ab9Jb = (JobId) ab9Jb,
                Ab9Id = ab9Id,
                Ab9Lv = ab9Lv,
                Ab10Jb = (JobId) ab10Jb,
                Ab10Id = ab10Id,
                Ab10Lv = ab10Lv,
                PAtk = pAtk,
                PDef = pDef,
                MAtk = mAtk,
                MDef = mDef,
                PrimaryWeapon = primaryWeapon,
                PrimaryWeaponColour = primaryWeaponColour,
                PWCrest1 = pWCrest1,
                PWC1Add1 = pWC1Add1,
                PWC1Add2 = pWC1Add2,
                PWCrest2 = pWCrest2,
                PWC2Add1 = pWC2Add1,
                PWC2Add2 = pWC2Add2,
                PWCrest3 = pWCrest3,
                PWC3Add1 = pWC3Add1,
                PWC3Add2 = pWC3Add2,
                PWCrest4 = pWCrest4,
                PWC4Add1 = pWC4Add1,
                PWC4Add2 = pWC4Add2,
                SecondaryWeapon = secondaryWeapon,
                SecondaryWeaponColour = secondaryWeaponColour,
                Head = head,
                HeadColour = headColour,
                HeadCrest1 = headCrest1,
                HC1Add1 = hC1Add1,
                HC1Add2 = hC1Add2,
                HeadCrest2 = headCrest2,
                HC2Add1 = hC2Add1,
                HC2Add2 = hC2Add2,
                HeadCrest3 = headCrest3,
                HC3Add1 = hC3Add1,
                HC3Add2 = hC3Add2,
                Body = body,
                BodyColour = bodyColour,
                BodyCrest1 = bodyCrest1,
                BC1Add1 = bC1Add1,
                BC1Add2 = bC1Add2,
                BodyCrest2 = bodyCrest2,
                BC2Add1 = bC2Add1,
                BC2Add2 = bC2Add2,
                BodyCrest3 = bodyCrest3,
                BC3Add1 = bC3Add1,
                BC3Add2 = bC3Add2,
                BodyCrest4 = bodyCrest4,
                BC4Add1 = bC4Add1,
                BC4Add2 = bC4Add2,
                Clothing = clothing,
                ClothingColour = clothingColour,
                Arm = arm,
                ArmColour = armColour,
                ArmCrest1 = armCrest1,
                AC1Add1 = aC1Add1,
                AC1Add2 = aC1Add2,
                ArmCrest2 = armCrest2,
                AC2Add1 = aC2Add1,
                AC2Add2 = aC2Add2,
                ArmCrest3 = armCrest3,
                AC3Add1 = aC3Add1,
                AC3Add2 = aC3Add2,
                Leg = leg,
                LegColour = legColour,
                LegCrest1 = legCrest1,
                LC1Add1 = lC1Add1,
                LC1Add2 = lC1Add2,
                LegCrest2 = legCrest2,
                LC2Add1 = lC2Add1,
                LC2Add2 = lC2Add2,
                LegCrest3 = legCrest3,
                LC3Add1 = lC3Add1,
                LC3Add2 = lC3Add2,
                Legwear = legwear,
                LegwearColour = legwearColour,
                Overwear = overwear,
                OverwearColour = overwearColour,
                Lantern = lantern,
                DisplayHelmet = displayHelmet,
                DisplayLantern = displayLantern,
                VPrimaryWeapon = vPrimaryWeapon,
                VPrimaryWeaponColour = vPrimaryWeaponColour,
                VSecondaryWeapon = vSecondaryWeapon,
                VSecondaryWeaponColour = vSecondaryWeaponColour,
                VHead = vHead,
                VHeadColour = vHeadColour,
                VBody = vBody,
                VBodyColour = vBodyColour,
                VClothing = vClothing,
                VClothingColour = vClothingColour,
                VArm = vArm,
                VArmColour = vArmColour,
                VLeg = vLeg,
                VLegColour = vLegColour,
                VLegwear = vLegwear,
                VLegwearColour = vLegwearColour,
                VOverwear = vOverwear,
                VOverwearColour = vOverwearColour,
                ClassItem1 = classItem1,
                ClassItem2 = classItem2,
                Jewelry1 = jewelry1,
                J1Crest1 = j1Crest1,
                J1C1Add1 = j1C1Add1,
                J1C1Add2 = j1C1Add2,
                J1Crest2 = j1Crest2,
                J1C2Add1 = j1C2Add1,
                J1C2Add2 = j1C2Add2,
                J1Crest3 = j1Crest3,
                J1C3Add1 = j1C3Add1,
                J1C3Add2 = j1C3Add2,
                J1Crest4 = j1Crest4,
                J1C4Add1 = j1C4Add1,
                J1C4Add2 = j1C4Add2,
                Jewelry2 = jewelry2,
                J2Crest1 = j2Crest1,
                J2C1Add1 = j2C1Add1,
                J2C1Add2 = j2C1Add2,
                J2Crest2 = j2Crest2,
                J2C2Add1 = j2C2Add1,
                J2C2Add2 = j2C2Add2,
                J2Crest3 = j2Crest3,
                J2C3Add1 = j2C3Add1,
                J2C3Add2 = j2C3Add2,
                J2Crest4 = j2Crest4,
                J2C4Add1 = j2C4Add1,
                J2C4Add2 = j2C4Add2,
                Jewelry3 = jewelry3,
                J3Crest1 = j3Crest1,
                J3C1Add1 = j3C1Add1,
                J3C1Add2 = j3C1Add2,
                J3Crest2 = j3Crest2,
                J3C2Add1 = j3C2Add1,
                J3C2Add2 = j3C2Add2,
                J3Crest3 = j3Crest3,
                J3C3Add1 = j3C3Add1,
                J3C3Add2 = j3C3Add2,
                J3Crest4 = j3Crest4,
                J3C4Add1 = j3C4Add1,
                J3C4Add2 = j3C4Add2,
                Jewelry4 = jewelry4,
                J4Crest1 = j4Crest1,
                J4C1Add1 = j4C1Add1,
                J4C1Add2 = j4C1Add2,
                J4Crest2 = j4Crest2,
                J4C2Add1 = j4C2Add1,
                J4C2Add2 = j4C2Add2,
                J4Crest3 = j4Crest3,
                J4C3Add1 = j4C3Add1,
                J4C3Add2 = j4C3Add2,
                J4Crest4 = j4Crest4,
                J4C4Add1 = j4C4Add1,
                J4C4Add2 = j4C4Add2,
                Jewelry5 = jewelry5,
                J5Crest1 = j5Crest1,
                J5C1Add1 = j5C1Add1,
                J5C1Add2 = j5C1Add2,
                J5Crest2 = j5Crest2,
                J5C2Add1 = j5C2Add1,
                J5C2Add2 = j5C2Add2,
                J5Crest3 = j5Crest3,
                J5C3Add1 = j5C3Add1,
                J5C3Add2 = j5C3Add2,
                J5Crest4 = j5Crest4,
                J5C4Add1 = j5C4Add1,
                J5C4Add2 = j5C4Add2,
                Exp = exp,
                JobPoint = jobPoint,
                Strength = strength,
                DownPower = downPower,
                ShakePower = shakePower,
                StunPower = stunPower,
                Consitution = consitution,
                Guts = guts,
                FireResist = fireResist,
                IceResist = iceResist,
                ThunderResist = thunderResist,
                HolyResist = holyResist,
                DarkResist = darkResist,
                SpreadResist = spreadResist,
                FreezeResist = freezeResist,
                ShockResist = shockResist,
                AbsorbResist = absorbResist,
                DarkElmResist = darkElmResist,
                PoisonResist = poisonResist,
                SlowResist = slowResist,
                SleepResist = sleepResist,
                StunResist = stunResist,
                WetResist = wetResist,
                OilResist = oilResist,
                SealResist = sealResist,
                CurseResist = curseResist,
                SoftResist = softResist,
                StoneResist = stoneResist,
                GoldResist = goldResist,
                FireReduceResist = fireReduceResist,
                IceReduceResist = iceReduceResist,
                ThunderReduceResist = thunderReduceResist,
                HolyReduceResist = holyReduceResist,
                DarkReduceResist = darkReduceResist,
                AtkDownResist = atkDownResist,
                DefDownResist = defDownResist,
                MAtkDownResist = mAtkDownResist,
                MDefDownResist = mDefDownResist,
                NormalSkill1A = normalSkill1A,
                NormalSkill1B = normalSkill1B,
                NormalSkill2A = normalSkill2A,
                NormalSkill2B = normalSkill2B,
                NormalSkill3A = normalSkill3A,
                NormalSkill3B = normalSkill3B,
            };
        }
    }
}
