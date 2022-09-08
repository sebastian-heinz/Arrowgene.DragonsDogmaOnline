using System.Linq;
using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.LoginServer.Dump;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class CreateCharacterHandler : StructurePacketHandler<LoginClient, C2LCreateCharacterDataReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CreateCharacterHandler));


        public CreateCharacterHandler(DdonLoginServer server) : base(server)
        {
        }

        public override void Handle(LoginClient client, StructurePacket<C2LCreateCharacterDataReq> packet)
        {
            Logger.Debug(client, $"Created character '{packet.Structure.CharacterInfo.FirstName} {packet.Structure.CharacterInfo.LastName}'");

            Character character = new Character();
            character.AccountId = client.Account.Id;
            character.CharacterInfo = packet.Structure.CharacterInfo;

            // Use the ArisenCsv row for the selected job as the preset equipment when the character is created
            ArisenCsv ArisenPreset = Server.AssetRepository.ArisenAsset.Where(x => x.Job == character.CharacterInfo.Job).Single();
            S2CCharacterDecideCharacterIdRes pcapCharacter = EntitySerializer.Get<S2CCharacterDecideCharacterIdRes>().Read(LoginDump.game_dump_data_Dump_13);
            // PlayPointList will be empty when a character is created
            character.CharacterInfo.StatusInfo = new CDataStatusInfo() {
                HP = ArisenPreset.HP,
                Stamina = ArisenPreset.Stamina,
                RevivePoint = ArisenPreset.RevivePoint,
                MaxHP = ArisenPreset.MaxHP,
                MaxStamina = ArisenPreset.MaxStamina,
                WhiteHP = ArisenPreset.WhiteHP,
                GainHP = ArisenPreset.GainHP,
                GainStamina = ArisenPreset.GainStamina,
                GainAttack = ArisenPreset.GainAttack,
                GainDefense = ArisenPreset.GainDefense,
                GainMagicAttack = ArisenPreset.GainMagicAttack,
                GainMagicDefense = ArisenPreset.GainMagicDefense
            };
            character.CharacterInfo.CharacterJobDataList = new List<CDataCharacterJobData>() {
                new CDataCharacterJobData {
                    Job = ArisenPreset.Job,
                    Exp = ArisenPreset.Exp,
                    JobPoint = ArisenPreset.JobPoint,
                    Lv = ArisenPreset.Lv,
                    Atk = ArisenPreset.PAtk,
                    Def = ArisenPreset.PDef,
                    MAtk = ArisenPreset.MAtk,
                    MDef = ArisenPreset.MDef,
                    Strength = ArisenPreset.Strength,
                    DownPower = ArisenPreset.DownPower,
                    ShakePower = ArisenPreset.ShakePower,
                    StunPower = ArisenPreset.StunPower,
                    Consitution = ArisenPreset.Consitution,
                    Guts = ArisenPreset.Guts,
                    FireResist = ArisenPreset.FireResist,
                    IceResist = ArisenPreset.IceResist,
                    ThunderResist = ArisenPreset.ThunderResist,
                    HolyResist = ArisenPreset.HolyResist,
                    DarkResist = ArisenPreset.DarkResist,
                    SpreadResist = ArisenPreset.SpreadResist,
                    FreezeResist = ArisenPreset.FreezeResist,
                    ShockResist = ArisenPreset.ShockResist,
                    AbsorbResist = ArisenPreset.AbsorbResist,
                    DarkElmResist = ArisenPreset.DarkElmResist,
                    PoisonResist = ArisenPreset.PoisonResist,
                    SlowResist = ArisenPreset.SlowResist,
                    SleepResist = ArisenPreset.SleepResist,
                    StunResist = ArisenPreset.StunResist,
                    WetResist = ArisenPreset.WetResist,
                    OilResist = ArisenPreset.OilResist,
                    SealResist = ArisenPreset.SealResist,
                    CurseResist = ArisenPreset.CurseResist,
                    SoftResist = ArisenPreset.SoftResist,
                    StoneResist = ArisenPreset.StoneResist,
                    GoldResist = ArisenPreset.GoldResist,
                    FireReduceResist = ArisenPreset.FireReduceResist,
                    IceReduceResist = ArisenPreset.IceReduceResist,
                    ThunderReduceResist = ArisenPreset.ThunderReduceResist,
                    HolyReduceResist = ArisenPreset.HolyReduceResist,
                    DarkReduceResist = ArisenPreset.DarkReduceResist,
                    AtkDownResist = ArisenPreset.AtkDownResist,
                    DefDownResist = ArisenPreset.DefDownResist,
                    MAtkDownResist = ArisenPreset.MAtkDownResist,
                    MDefDownResist = ArisenPreset.MDefDownResist
                }
            };
            character.CharacterInfo.CharacterEquipDataList = new List<CDataCharacterEquipData>() {
                new CDataCharacterEquipData {
                    Equips = new List<CDataEquipItemInfo>() {
                        new CDataEquipItemInfo {
                            ItemId = ArisenPreset.PrimaryWeapon,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 1,
                            Color = ArisenPreset.PrimaryWeaponColour,
                            PlusValue = 0,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = ArisenPreset.PWCrest1,
                                    u2 = (ushort) (ArisenPreset.PWC1Add1 << 8 | ArisenPreset.PWC1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = ArisenPreset.PWCrest2,
                                    u2 = (ushort) (ArisenPreset.PWC2Add1 << 8 | ArisenPreset.PWC2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = ArisenPreset.PWCrest3,
                                    u2 = (ushort) (ArisenPreset.PWC3Add1 << 8 | ArisenPreset.PWC3Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 4,
                                    u1 = ArisenPreset.PWCrest4,
                                    u2 = (ushort) (ArisenPreset.PWC4Add1 << 8 | ArisenPreset.PWC4Add2),
                                }
                            },
                            ArmorCrestDataList = new List<CDataArmorCrestData>() {
                                new CDataArmorCrestData {
                                    u0 = 1,
                                    u1 = 1,
                                    u2 = 0x59,
                                    u3 = 0x04
                                }
                            },
                            // Empty EquipElementParamList
                        },
                        new CDataEquipItemInfo {
                            ItemId = ArisenPreset.SecondaryWeapon,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 2,
                            Color = ArisenPreset.SecondaryWeaponColour
                        },
                        new CDataEquipItemInfo {
                            ItemId = ArisenPreset.Head,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 3,
                            Color = ArisenPreset.HeadColour,
                            PlusValue = 3,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = ArisenPreset.HeadCrest1,
                                    u2 = (ushort) (ArisenPreset.HC1Add1 << 8 | ArisenPreset.HC1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = ArisenPreset.HeadCrest2,
                                    u2 = (ushort) (ArisenPreset.HC2Add1 << 8 | ArisenPreset.HC2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = ArisenPreset.HeadCrest3,
                                    u2 = (ushort) (ArisenPreset.HC3Add1 << 8 | ArisenPreset.HC3Add2),
                                }
                            },
                            ArmorCrestDataList = new List<CDataArmorCrestData>() {
                                new CDataArmorCrestData {
                                    u0 = 1,
                                    u1 = 1,
                                    u2 = 0x29D,
                                    u3 = 0x01
                                }
                            },
                            // Empty EquipElementParamList
                        },
                        new CDataEquipItemInfo {
                            ItemId = ArisenPreset.Body,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 4,
                            Color = ArisenPreset.BodyColour,
                            PlusValue = 4,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = ArisenPreset.BodyCrest1,
                                    u2 = (ushort) (ArisenPreset.BC1Add1 << 8 | ArisenPreset.BC1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = ArisenPreset.BodyCrest2,
                                    u2 = (ushort) (ArisenPreset.BC2Add1 << 8 | ArisenPreset.BC2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = ArisenPreset.BodyCrest3,
                                    u2 = (ushort) (ArisenPreset.BC3Add1 << 8 | ArisenPreset.BC3Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 4,
                                    u1 = ArisenPreset.BodyCrest4,
                                    u2 = (ushort) (ArisenPreset.BC4Add1 << 8 | ArisenPreset.BC4Add2),
                                }                                
                            },
                            ArmorCrestDataList = new List<CDataArmorCrestData>() {
                                new CDataArmorCrestData {
                                    u0 = 1,
                                    u1 = 1,
                                    u2 = 0x280,
                                    u3 = 0x01
                                }
                            },
                            // Empty EquipElementParamList
                        },
                        new CDataEquipItemInfo {
                            ItemId = ArisenPreset.Clothing,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 5,
                            Color = ArisenPreset.ClothingColour
                        },
                        new CDataEquipItemInfo {
                            ItemId = ArisenPreset.Arm,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 6,
                            Color = ArisenPreset.ArmColour,
                            PlusValue = 3,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = ArisenPreset.ArmCrest1,
                                    u2 = (ushort) (ArisenPreset.AC1Add1 << 8 | ArisenPreset.AC1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = ArisenPreset.ArmCrest2,
                                    u2 = (ushort) (ArisenPreset.AC2Add1 << 8 | ArisenPreset.AC2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = ArisenPreset.ArmCrest3,
                                    u2 = (ushort) (ArisenPreset.AC3Add1 << 8 | ArisenPreset.AC3Add2),
                                }
                            },
                            ArmorCrestDataList = new List<CDataArmorCrestData>() {
                                new CDataArmorCrestData {
                                    u0 = 1,
                                    u1 = 1,
                                    u2 = 0x1D2,
                                    u3 = 0x01
                                }
                            },
                            // Empty EquipElementParamList
                        },
                        new CDataEquipItemInfo {
                            ItemId = ArisenPreset.Leg,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 7,
                            Color = ArisenPreset.LegColour,
                            PlusValue = 3,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = ArisenPreset.LegCrest1,
                                    u2 = (ushort) (ArisenPreset.LC1Add1 << 8 | ArisenPreset.LC1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = ArisenPreset.LegCrest2,
                                    u2 = (ushort) (ArisenPreset.LC2Add1 << 8 | ArisenPreset.LC2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = ArisenPreset.LegCrest3,
                                    u2 = (ushort) (ArisenPreset.LC3Add1 << 8 | ArisenPreset.LC3Add2),
                                }
                            },
                            ArmorCrestDataList = new List<CDataArmorCrestData>() {
                                new CDataArmorCrestData {
                                    u0 = 1,
                                    u1 = 1,
                                    u2 = 0x225,
                                    u3 = 0x01
                                }
                            },
                            // Empty EquipElementParamList
                        },
                        new CDataEquipItemInfo {
                            ItemId = ArisenPreset.Legwear,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 8,
                            Color = ArisenPreset.LegwearColour
                        },
                        new CDataEquipItemInfo {
                            ItemId = ArisenPreset.Overwear,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 9,
                            Color = ArisenPreset.OverwearColour
                        },
                        new CDataEquipItemInfo {
                            ItemId = ArisenPreset.Jewelry1,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 10,
                            Color = 0,
                            PlusValue = 0,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = ArisenPreset.J1Crest1,
                                    u2 = (ushort) (ArisenPreset.J1C1Add1 << 8 | ArisenPreset.J1C1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = ArisenPreset.J1Crest2,
                                    u2 = (ushort) (ArisenPreset.J1C2Add1 << 8 | ArisenPreset.J1C2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = ArisenPreset.J1Crest3,
                                    u2 = (ushort) (ArisenPreset.J1C3Add1 << 8 | ArisenPreset.J1C3Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 4,
                                    u1 = ArisenPreset.J1Crest4,
                                    u2 = (ushort) (ArisenPreset.J1C4Add1 << 8 | ArisenPreset.J1C4Add2),
                                }                                
                            },
                            // Empty ArmorCrestDataList
                            EquipElementParamList = new List<CDataEquipElementParam>() {
                                new CDataEquipElementParam {
                                    SlotNo = 0x2,
                                    ItemId = 0x02
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x3,
                                    ItemId = 0x02
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x4,
                                    ItemId = 0x02
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x5,
                                    ItemId = 0x02
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x6,
                                    ItemId = 0x50
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x7,
                                    ItemId = 0x3C
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x8,
                                    ItemId = 0x05
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x9,
                                    ItemId = 0x07
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xA,
                                    ItemId = 0x04
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xB,
                                    ItemId = 0x04
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xC,
                                    ItemId = 0x04
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xD,
                                    ItemId = 0x04
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xE,
                                    ItemId = 0x00
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xF,
                                    ItemId = 0x05
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x10,
                                    ItemId = 0x05
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x11,
                                    ItemId = 0x05
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x12,
                                    ItemId = 0x05
                                },
                            }
                        },
                        new CDataEquipItemInfo {
                            ItemId = ArisenPreset.Jewelry2,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 11,
                            Color = 0,
                            PlusValue = 0,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = ArisenPreset.J2Crest1,
                                    u2 = (ushort) (ArisenPreset.J2C1Add1 << 8 | ArisenPreset.J2C1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = ArisenPreset.J2Crest2,
                                    u2 = (ushort) (ArisenPreset.J2C2Add1 << 8 | ArisenPreset.J2C2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = ArisenPreset.J2Crest3,
                                    u2 = (ushort) (ArisenPreset.J2C3Add1 << 8 | ArisenPreset.J2C3Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 4,
                                    u1 = ArisenPreset.J2Crest4,
                                    u2 = (ushort) (ArisenPreset.J2C4Add1 << 8 | ArisenPreset.J2C4Add2),
                                }                                
                            },
                            // Empty ArmorCrestDataList
                            EquipElementParamList = new List<CDataEquipElementParam>() {
                                new CDataEquipElementParam {
                                    SlotNo = 0x2,
                                    ItemId = 0x02
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x3,
                                    ItemId = 0x02
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x4,
                                    ItemId = 0x02
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x5,
                                    ItemId = 0x02
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x6,
                                    ItemId = 0x50
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x7,
                                    ItemId = 0x3C
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x8,
                                    ItemId = 0x05
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x9,
                                    ItemId = 0x07
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xA,
                                    ItemId = 0x04
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xB,
                                    ItemId = 0x04
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xC,
                                    ItemId = 0x04
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xD,
                                    ItemId = 0x04
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xE,
                                    ItemId = 0x00
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xF,
                                    ItemId = 0x05
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x10,
                                    ItemId = 0x05
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x11,
                                    ItemId = 0x05
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x12,
                                    ItemId = 0x05
                                },
                            }
                        },
                        new CDataEquipItemInfo {
                            ItemId = ArisenPreset.Jewelry3,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 12,
                            Color = 0,
                            PlusValue = 0,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = ArisenPreset.J3Crest1,
                                    u2 = (ushort) (ArisenPreset.J3C1Add1 << 8 | ArisenPreset.J3C1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = ArisenPreset.J3Crest2,
                                    u2 = (ushort) (ArisenPreset.J3C2Add1 << 8 | ArisenPreset.J3C2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = ArisenPreset.J3Crest3,
                                    u2 = (ushort) (ArisenPreset.J3C3Add1 << 8 | ArisenPreset.J3C3Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 4,
                                    u1 = ArisenPreset.J3Crest4,
                                    u2 = (ushort) (ArisenPreset.J3C4Add1 << 8 | ArisenPreset.J3C4Add2),
                                }                                
                            },
                            // Empty ArmorCrestDataList
                            EquipElementParamList = new List<CDataEquipElementParam>() {
                                new CDataEquipElementParam {
                                    SlotNo = 0x2,
                                    ItemId = 0x02
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x3,
                                    ItemId = 0x02
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x4,
                                    ItemId = 0x02
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x5,
                                    ItemId = 0x02
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x6,
                                    ItemId = 0x50
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x7,
                                    ItemId = 0x3C
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x8,
                                    ItemId = 0x05
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x9,
                                    ItemId = 0x07
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xA,
                                    ItemId = 0x04
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xB,
                                    ItemId = 0x04
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xC,
                                    ItemId = 0x04
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xD,
                                    ItemId = 0x04
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xE,
                                    ItemId = 0x00
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xF,
                                    ItemId = 0x05
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x10,
                                    ItemId = 0x05
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x11,
                                    ItemId = 0x05
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x12,
                                    ItemId = 0x05
                                },
                            }
                        },
                        new CDataEquipItemInfo {
                            ItemId = ArisenPreset.Jewelry4,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 13,
                            Color = 0,
                            PlusValue = 0,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = ArisenPreset.J4Crest1,
                                    u2 = (ushort) (ArisenPreset.J4C1Add1 << 8 | ArisenPreset.J4C1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = ArisenPreset.J4Crest2,
                                    u2 = (ushort) (ArisenPreset.J4C2Add1 << 8 | ArisenPreset.J4C2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = ArisenPreset.J4Crest3,
                                    u2 = (ushort) (ArisenPreset.J4C3Add1 << 8 | ArisenPreset.J4C3Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 4,
                                    u1 = ArisenPreset.J4Crest4,
                                    u2 = (ushort) (ArisenPreset.J4C4Add1 << 8 | ArisenPreset.J4C4Add2),
                                }                                
                            },
                            // Empty ArmorCrestDataList
                            EquipElementParamList = new List<CDataEquipElementParam>() {
                                new CDataEquipElementParam {
                                    SlotNo = 0x2,
                                    ItemId = 0x02
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x3,
                                    ItemId = 0x02
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x4,
                                    ItemId = 0x02
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x5,
                                    ItemId = 0x02
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x6,
                                    ItemId = 0x50
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x7,
                                    ItemId = 0x3C
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x8,
                                    ItemId = 0x05
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x9,
                                    ItemId = 0x07
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xA,
                                    ItemId = 0x04
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xB,
                                    ItemId = 0x04
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xC,
                                    ItemId = 0x04
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xD,
                                    ItemId = 0x04
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xE,
                                    ItemId = 0x00
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xF,
                                    ItemId = 0x05
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x10,
                                    ItemId = 0x05
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x11,
                                    ItemId = 0x05
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x12,
                                    ItemId = 0x05
                                },
                            }
                        },
                        new CDataEquipItemInfo {
                            ItemId = ArisenPreset.Jewelry5,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 14,
                            Color = 0,
                            PlusValue = 0,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = ArisenPreset.J5Crest1,
                                    u2 = (ushort) (ArisenPreset.J5C1Add1 << 8 | ArisenPreset.J5C1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = ArisenPreset.J5Crest2,
                                    u2 = (ushort) (ArisenPreset.J5C2Add1 << 8 | ArisenPreset.J5C2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = ArisenPreset.J5Crest3,
                                    u2 = (ushort) (ArisenPreset.J5C3Add1 << 8 | ArisenPreset.J5C3Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 4,
                                    u1 = ArisenPreset.J5Crest4,
                                    u2 = (ushort) (ArisenPreset.J5C4Add1 << 8 | ArisenPreset.J5C4Add2),
                                }                                
                            },
                            // Empty ArmorCrestDataList
                            EquipElementParamList = new List<CDataEquipElementParam>() {
                                new CDataEquipElementParam {
                                    SlotNo = 0x2,
                                    ItemId = 0x02
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x3,
                                    ItemId = 0x02
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x4,
                                    ItemId = 0x02
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x5,
                                    ItemId = 0x02
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x6,
                                    ItemId = 0x50
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x7,
                                    ItemId = 0x3C
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x8,
                                    ItemId = 0x05
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x9,
                                    ItemId = 0x07
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xA,
                                    ItemId = 0x04
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xB,
                                    ItemId = 0x04
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xC,
                                    ItemId = 0x04
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xD,
                                    ItemId = 0x04
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xE,
                                    ItemId = 0x00
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0xF,
                                    ItemId = 0x05
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x10,
                                    ItemId = 0x05
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x11,
                                    ItemId = 0x05
                                },
                                new CDataEquipElementParam {
                                    SlotNo = 0x12,
                                    ItemId = 0x05
                                },
                            }
                        },
                        new CDataEquipItemInfo {
                            ItemId = ArisenPreset.Lantern,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 15
                        }
                    }
                }
            };
            character.CharacterInfo.CharacterEquipViewDataList = new List<CDataCharacterEquipData>() {
                new CDataCharacterEquipData {
                    Equips = new List<CDataEquipItemInfo>() {
                        new CDataEquipItemInfo {
                            ItemId = ArisenPreset.VPrimaryWeapon,
                            Unk0 = 0,
                            EquipType = 2,
                            EquipSlot = 1,
                            Color = ArisenPreset.VPrimaryWeaponColour
                        },
                        new CDataEquipItemInfo {
                            ItemId = ArisenPreset.VSecondaryWeapon,
                            Unk0 = 0,
                            EquipType = 2,
                            EquipSlot = 2,
                            Color = ArisenPreset.VSecondaryWeaponColour
                        },
                        new CDataEquipItemInfo {
                            ItemId = ArisenPreset.VHead,
                            Unk0 = 0,
                            EquipType = 2,
                            EquipSlot = 3,
                            Color = ArisenPreset.VHeadColour
                        },
                        new CDataEquipItemInfo {
                            ItemId = ArisenPreset.VBody,
                            Unk0 = 0,
                            EquipType = 2,
                            EquipSlot = 4,
                            Color = ArisenPreset.VBodyColour
                        },
                        new CDataEquipItemInfo {
                            ItemId = ArisenPreset.VClothing,
                            Unk0 = 0,
                            EquipType = 2,
                            EquipSlot = 5,
                            Color = ArisenPreset.VClothingColour
                        },
                        new CDataEquipItemInfo {
                            ItemId = ArisenPreset.VArm,
                            Unk0 = 0,
                            EquipType = 2,
                            EquipSlot = 6,
                            Color = ArisenPreset.VArmColour
                        },
                        new CDataEquipItemInfo {
                            ItemId = ArisenPreset.VLeg,
                            Unk0 = 0,
                            EquipType = 2,
                            EquipSlot = 7,
                            Color = ArisenPreset.VLegColour
                        },
                        new CDataEquipItemInfo {
                            ItemId = ArisenPreset.VLegwear,
                            Unk0 = 0,
                            EquipType = 2,
                            EquipSlot = 8,
                            Color = ArisenPreset.VLegwearColour
                        },
                        new CDataEquipItemInfo {
                            ItemId = ArisenPreset.VOverwear,
                            Unk0 = 0,
                            EquipType = 2,
                            EquipSlot = 9,
                            Color = ArisenPreset.VOverwearColour,
                        }
                    }
                }
            };
            character.CharacterInfo.CharacterEquipJobItemList = new List<CDataEquipJobItem>() {
                new CDataEquipJobItem {
                    JobItemId = ArisenPreset.ClassItem1,
                    EquipSlotNo = 1
                },
                new CDataEquipJobItem {
                    JobItemId = ArisenPreset.ClassItem2,
                    EquipSlotNo = 2
                }
            };
            character.CharacterInfo.HideEquipHead = ArisenPreset.DisplayHelmet;
            character.CharacterInfo.HideEquipLantern = ArisenPreset.DisplayLantern;
            character.CharacterInfo.HideEquipHeadPawn = Server.AssetRepository.MyPawnAsset[0].HideEquipHead;
            character.CharacterInfo.HideEquipLanternPawn = Server.AssetRepository.MyPawnAsset[0].HideEquipLantern;
            // TODO: Load from Arisen.csv or something
            character.NormalSkills = new List<CDataNormalSkillParam>() {
                    new CDataNormalSkillParam() {
                    Job = ArisenPreset.Job,
                    SkillNo = 1,
                    Index = 0,
                    PreSkillNo = 0
                },
                new CDataNormalSkillParam() {
                    Job = ArisenPreset.Job,
                    SkillNo = 2,
                    Index = 0,
                    PreSkillNo = 0
                },
                new CDataNormalSkillParam() {
                    Job = ArisenPreset.Job,
                    SkillNo = 3,
                    Index = 0,
                    PreSkillNo = 0
                }
            };
            character.CustomSkills = new List<CDataSetAcquirementParam>() {
                // Main Palette
                new CDataSetAcquirementParam() {
                    Job = ArisenPreset.Job,
                    Type = 0,
                    SlotNo = 1,
                    AcquirementNo = ArisenPreset.Cs1MpId,
                    AcquirementLv = ArisenPreset.Cs1MpLv
                },
                new CDataSetAcquirementParam() {
                    Job = ArisenPreset.Job,
                    Type = 0,
                    SlotNo = 2,
                    AcquirementNo = ArisenPreset.Cs2MpId,
                    AcquirementLv = ArisenPreset.Cs2MpLv
                },
                new CDataSetAcquirementParam() {
                    Job = ArisenPreset.Job,
                    Type = 0,
                    SlotNo = 3,
                    AcquirementNo = ArisenPreset.Cs3MpId,
                    AcquirementLv = ArisenPreset.Cs3MpLv
                },
                new CDataSetAcquirementParam() {
                    Job = ArisenPreset.Job,
                    Type = 0,
                    SlotNo = 4,
                    AcquirementNo = ArisenPreset.Cs4MpId,
                    AcquirementLv = ArisenPreset.Cs4MpLv
                },
                // Sub Palette
                new CDataSetAcquirementParam() {
                    Job = ArisenPreset.Job,
                    Type = 0,
                    SlotNo = (1<<4) | 1,
                    AcquirementNo = ArisenPreset.Cs1SpId,
                    AcquirementLv = ArisenPreset.Cs1SpLv
                },
                new CDataSetAcquirementParam() {
                    Job = ArisenPreset.Job,
                    Type = 0,
                    SlotNo = (1<<4) | 2,
                    AcquirementNo = ArisenPreset.Cs2SpId,
                    AcquirementLv = ArisenPreset.Cs2SpLv
                },
                new CDataSetAcquirementParam() {
                    Job = ArisenPreset.Job,
                    Type = 0,
                    SlotNo = (1<<4) | 3,
                    AcquirementNo = ArisenPreset.Cs3SpId,
                    AcquirementLv = ArisenPreset.Cs3SpLv
                },
                new CDataSetAcquirementParam() {
                    Job = ArisenPreset.Job,
                    Type = 0,
                    SlotNo = (1<<4) | 4,
                    AcquirementNo = ArisenPreset.Cs4SpId,
                    AcquirementLv = ArisenPreset.Cs4SpLv
                }
            };
            character.Abilities = new List<CDataSetAcquirementParam>() {
                new CDataSetAcquirementParam() {
                    Job = ArisenPreset.Ab1Jb,
                    Type = 0,
                    SlotNo = 1,
                    AcquirementNo = ArisenPreset.Ab1Id,
                    AcquirementLv = ArisenPreset.Ab1Lv
                },
                new CDataSetAcquirementParam() {
                    Job = ArisenPreset.Ab2Jb,
                    Type = 0,
                    SlotNo = 2,
                    AcquirementNo = ArisenPreset.Ab2Id,
                    AcquirementLv = ArisenPreset.Ab2Lv
                },
                new CDataSetAcquirementParam() {
                    Job = ArisenPreset.Ab3Jb,
                    Type = 0,
                    SlotNo = 3,
                    AcquirementNo = ArisenPreset.Ab3Id,
                    AcquirementLv = ArisenPreset.Ab3Lv
                },
                new CDataSetAcquirementParam() {
                    Job = ArisenPreset.Ab4Jb,
                    Type = 0,
                    SlotNo = 4,
                    AcquirementNo = ArisenPreset.Ab4Id,
                    AcquirementLv = ArisenPreset.Ab4Lv
                },
                new CDataSetAcquirementParam() {
                    Job = ArisenPreset.Ab5Jb,
                    Type = 0,
                    SlotNo = 5,
                    AcquirementNo = ArisenPreset.Ab5Id,
                    AcquirementLv = ArisenPreset.Ab5Lv
                },
                new CDataSetAcquirementParam() {
                    Job = ArisenPreset.Ab6Jb,
                    Type = 0,
                    SlotNo = 6,
                    AcquirementNo = ArisenPreset.Ab6Id,
                    AcquirementLv = ArisenPreset.Ab6Lv
                },
                new CDataSetAcquirementParam() {
                    Job = ArisenPreset.Ab7Jb,
                    Type = 0,
                    SlotNo = 7,
                    AcquirementNo = ArisenPreset.Ab7Id,
                    AcquirementLv = ArisenPreset.Ab7Lv
                },
                new CDataSetAcquirementParam() {
                    Job = ArisenPreset.Ab8Jb,
                    Type = 0,
                    SlotNo = 8,
                    AcquirementNo = ArisenPreset.Ab8Id,
                    AcquirementLv = ArisenPreset.Ab8Lv
                },
                new CDataSetAcquirementParam() {
                    Job = ArisenPreset.Ab9Jb,
                    Type = 0,
                    SlotNo = 9,
                    AcquirementNo = ArisenPreset.Ab9Id,
                    AcquirementLv = ArisenPreset.Ab9Lv
                },
                new CDataSetAcquirementParam() {
                    Job = ArisenPreset.Ab10Jb,
                    Type = 0,
                    SlotNo = 10,
                    AcquirementNo = ArisenPreset.Ab10Id,
                    AcquirementLv = ArisenPreset.Ab10Lv
                }
            };

            L2CCreateCharacterDataRes res = new L2CCreateCharacterDataRes();
            if (!Database.CreateCharacter(character))
            {
                Logger.Error(client, "Failed to create character");
                res.Result = 1;
                client.Send(res);
            }
            
            L2CCreateCharacterDataNtc ntc = new L2CCreateCharacterDataNtc();
            ntc.Result = character.Id; // Value will show up in DecideCharacterIdHandler as CharacterId
            client.Send(ntc);

            // Sent to client once the player queue "WaitNum" above is 0,
            // send immediately in our case.

            res.Result = 0;
            res.WaitNum = 0;
            client.Send(res);
        }
    }
}
