using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterDecideCharacterIdHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterDecideCharacterIdHandler));


        public CharacterDecideCharacterIdHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_CHARACTER_DECIDE_CHARACTER_ID_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            S2CCharacterDecideCharacterIdRes res = EntitySerializer.Get<S2CCharacterDecideCharacterIdRes>().Read(GameDump.data_Dump_13);
            res.CharacterId = client.Character.Id;
            res.CharacterInfo.CharacterId = client.Character.Id;
            res.CharacterInfo.FirstName = client.Character.FirstName;
            res.CharacterInfo.LastName = client.Character.LastName;
            res.CharacterInfo.EditInfo = client.Character.Visual;
            // TODO: StatusInfo
            res.CharacterInfo.Job = Server.AssetRepository.ArisenAsset[0].Job;
            res.CharacterInfo.CharacterJobDataList = new List<CDataCharacterJobData>() {
                new CDataCharacterJobData {
                    Job = Server.AssetRepository.ArisenAsset[0].Job,
                    Exp = Server.AssetRepository.ArisenAsset[0].Exp,
                    JobPoint = Server.AssetRepository.ArisenAsset[0].JobPoint,
                    Lv = Server.AssetRepository.ArisenAsset[0].Lv,
                    Atk = Server.AssetRepository.ArisenAsset[0].PAtk,
                    Def = Server.AssetRepository.ArisenAsset[0].PDef,
                    MAtk = Server.AssetRepository.ArisenAsset[0].MAtk,
                    MDef = Server.AssetRepository.ArisenAsset[0].MDef,
                    Strength = Server.AssetRepository.ArisenAsset[0].Strength,
                    DownPower = Server.AssetRepository.ArisenAsset[0].DownPower,
                    ShakePower = Server.AssetRepository.ArisenAsset[0].ShakePower,
                    StunPower = Server.AssetRepository.ArisenAsset[0].StunPower,
                    Consitution = Server.AssetRepository.ArisenAsset[0].Consitution,
                    Guts = Server.AssetRepository.ArisenAsset[0].Guts,
                    FireResist = Server.AssetRepository.ArisenAsset[0].FireResist,
                    IceResist = Server.AssetRepository.ArisenAsset[0].IceResist,
                    ThunderResist = Server.AssetRepository.ArisenAsset[0].ThunderResist,
                    HolyResist = Server.AssetRepository.ArisenAsset[0].HolyResist,
                    DarkResist = Server.AssetRepository.ArisenAsset[0].DarkResist,
                    SpreadResist = Server.AssetRepository.ArisenAsset[0].SpreadResist,
                    FreezeResist = Server.AssetRepository.ArisenAsset[0].FreezeResist,
                    ShockResist = Server.AssetRepository.ArisenAsset[0].ShockResist,
                    AbsorbResist = Server.AssetRepository.ArisenAsset[0].AbsorbResist,
                    DarkElmResist = Server.AssetRepository.ArisenAsset[0].DarkElmResist,
                    PoisonResist = Server.AssetRepository.ArisenAsset[0].PoisonResist,
                    SlowResist = Server.AssetRepository.ArisenAsset[0].SlowResist,
                    SleepResist = Server.AssetRepository.ArisenAsset[0].SleepResist,
                    StunResist = Server.AssetRepository.ArisenAsset[0].StunResist,
                    WetResist = Server.AssetRepository.ArisenAsset[0].WetResist,
                    OilResist = Server.AssetRepository.ArisenAsset[0].OilResist,
                    SealResist = Server.AssetRepository.ArisenAsset[0].SealResist,
                    CurseResist = Server.AssetRepository.ArisenAsset[0].CurseResist,
                    SoftResist = Server.AssetRepository.ArisenAsset[0].SoftResist,
                    StoneResist = Server.AssetRepository.ArisenAsset[0].StoneResist,
                    GoldResist = Server.AssetRepository.ArisenAsset[0].GoldResist,
                    FireReduceResist = Server.AssetRepository.ArisenAsset[0].FireReduceResist,
                    IceReduceResist = Server.AssetRepository.ArisenAsset[0].IceReduceResist,
                    ThunderReduceResist = Server.AssetRepository.ArisenAsset[0].ThunderReduceResist,
                    HolyReduceResist = Server.AssetRepository.ArisenAsset[0].HolyReduceResist,
                    DarkReduceResist = Server.AssetRepository.ArisenAsset[0].DarkReduceResist,
                    AtkDownResist = Server.AssetRepository.ArisenAsset[0].AtkDownResist,
                    DefDownResist = Server.AssetRepository.ArisenAsset[0].DefDownResist,
                    MAtkDownResist = Server.AssetRepository.ArisenAsset[0].MAtkDownResist,
                    MDefDownResist = Server.AssetRepository.ArisenAsset[0].MDefDownResist
                }
            };
            // TODO: PlayPointList
            res.CharacterInfo.CharacterEquipDataList = new List<CDataCharacterEquipData>() {
                new CDataCharacterEquipData {
                    Equips = new List<CDataEquipItemInfo>() {
                        new CDataEquipItemInfo {
                            U0 = Server.AssetRepository.ArisenAsset[0].PrimaryWeapon,
                            EquipType = 0,
                            EquipSlot = 1,
                            ItemId = 1,
                            Color = 6,
                            PlusValue = 0,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = Server.AssetRepository.ArisenAsset[0].PWCrest1,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].PWC1Add1 << 8 | Server.AssetRepository.ArisenAsset[0].PWC1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = Server.AssetRepository.ArisenAsset[0].PWCrest2,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].PWC2Add1 << 8 | Server.AssetRepository.ArisenAsset[0].PWC2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = Server.AssetRepository.ArisenAsset[0].PWCrest3,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].PWC3Add1 << 8 | Server.AssetRepository.ArisenAsset[0].PWC3Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 4,
                                    u1 = Server.AssetRepository.ArisenAsset[0].PWCrest4,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].PWC4Add1 << 8 | Server.AssetRepository.ArisenAsset[0].PWC4Add2),
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
                            U0 = Server.AssetRepository.ArisenAsset[0].SecondaryWeapon,
                            EquipType = 0,
                            EquipSlot = 1,
                            ItemId = 2
                        },
                        new CDataEquipItemInfo {
                            U0 = Server.AssetRepository.ArisenAsset[0].Head,
                            EquipType = 0,
                            EquipSlot = 1,
                            ItemId = 3,
                            Color = 0,
                            PlusValue = 3,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = Server.AssetRepository.ArisenAsset[0].HeadCrest1,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].HC1Add1 << 8 | Server.AssetRepository.ArisenAsset[0].HC1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = Server.AssetRepository.ArisenAsset[0].HeadCrest2,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].HC2Add1 << 8 | Server.AssetRepository.ArisenAsset[0].HC2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = Server.AssetRepository.ArisenAsset[0].HeadCrest3,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].HC3Add1 << 8 | Server.AssetRepository.ArisenAsset[0].HC3Add2),
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
                            U0 = Server.AssetRepository.ArisenAsset[0].Body,
                            EquipType = 0,
                            EquipSlot = 1,
                            ItemId = 4,
                            Color = 0,
                            PlusValue = 4,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = Server.AssetRepository.ArisenAsset[0].BodyCrest1,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].BC1Add1 << 8 | Server.AssetRepository.ArisenAsset[0].BC1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = Server.AssetRepository.ArisenAsset[0].BodyCrest2,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].BC2Add1 << 8 | Server.AssetRepository.ArisenAsset[0].BC2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = Server.AssetRepository.ArisenAsset[0].BodyCrest3,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].BC3Add1 << 8 | Server.AssetRepository.ArisenAsset[0].BC3Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 4,
                                    u1 = Server.AssetRepository.ArisenAsset[0].BodyCrest4,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].BC4Add1 << 8 | Server.AssetRepository.ArisenAsset[0].BC4Add2),
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
                            U0 = Server.AssetRepository.ArisenAsset[0].Clothing,
                            EquipType = 0,
                            EquipSlot = 1,
                            ItemId = 5
                        },
                        new CDataEquipItemInfo {
                            U0 = Server.AssetRepository.ArisenAsset[0].Arm,
                            EquipType = 0,
                            EquipSlot = 1,
                            ItemId = 6,
                            Color = 0,
                            PlusValue = 3,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = Server.AssetRepository.ArisenAsset[0].ArmCrest1,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].AC1Add1 << 8 | Server.AssetRepository.ArisenAsset[0].AC1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = Server.AssetRepository.ArisenAsset[0].ArmCrest2,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].AC2Add1 << 8 | Server.AssetRepository.ArisenAsset[0].AC2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = Server.AssetRepository.ArisenAsset[0].ArmCrest3,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].AC3Add1 << 8 | Server.AssetRepository.ArisenAsset[0].AC3Add2),
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
                            U0 = Server.AssetRepository.ArisenAsset[0].Leg,
                            EquipType = 0,
                            EquipSlot = 1,
                            ItemId = 7,
                            Color = 0,
                            PlusValue = 3,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = Server.AssetRepository.ArisenAsset[0].LegCrest1,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].LC1Add1 << 8 | Server.AssetRepository.ArisenAsset[0].LC1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = Server.AssetRepository.ArisenAsset[0].LegCrest2,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].LC2Add1 << 8 | Server.AssetRepository.ArisenAsset[0].LC2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = Server.AssetRepository.ArisenAsset[0].LegCrest3,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].LC3Add1 << 8 | Server.AssetRepository.ArisenAsset[0].LC3Add2),
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
                            U0 = Server.AssetRepository.ArisenAsset[0].Legwear,
                            EquipType = 0,
                            EquipSlot = 1,
                            ItemId = 8
                        },
                        new CDataEquipItemInfo {
                            U0 = Server.AssetRepository.ArisenAsset[0].Overwear,
                            EquipType = 0,
                            EquipSlot = 1,
                            ItemId = 9
                        },
                        new CDataEquipItemInfo {
                            U0 = Server.AssetRepository.ArisenAsset[0].Jewelry1,
                            EquipType = 0,
                            EquipSlot = 1,
                            ItemId = 10,
                            Color = 0,
                            PlusValue = 0,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = Server.AssetRepository.ArisenAsset[0].J1Crest1,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].J1C1Add1 << 8 | Server.AssetRepository.ArisenAsset[0].J1C1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = Server.AssetRepository.ArisenAsset[0].J1Crest2,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].J1C2Add1 << 8 | Server.AssetRepository.ArisenAsset[0].J1C2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = Server.AssetRepository.ArisenAsset[0].J1Crest3,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].J1C3Add1 << 8 | Server.AssetRepository.ArisenAsset[0].J1C3Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 4,
                                    u1 = Server.AssetRepository.ArisenAsset[0].J1Crest4,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].J1C4Add1 << 8 | Server.AssetRepository.ArisenAsset[0].J1C4Add2),
                                }                                
                            },
                            // Empty ArmorCrestDataList
                            EquipElementParamList = new List<CDataEquipElementParam>() {
                                new CDataEquipElementParam {
                                    u0 = 0x2,
                                    u2 = 0x02
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x3,
                                    u2 = 0x02
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x4,
                                    u2 = 0x02
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x5,
                                    u2 = 0x02
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x6,
                                    u2 = 0x50
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x7,
                                    u2 = 0x3C
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x8,
                                    u2 = 0x05
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x9,
                                    u2 = 0x07
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xA,
                                    u2 = 0x04
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xB,
                                    u2 = 0x04
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xC,
                                    u2 = 0x04
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xD,
                                    u2 = 0x04
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xE,
                                    u2 = 0x00
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xF,
                                    u2 = 0x05
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x10,
                                    u2 = 0x05
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x11,
                                    u2 = 0x05
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x12,
                                    u2 = 0x05
                                },
                            }
                        },
                        new CDataEquipItemInfo {
                            U0 = Server.AssetRepository.ArisenAsset[0].Jewelry2,
                            EquipType = 0,
                            EquipSlot = 1,
                            ItemId = 11,
                            Color = 0,
                            PlusValue = 0,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = Server.AssetRepository.ArisenAsset[0].J2Crest1,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].J2C1Add1 << 8 | Server.AssetRepository.ArisenAsset[0].J2C1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = Server.AssetRepository.ArisenAsset[0].J2Crest2,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].J2C2Add1 << 8 | Server.AssetRepository.ArisenAsset[0].J2C2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = Server.AssetRepository.ArisenAsset[0].J2Crest3,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].J2C3Add1 << 8 | Server.AssetRepository.ArisenAsset[0].J2C3Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 4,
                                    u1 = Server.AssetRepository.ArisenAsset[0].J2Crest4,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].J2C4Add1 << 8 | Server.AssetRepository.ArisenAsset[0].J2C4Add2),
                                }                                
                            },
                            // Empty ArmorCrestDataList
                            EquipElementParamList = new List<CDataEquipElementParam>() {
                                new CDataEquipElementParam {
                                    u0 = 0x2,
                                    u2 = 0x02
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x3,
                                    u2 = 0x02
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x4,
                                    u2 = 0x02
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x5,
                                    u2 = 0x02
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x6,
                                    u2 = 0x50
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x7,
                                    u2 = 0x3C
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x8,
                                    u2 = 0x05
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x9,
                                    u2 = 0x07
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xA,
                                    u2 = 0x04
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xB,
                                    u2 = 0x04
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xC,
                                    u2 = 0x04
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xD,
                                    u2 = 0x04
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xE,
                                    u2 = 0x00
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xF,
                                    u2 = 0x05
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x10,
                                    u2 = 0x05
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x11,
                                    u2 = 0x05
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x12,
                                    u2 = 0x05
                                },
                            }
                        },
                        new CDataEquipItemInfo {
                            U0 = Server.AssetRepository.ArisenAsset[0].Jewelry3,
                            EquipType = 0,
                            EquipSlot = 1,
                            ItemId = 12,
                            Color = 0,
                            PlusValue = 0,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = Server.AssetRepository.ArisenAsset[0].J3Crest1,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].J3C1Add1 << 8 | Server.AssetRepository.ArisenAsset[0].J3C1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = Server.AssetRepository.ArisenAsset[0].J3Crest2,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].J3C2Add1 << 8 | Server.AssetRepository.ArisenAsset[0].J3C2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = Server.AssetRepository.ArisenAsset[0].J3Crest3,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].J3C3Add1 << 8 | Server.AssetRepository.ArisenAsset[0].J3C3Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 4,
                                    u1 = Server.AssetRepository.ArisenAsset[0].J3Crest4,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].J3C4Add1 << 8 | Server.AssetRepository.ArisenAsset[0].J3C4Add2),
                                }                                
                            },
                            // Empty ArmorCrestDataList
                            EquipElementParamList = new List<CDataEquipElementParam>() {
                                new CDataEquipElementParam {
                                    u0 = 0x2,
                                    u2 = 0x02
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x3,
                                    u2 = 0x02
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x4,
                                    u2 = 0x02
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x5,
                                    u2 = 0x02
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x6,
                                    u2 = 0x50
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x7,
                                    u2 = 0x3C
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x8,
                                    u2 = 0x05
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x9,
                                    u2 = 0x07
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xA,
                                    u2 = 0x04
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xB,
                                    u2 = 0x04
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xC,
                                    u2 = 0x04
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xD,
                                    u2 = 0x04
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xE,
                                    u2 = 0x00
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xF,
                                    u2 = 0x05
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x10,
                                    u2 = 0x05
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x11,
                                    u2 = 0x05
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x12,
                                    u2 = 0x05
                                },
                            }
                        },
                        new CDataEquipItemInfo {
                            U0 = Server.AssetRepository.ArisenAsset[0].Jewelry4,
                            EquipType = 0,
                            EquipSlot = 1,
                            ItemId = 13,
                            Color = 0,
                            PlusValue = 0,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = Server.AssetRepository.ArisenAsset[0].J4Crest1,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].J4C1Add1 << 8 | Server.AssetRepository.ArisenAsset[0].J4C1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = Server.AssetRepository.ArisenAsset[0].J4Crest2,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].J4C2Add1 << 8 | Server.AssetRepository.ArisenAsset[0].J4C2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = Server.AssetRepository.ArisenAsset[0].J4Crest3,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].J4C3Add1 << 8 | Server.AssetRepository.ArisenAsset[0].J4C3Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 4,
                                    u1 = Server.AssetRepository.ArisenAsset[0].J4Crest4,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].J4C4Add1 << 8 | Server.AssetRepository.ArisenAsset[0].J4C4Add2),
                                }                                
                            },
                            // Empty ArmorCrestDataList
                            EquipElementParamList = new List<CDataEquipElementParam>() {
                                new CDataEquipElementParam {
                                    u0 = 0x2,
                                    u2 = 0x02
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x3,
                                    u2 = 0x02
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x4,
                                    u2 = 0x02
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x5,
                                    u2 = 0x02
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x6,
                                    u2 = 0x50
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x7,
                                    u2 = 0x3C
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x8,
                                    u2 = 0x05
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x9,
                                    u2 = 0x07
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xA,
                                    u2 = 0x04
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xB,
                                    u2 = 0x04
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xC,
                                    u2 = 0x04
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xD,
                                    u2 = 0x04
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xE,
                                    u2 = 0x00
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xF,
                                    u2 = 0x05
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x10,
                                    u2 = 0x05
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x11,
                                    u2 = 0x05
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x12,
                                    u2 = 0x05
                                },
                            }
                        },
                        new CDataEquipItemInfo {
                            U0 = Server.AssetRepository.ArisenAsset[0].Jewelry5,
                            EquipType = 0,
                            EquipSlot = 1,
                            ItemId = 14,
                            Color = 0,
                            PlusValue = 0,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = Server.AssetRepository.ArisenAsset[0].J5Crest1,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].J5C1Add1 << 8 | Server.AssetRepository.ArisenAsset[0].J5C1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = Server.AssetRepository.ArisenAsset[0].J5Crest2,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].J5C2Add1 << 8 | Server.AssetRepository.ArisenAsset[0].J5C2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = Server.AssetRepository.ArisenAsset[0].J5Crest3,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].J5C3Add1 << 8 | Server.AssetRepository.ArisenAsset[0].J5C3Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 4,
                                    u1 = Server.AssetRepository.ArisenAsset[0].J5Crest4,
                                    u2 = (ushort) (Server.AssetRepository.ArisenAsset[0].J5C4Add1 << 8 | Server.AssetRepository.ArisenAsset[0].J5C4Add2),
                                }                                
                            },
                            // Empty ArmorCrestDataList
                            EquipElementParamList = new List<CDataEquipElementParam>() {
                                new CDataEquipElementParam {
                                    u0 = 0x2,
                                    u2 = 0x02
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x3,
                                    u2 = 0x02
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x4,
                                    u2 = 0x02
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x5,
                                    u2 = 0x02
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x6,
                                    u2 = 0x50
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x7,
                                    u2 = 0x3C
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x8,
                                    u2 = 0x05
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x9,
                                    u2 = 0x07
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xA,
                                    u2 = 0x04
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xB,
                                    u2 = 0x04
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xC,
                                    u2 = 0x04
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xD,
                                    u2 = 0x04
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xE,
                                    u2 = 0x00
                                },
                                new CDataEquipElementParam {
                                    u0 = 0xF,
                                    u2 = 0x05
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x10,
                                    u2 = 0x05
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x11,
                                    u2 = 0x05
                                },
                                new CDataEquipElementParam {
                                    u0 = 0x12,
                                    u2 = 0x05
                                },
                            }
                        },
                        new CDataEquipItemInfo {
                            U0 = Server.AssetRepository.ArisenAsset[0].Lantern,
                            EquipType = 0,
                            EquipSlot = 1,
                            ItemId = 15
                        }
                    }
                }
            };
            res.CharacterInfo.CharacterEquipViewDataList = new List<CDataCharacterEquipData>() {
                new CDataCharacterEquipData {
                    Equips = new List<CDataEquipItemInfo>() {
                        new CDataEquipItemInfo {
                            U0 = Server.AssetRepository.ArisenAsset[0].VPrimaryWeapon,
                            EquipType = 0,
                            EquipSlot = 2,
                            ItemId = 1
                        },
                        new CDataEquipItemInfo {
                            U0 = Server.AssetRepository.ArisenAsset[0].VSecondaryWeapon,
                            EquipType = 0,
                            EquipSlot = 2,
                            ItemId = 2
                        },
                        new CDataEquipItemInfo {
                            U0 = Server.AssetRepository.ArisenAsset[0].VHead,
                            EquipType = 0,
                            EquipSlot = 2,
                            ItemId = 3
                        },
                        new CDataEquipItemInfo {
                            U0 = Server.AssetRepository.ArisenAsset[0].VBody,
                            EquipType = 0,
                            EquipSlot = 2,
                            ItemId = 4
                        },
                        new CDataEquipItemInfo {
                            U0 = Server.AssetRepository.ArisenAsset[0].VClothing,
                            EquipType = 0,
                            EquipSlot = 2,
                            ItemId = 5
                        },
                        new CDataEquipItemInfo {
                            U0 = Server.AssetRepository.ArisenAsset[0].VArm,
                            EquipType = 0,
                            EquipSlot = 2,
                            ItemId = 6
                        },
                        new CDataEquipItemInfo {
                            U0 = Server.AssetRepository.ArisenAsset[0].VLeg,
                            EquipType = 0,
                            EquipSlot = 2,
                            ItemId = 7
                        },
                        new CDataEquipItemInfo {
                            U0 = Server.AssetRepository.ArisenAsset[0].VLegwear,
                            EquipType = 0,
                            EquipSlot = 2,
                            ItemId = 8
                        },
                        new CDataEquipItemInfo {
                            U0 = Server.AssetRepository.ArisenAsset[0].VOverwear,
                            EquipType = 0,
                            EquipSlot = 2,
                            ItemId = 9
                        }
                    }
                }
            };
            res.CharacterInfo.CharacterEquipJobItemList = new List<CDataEquipJobItem>() {
                new CDataEquipJobItem {
                    JobItemId = Server.AssetRepository.ArisenAsset[0].ClassItem1,
                    EquipSlotNo = 1
                },
                new CDataEquipJobItem {
                    JobItemId = Server.AssetRepository.ArisenAsset[0].ClassItem2,
                    EquipSlotNo = 2
                }
            };
            // Plenty of things to do in between
            res.CharacterInfo.HideEquipHead = Server.AssetRepository.ArisenAsset[0].DisplayHelmet;
            res.CharacterInfo.HideEquipLantern = Server.AssetRepository.ArisenAsset[0].DisplayLantern;
            res.CharacterInfo.HideEquipHeadPawn = Server.AssetRepository.MyPawnAsset[0].HideEquipHead;
            res.CharacterInfo.HideEquipLanternPawn = Server.AssetRepository.MyPawnAsset[0].HideEquipLantern;
            // Some more data to do
            client.Send(res);
            
            S2CCharacterContentsReleaseElementNotice contentsReleaseElementNotice = EntitySerializer.Get<S2CCharacterContentsReleaseElementNotice>().Read(GameFull.data_Dump_20);
            client.Send(contentsReleaseElementNotice);
        }
    }
}
