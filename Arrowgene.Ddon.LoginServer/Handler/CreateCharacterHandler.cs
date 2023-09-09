using System.Security.AccessControl;
using System;
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
            character.CharacterId = packet.Structure.CharacterInfo.CharacterId;
            character.UserId = packet.Structure.CharacterInfo.UserId;
            character.Version = packet.Structure.CharacterInfo.Version;
            character.FirstName = packet.Structure.CharacterInfo.FirstName;
            character.LastName = packet.Structure.CharacterInfo.LastName;
            character.EditInfo = packet.Structure.CharacterInfo.EditInfo;
            character.StatusInfo = packet.Structure.CharacterInfo.StatusInfo;
            character.Job = packet.Structure.CharacterInfo.Job;
            character.CharacterJobDataList = packet.Structure.CharacterInfo.CharacterJobDataList;
            character.PlayPointList = packet.Structure.CharacterInfo.PlayPointList;
            character.Equipment = new Equipment(
                new Dictionary<JobId, Dictionary<EquipType, List<Item>>>()
                {
                    {
                        packet.Structure.CharacterInfo.Job,
                        new Dictionary<EquipType, List<Item>>() {
                            {
                                EquipType.Performance,
                                Enumerable.Range(1, 15)
                                    .Select(equipSlot => {
                                        CDataEquipItemInfo info = packet.Structure.CharacterInfo.CharacterEquipDataList.SelectMany(x => x.Equips).Where(x => x.EquipSlot == equipSlot).SingleOrDefault();
                                        if(info == null) {
                                            return null;
                                        } else {
                                            return new Item()
                                            {
                                                ItemId = info.ItemId,
                                                Unk3 = info.Unk0,
                                                Color = info.Color,
                                                PlusValue = info.PlusValue,
                                                WeaponCrestDataList = info.WeaponCrestDataList,
                                                ArmorCrestDataList = info.ArmorCrestDataList,
                                                EquipElementParamList = info.EquipElementParamList
                                            };
                                        }
                                    })
                                    .ToList()
                            },
                            {
                                EquipType.Visual,
                                Enumerable.Range(1, 15)
                                    .Select(equipSlot => {
                                        CDataEquipItemInfo info = packet.Structure.CharacterInfo.CharacterEquipViewDataList.SelectMany(x => x.Equips).Where(x => x.EquipSlot == equipSlot).SingleOrDefault();
                                        if(info == null) {
                                            return null;
                                        } else {
                                            return new Item()
                                            {
                                                ItemId = info.ItemId,
                                                Unk3 = info.Unk0,
                                                Color = info.Color,
                                                PlusValue = info.PlusValue,
                                                WeaponCrestDataList = info.WeaponCrestDataList,
                                                ArmorCrestDataList = info.ArmorCrestDataList,
                                                EquipElementParamList = info.EquipElementParamList
                                            };
                                        }
                                    })
                                    .ToList()
                            }
                        }
                    }
                },
                new Dictionary<JobId, List<Item?>>());
            character.JewelrySlotNum = packet.Structure.CharacterInfo.JewelrySlotNum;
            //character.CharacterItemSlotInfoList = packet.Structure.CharacterInfo.CharacterItemSlotInfoList;
            //character.UnkCharData0 = packet.Structure.CharacterInfo.UnkCharData0;
            //character.UnkCharData1 = packet.Structure.CharacterInfo.UnkCharData1;
            character.MyPawnSlotNum = packet.Structure.CharacterInfo.MyPawnSlotNum;
            character.RentalPawnSlotNum = packet.Structure.CharacterInfo.RentalPawnSlotNum;
            //character.OrbStatusList = packet.Structure.CharacterInfo.OrbStatusList;
            //character.MsgSetList = packet.Structure.CharacterInfo.MsgSetList;
            //character.ShortCutList = packet.Structure.CharacterInfo.ShortCutList;
            //character.CommunicationShortCutList = packet.Structure.CharacterInfo.CommunicationShortCutList;
            character.MatchingProfile = packet.Structure.CharacterInfo.MatchingProfile;
            character.ArisenProfile = packet.Structure.CharacterInfo.ArisenProfile;
            character.HideEquipHead = packet.Structure.CharacterInfo.HideEquipHead;
            character.HideEquipLantern = packet.Structure.CharacterInfo.HideEquipLantern;
            character.HideEquipHeadPawn = packet.Structure.CharacterInfo.HideEquipHeadPawn;
            character.HideEquipLanternPawn = packet.Structure.CharacterInfo.HideEquipLanternPawn;
            character.ArisenProfileShareRange = packet.Structure.CharacterInfo.ArisenProfileShareRange;
            character.OnlineStatus = packet.Structure.CharacterInfo.OnlineStatus;

            // Use the ArisenCsv row for the selected job as the preset equipment when the character is created
            ArisenCsv ActiveJobPreset = Server.AssetRepository.ArisenAsset.Where(x => x.Job == character.Job).Single();
            S2CCharacterDecideCharacterIdRes pcapCharacter = EntitySerializer.Get<S2CCharacterDecideCharacterIdRes>().Read(LoginDump.game_dump_data_Dump_13);
            // PlayPointList will be empty when a character is created
            character.StatusInfo = new CDataStatusInfo() {
                HP = ActiveJobPreset.HP,
                Stamina = ActiveJobPreset.Stamina,
                RevivePoint = ActiveJobPreset.RevivePoint,
                MaxHP = ActiveJobPreset.MaxHP,
                MaxStamina = ActiveJobPreset.MaxStamina,
                WhiteHP = ActiveJobPreset.WhiteHP,
                GainHP = ActiveJobPreset.GainHP,
                GainStamina = ActiveJobPreset.GainStamina,
                GainAttack = ActiveJobPreset.GainAttack,
                GainDefense = ActiveJobPreset.GainDefense,
                GainMagicAttack = ActiveJobPreset.GainMagicAttack,
                GainMagicDefense = ActiveJobPreset.GainMagicDefense
            };
            character.CharacterJobDataList = Server.AssetRepository.ArisenAsset.Select(arisenPreset => new CDataCharacterJobData {
                    Job = arisenPreset.Job,
                    Exp = arisenPreset.Exp,
                    JobPoint = arisenPreset.JobPoint,
                    Lv = arisenPreset.Lv,
                    Atk = arisenPreset.PAtk,
                    Def = arisenPreset.PDef,
                    MAtk = arisenPreset.MAtk,
                    MDef = arisenPreset.MDef,
                    Strength = arisenPreset.Strength,
                    DownPower = arisenPreset.DownPower,
                    ShakePower = arisenPreset.ShakePower,
                    StunPower = arisenPreset.StunPower,
                    Consitution = arisenPreset.Consitution,
                    Guts = arisenPreset.Guts,
                    FireResist = arisenPreset.FireResist,
                    IceResist = arisenPreset.IceResist,
                    ThunderResist = arisenPreset.ThunderResist,
                    HolyResist = arisenPreset.HolyResist,
                    DarkResist = arisenPreset.DarkResist,
                    SpreadResist = arisenPreset.SpreadResist,
                    FreezeResist = arisenPreset.FreezeResist,
                    ShockResist = arisenPreset.ShockResist,
                    AbsorbResist = arisenPreset.AbsorbResist,
                    DarkElmResist = arisenPreset.DarkElmResist,
                    PoisonResist = arisenPreset.PoisonResist,
                    SlowResist = arisenPreset.SlowResist,
                    SleepResist = arisenPreset.SleepResist,
                    StunResist = arisenPreset.StunResist,
                    WetResist = arisenPreset.WetResist,
                    OilResist = arisenPreset.OilResist,
                    SealResist = arisenPreset.SealResist,
                    CurseResist = arisenPreset.CurseResist,
                    SoftResist = arisenPreset.SoftResist,
                    StoneResist = arisenPreset.StoneResist,
                    GoldResist = arisenPreset.GoldResist,
                    FireReduceResist = arisenPreset.FireReduceResist,
                    IceReduceResist = arisenPreset.IceReduceResist,
                    ThunderReduceResist = arisenPreset.ThunderReduceResist,
                    HolyReduceResist = arisenPreset.HolyReduceResist,
                    DarkReduceResist = arisenPreset.DarkReduceResist,
                    AtkDownResist = arisenPreset.AtkDownResist,
                    DefDownResist = arisenPreset.DefDownResist,
                    MAtkDownResist = arisenPreset.MAtkDownResist,
                    MDefDownResist = arisenPreset.MDefDownResist
            }).ToList();
            character.Equipment = new Equipment(
                Server.AssetRepository.ArisenAsset.Select(arisenPreset => new Tuple<JobId, Dictionary<EquipType, List<Item>>>(arisenPreset.Job, new Dictionary<EquipType, List<Item>>() {
                    {
                        EquipType.Performance,
                        new List<Item>() {
                            new Item {
                                ItemId = arisenPreset.PrimaryWeapon,
                                Unk3 = 0,
                                Color = arisenPreset.PrimaryWeaponColour,
                                PlusValue = 0,
                                WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                    new CDataWeaponCrestData {
                                        SlotNo = 1,
                                        CrestId = arisenPreset.PWCrest1,
                                        Add = (ushort) (arisenPreset.PWC1Add1 << 8 | arisenPreset.PWC1Add2),
                                    },
                                    new CDataWeaponCrestData {
                                        SlotNo = 2,
                                        CrestId = arisenPreset.PWCrest2,
                                        Add = (ushort) (arisenPreset.PWC2Add1 << 8 | arisenPreset.PWC2Add2),
                                    },
                                    new CDataWeaponCrestData {
                                        SlotNo = 3,
                                        CrestId = arisenPreset.PWCrest3,
                                        Add = (ushort) (arisenPreset.PWC3Add1 << 8 | arisenPreset.PWC3Add2),
                                    },
                                    new CDataWeaponCrestData {
                                        SlotNo = 4,
                                        CrestId = arisenPreset.PWCrest4,
                                        Add = (ushort) (arisenPreset.PWC4Add1 << 8 | arisenPreset.PWC4Add2),
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
                            new Item {
                                ItemId = arisenPreset.SecondaryWeapon,
                                Unk3 = 0,
                                Color = arisenPreset.SecondaryWeaponColour
                            },
                            new Item {
                                ItemId = arisenPreset.Head,
                                Unk3 = 0,
                                Color = arisenPreset.HeadColour,
                                PlusValue = 3,
                                WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                    new CDataWeaponCrestData {
                                        SlotNo = 1,
                                        CrestId = arisenPreset.HeadCrest1,
                                        Add = (ushort) (arisenPreset.HC1Add1 << 8 | arisenPreset.HC1Add2),
                                    },
                                    new CDataWeaponCrestData {
                                        SlotNo = 2,
                                        CrestId = arisenPreset.HeadCrest2,
                                        Add = (ushort) (arisenPreset.HC2Add1 << 8 | arisenPreset.HC2Add2),
                                    },
                                    new CDataWeaponCrestData {
                                        SlotNo = 3,
                                        CrestId = arisenPreset.HeadCrest3,
                                        Add = (ushort) (arisenPreset.HC3Add1 << 8 | arisenPreset.HC3Add2),
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
                            new Item {
                                ItemId = arisenPreset.Body,
                                Unk3 = 0,
                                Color = arisenPreset.BodyColour,
                                PlusValue = 4,
                                WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                    new CDataWeaponCrestData {
                                        SlotNo = 1,
                                        CrestId = arisenPreset.BodyCrest1,
                                        Add = (ushort) (arisenPreset.BC1Add1 << 8 | arisenPreset.BC1Add2),
                                    },
                                    new CDataWeaponCrestData {
                                        SlotNo = 2,
                                        CrestId = arisenPreset.BodyCrest2,
                                        Add = (ushort) (arisenPreset.BC2Add1 << 8 | arisenPreset.BC2Add2),
                                    },
                                    new CDataWeaponCrestData {
                                        SlotNo = 3,
                                        CrestId = arisenPreset.BodyCrest3,
                                        Add = (ushort) (arisenPreset.BC3Add1 << 8 | arisenPreset.BC3Add2),
                                    },
                                    new CDataWeaponCrestData {
                                        SlotNo = 4,
                                        CrestId = arisenPreset.BodyCrest4,
                                        Add = (ushort) (arisenPreset.BC4Add1 << 8 | arisenPreset.BC4Add2),
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
                            new Item {
                                ItemId = arisenPreset.Clothing,
                                Unk3 = 0,
                                Color = arisenPreset.ClothingColour
                            },
                            new Item {
                                ItemId = arisenPreset.Arm,
                                Unk3 = 0,
                                Color = arisenPreset.ArmColour,
                                PlusValue = 3,
                                WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                    new CDataWeaponCrestData {
                                        SlotNo = 1,
                                        CrestId = arisenPreset.ArmCrest1,
                                        Add = (ushort) (arisenPreset.AC1Add1 << 8 | arisenPreset.AC1Add2),
                                    },
                                    new CDataWeaponCrestData {
                                        SlotNo = 2,
                                        CrestId = arisenPreset.ArmCrest2,
                                        Add = (ushort) (arisenPreset.AC2Add1 << 8 | arisenPreset.AC2Add2),
                                    },
                                    new CDataWeaponCrestData {
                                        SlotNo = 3,
                                        CrestId = arisenPreset.ArmCrest3,
                                        Add = (ushort) (arisenPreset.AC3Add1 << 8 | arisenPreset.AC3Add2),
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
                            new Item {
                                ItemId = arisenPreset.Leg,
                                Unk3 = 0,
                                Color = arisenPreset.LegColour,
                                PlusValue = 3,
                                WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                    new CDataWeaponCrestData {
                                        SlotNo = 1,
                                        CrestId = arisenPreset.LegCrest1,
                                        Add = (ushort) (arisenPreset.LC1Add1 << 8 | arisenPreset.LC1Add2),
                                    },
                                    new CDataWeaponCrestData {
                                        SlotNo = 2,
                                        CrestId = arisenPreset.LegCrest2,
                                        Add = (ushort) (arisenPreset.LC2Add1 << 8 | arisenPreset.LC2Add2),
                                    },
                                    new CDataWeaponCrestData {
                                        SlotNo = 3,
                                        CrestId = arisenPreset.LegCrest3,
                                        Add = (ushort) (arisenPreset.LC3Add1 << 8 | arisenPreset.LC3Add2),
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
                            new Item {
                                ItemId = arisenPreset.Legwear,
                                Unk3 = 0,
                                Color = arisenPreset.LegwearColour
                            },
                            new Item {
                                ItemId = arisenPreset.Overwear,
                                Unk3 = 0,
                                Color = arisenPreset.OverwearColour
                            },
                            new Item {
                                ItemId = arisenPreset.Jewelry1,
                                Unk3 = 0,
                                Color = 0,
                                PlusValue = 0,
                                WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                    new CDataWeaponCrestData {
                                        SlotNo = 1,
                                        CrestId = arisenPreset.J1Crest1,
                                        Add = (ushort) (arisenPreset.J1C1Add1 << 8 | arisenPreset.J1C1Add2),
                                    },
                                    new CDataWeaponCrestData {
                                        SlotNo = 2,
                                        CrestId = arisenPreset.J1Crest2,
                                        Add = (ushort) (arisenPreset.J1C2Add1 << 8 | arisenPreset.J1C2Add2),
                                    },
                                    new CDataWeaponCrestData {
                                        SlotNo = 3,
                                        CrestId = arisenPreset.J1Crest3,
                                        Add = (ushort) (arisenPreset.J1C3Add1 << 8 | arisenPreset.J1C3Add2),
                                    },
                                    new CDataWeaponCrestData {
                                        SlotNo = 4,
                                        CrestId = arisenPreset.J1Crest4,
                                        Add = (ushort) (arisenPreset.J1C4Add1 << 8 | arisenPreset.J1C4Add2),
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
                            new Item {
                                ItemId = arisenPreset.Jewelry2,
                                Unk3 = 0,
                                Color = 0,
                                PlusValue = 0,
                                WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                    new CDataWeaponCrestData {
                                        SlotNo = 1,
                                        CrestId = arisenPreset.J2Crest1,
                                        Add = (ushort) (arisenPreset.J2C1Add1 << 8 | arisenPreset.J2C1Add2),
                                    },
                                    new CDataWeaponCrestData {
                                        SlotNo = 2,
                                        CrestId = arisenPreset.J2Crest2,
                                        Add = (ushort) (arisenPreset.J2C2Add1 << 8 | arisenPreset.J2C2Add2),
                                    },
                                    new CDataWeaponCrestData {
                                        SlotNo = 3,
                                        CrestId = arisenPreset.J2Crest3,
                                        Add = (ushort) (arisenPreset.J2C3Add1 << 8 | arisenPreset.J2C3Add2),
                                    },
                                    new CDataWeaponCrestData {
                                        SlotNo = 4,
                                        CrestId = arisenPreset.J2Crest4,
                                        Add = (ushort) (arisenPreset.J2C4Add1 << 8 | arisenPreset.J2C4Add2),
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
                            new Item {
                                ItemId = arisenPreset.Jewelry3,
                                Unk3 = 0,
                                Color = 0,
                                PlusValue = 0,
                                WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                    new CDataWeaponCrestData {
                                        SlotNo = 1,
                                        CrestId = arisenPreset.J3Crest1,
                                        Add = (ushort) (arisenPreset.J3C1Add1 << 8 | arisenPreset.J3C1Add2),
                                    },
                                    new CDataWeaponCrestData {
                                        SlotNo = 2,
                                        CrestId = arisenPreset.J3Crest2,
                                        Add = (ushort) (arisenPreset.J3C2Add1 << 8 | arisenPreset.J3C2Add2),
                                    },
                                    new CDataWeaponCrestData {
                                        SlotNo = 3,
                                        CrestId = arisenPreset.J3Crest3,
                                        Add = (ushort) (arisenPreset.J3C3Add1 << 8 | arisenPreset.J3C3Add2),
                                    },
                                    new CDataWeaponCrestData {
                                        SlotNo = 4,
                                        CrestId = arisenPreset.J3Crest4,
                                        Add = (ushort) (arisenPreset.J3C4Add1 << 8 | arisenPreset.J3C4Add2),
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
                            new Item {
                                ItemId = arisenPreset.Jewelry4,
                                Unk3 = 0,
                                Color = 0,
                                PlusValue = 0,
                                WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                    new CDataWeaponCrestData {
                                        SlotNo = 1,
                                        CrestId = arisenPreset.J4Crest1,
                                        Add = (ushort) (arisenPreset.J4C1Add1 << 8 | arisenPreset.J4C1Add2),
                                    },
                                    new CDataWeaponCrestData {
                                        SlotNo = 2,
                                        CrestId = arisenPreset.J4Crest2,
                                        Add = (ushort) (arisenPreset.J4C2Add1 << 8 | arisenPreset.J4C2Add2),
                                    },
                                    new CDataWeaponCrestData {
                                        SlotNo = 3,
                                        CrestId = arisenPreset.J4Crest3,
                                        Add = (ushort) (arisenPreset.J4C3Add1 << 8 | arisenPreset.J4C3Add2),
                                    },
                                    new CDataWeaponCrestData {
                                        SlotNo = 4,
                                        CrestId = arisenPreset.J4Crest4,
                                        Add = (ushort) (arisenPreset.J4C4Add1 << 8 | arisenPreset.J4C4Add2),
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
                            new Item {
                                ItemId = arisenPreset.Jewelry5,
                                Unk3 = 0,
                                Color = 0,
                                PlusValue = 0,
                                WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                    new CDataWeaponCrestData {
                                        SlotNo = 1,
                                        CrestId = arisenPreset.J5Crest1,
                                        Add = (ushort) (arisenPreset.J5C1Add1 << 8 | arisenPreset.J5C1Add2),
                                    },
                                    new CDataWeaponCrestData {
                                        SlotNo = 2,
                                        CrestId = arisenPreset.J5Crest2,
                                        Add = (ushort) (arisenPreset.J5C2Add1 << 8 | arisenPreset.J5C2Add2),
                                    },
                                    new CDataWeaponCrestData {
                                        SlotNo = 3,
                                        CrestId = arisenPreset.J5Crest3,
                                        Add = (ushort) (arisenPreset.J5C3Add1 << 8 | arisenPreset.J5C3Add2),
                                    },
                                    new CDataWeaponCrestData {
                                        SlotNo = 4,
                                        CrestId = arisenPreset.J5Crest4,
                                        Add = (ushort) (arisenPreset.J5C4Add1 << 8 | arisenPreset.J5C4Add2),
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
                            new Item {
                                ItemId = arisenPreset.Lantern,
                                Unk3 = 0,
                            }
                        }.Select(item => (item == null || item.ItemId == 0) ? null : item).ToList()
                    },
                    {
                        EquipType.Visual,
                        new List<Item>() {
                            new Item {
                                ItemId = arisenPreset.VPrimaryWeapon,
                                Unk3 = 0,
                                Color = arisenPreset.VPrimaryWeaponColour
                            },
                            new Item {
                                ItemId = arisenPreset.VSecondaryWeapon,
                                Unk3 = 0,
                                Color = arisenPreset.VSecondaryWeaponColour
                            },
                            new Item {
                                ItemId = arisenPreset.VHead,
                                Unk3 = 0,
                                Color = arisenPreset.VHeadColour
                            },
                            new Item {
                                ItemId = arisenPreset.VBody,
                                Unk3 = 0,
                                Color = arisenPreset.VBodyColour
                            },
                            new Item {
                                ItemId = arisenPreset.VClothing,
                                Unk3 = 0,
                                Color = arisenPreset.VClothingColour
                            },
                            new Item {
                                ItemId = arisenPreset.VArm,
                                Unk3 = 0,
                                Color = arisenPreset.VArmColour
                            },
                            new Item {
                                ItemId = arisenPreset.VLeg,
                                Unk3 = 0,
                                Color = arisenPreset.VLegColour
                            },
                            new Item {
                                ItemId = arisenPreset.VLegwear,
                                Unk3 = 0,
                                Color = arisenPreset.VLegwearColour
                            },
                            new Item {
                                ItemId = arisenPreset.VOverwear,
                                Unk3 = 0,
                                Color = arisenPreset.VOverwearColour,
                            }
                        }.Select(item => (item == null || item.ItemId == 0) ? null : item).ToList()
                    }
                })).ToDictionary(x => x.Item1, x => x.Item2),
                Server.AssetRepository.ArisenAsset.Select(arisenPreset => new Tuple<JobId, List<Item>>(arisenPreset.Job, new List<Item>() {
                        new Item()
                        {
                            ItemId = arisenPreset.ClassItem1
                        },
                        new Item()
                        {
                            ItemId = arisenPreset.ClassItem1
                        }
                })).ToDictionary(x => x.Item1, x => x.Item2)
            );
            character.HideEquipHead = ActiveJobPreset.DisplayHelmet;
            character.HideEquipLantern = ActiveJobPreset.DisplayLantern;
            character.HideEquipHeadPawn = packet.Structure.CharacterInfo.HideEquipHeadPawn;
            character.HideEquipLanternPawn = packet.Structure.CharacterInfo.HideEquipLanternPawn;
            character.EquippedCustomSkillsDictionary = Server.AssetRepository.ArisenAsset.Select(arisenPreset => new Tuple<JobId, List<CustomSkill>>(arisenPreset.Job, new List<CustomSkill>() {
                // Main Palette
                new CustomSkill() {
                    Job = arisenPreset.Job,
                    SkillId = arisenPreset.Cs1MpId,
                    SkillLv = arisenPreset.Cs1MpLv
                },
                new CustomSkill() {
                    Job = arisenPreset.Job,
                    SkillId = arisenPreset.Cs2MpId,
                    SkillLv = arisenPreset.Cs2MpLv
                },
                new CustomSkill() {
                    Job = arisenPreset.Job,
                    SkillId = arisenPreset.Cs3MpId,
                    SkillLv = arisenPreset.Cs3MpLv
                },
                new CustomSkill() {
                    Job = arisenPreset.Job,
                    SkillId = arisenPreset.Cs4MpId,
                    SkillLv = arisenPreset.Cs4MpLv
                },
                null, null, null, null, null, null, null, null, null, null, null, null, // Padding from slots 0x04 (Main Palette slot 4) to 0x11 (Sub Palette slot 1)
                // Sub Palette
                new CustomSkill() {
                    Job = arisenPreset.Job,
                    SkillId = arisenPreset.Cs1SpId,
                    SkillLv = arisenPreset.Cs1SpLv
                },
                new CustomSkill() {
                    Job = arisenPreset.Job,
                    SkillId = arisenPreset.Cs2SpId,
                    SkillLv = arisenPreset.Cs2SpLv
                },
                new CustomSkill() {
                    Job = arisenPreset.Job,
                    SkillId = arisenPreset.Cs3SpId,
                    SkillLv = arisenPreset.Cs3SpLv
                },
                new CustomSkill() {
                    Job = arisenPreset.Job,
                    SkillId = arisenPreset.Cs4SpId,
                    SkillLv = arisenPreset.Cs4SpLv
                }
            }.Select(skill => skill?.SkillId == 0 ? null : skill).ToList()
            )).ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);
            character.LearnedCustomSkills = character.EquippedCustomSkillsDictionary.SelectMany(jobAndSkills => jobAndSkills.Value).Where(skill => skill != null).ToList();
            character.EquippedAbilitiesDictionary = Server.AssetRepository.ArisenAsset.Select(arisenPreset => new Tuple<JobId, List<Ability>>(arisenPreset.Job, new List<Ability>() {
                new Ability() {
                    Job = arisenPreset.Ab1Jb,
                    AbilityId = arisenPreset.Ab1Id,
                    AbilityLv = arisenPreset.Ab1Lv
                },
                new Ability() {
                    Job = arisenPreset.Ab2Jb,
                    AbilityId = arisenPreset.Ab2Id,
                    AbilityLv = arisenPreset.Ab2Lv
                },
                new Ability() {
                    Job = arisenPreset.Ab3Jb,
                    AbilityId = arisenPreset.Ab3Id,
                    AbilityLv = arisenPreset.Ab3Lv
                },
                new Ability() {
                    Job = arisenPreset.Ab4Jb,
                    AbilityId = arisenPreset.Ab4Id,
                    AbilityLv = arisenPreset.Ab4Lv
                },
                new Ability() {
                    Job = arisenPreset.Ab5Jb,
                    AbilityId = arisenPreset.Ab5Id,
                    AbilityLv = arisenPreset.Ab5Lv
                },
                new Ability() {
                    Job = arisenPreset.Ab6Jb,
                    AbilityId = arisenPreset.Ab6Id,
                    AbilityLv = arisenPreset.Ab6Lv
                },
                new Ability() {
                    Job = arisenPreset.Ab7Jb,
                    AbilityId = arisenPreset.Ab7Id,
                    AbilityLv = arisenPreset.Ab7Lv
                },
                new Ability() {
                    Job = arisenPreset.Ab8Jb,
                    AbilityId = arisenPreset.Ab8Id,
                    AbilityLv = arisenPreset.Ab8Lv
                },
                new Ability() {
                    Job = arisenPreset.Ab9Jb,
                    AbilityId = arisenPreset.Ab9Id,
                    AbilityLv = arisenPreset.Ab9Lv
                },
                new Ability() {
                    Job = arisenPreset.Ab10Jb,
                    AbilityId = arisenPreset.Ab10Id,
                    AbilityLv = arisenPreset.Ab10Lv
                }
            }.Select(aug => aug?.AbilityId == 0 ? null : aug).ToList()
            )).ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);
            character.LearnedAbilities = character.EquippedAbilitiesDictionary.SelectMany(jobAndAugs => jobAndAugs.Value).Where(aug => aug != null).ToList();
            character.Storage = new Storages(Server.AssetRepository.StorageAsset.ToDictionary(x => x.StorageType, x => x.SlotMax));
            character.WalletPointList = new List<CDataWalletPoint>()
            {
                // TODO: Figure out what other currencies there are.
                // Pcap currencies:
                new CDataWalletPoint() {
                    Type = WalletType.Gold,
                    Value = 10000
                },
                new CDataWalletPoint() {
                    Type = WalletType.RiftPoints,
                    Value = 10000
                },
                new CDataWalletPoint() {
                    Type = WalletType.BloodOrbs,
                    Value = 0
                },
                new CDataWalletPoint() {
                    Type = WalletType.Unk4,
                    Value = 5648
                },
                new CDataWalletPoint() {
                    Type = WalletType.Unk6,
                    Value = 99999
                },
                new CDataWalletPoint() {
                    Type = WalletType.HighOrbs,
                    Value = 0
                },
                new CDataWalletPoint() {
                    Type = WalletType.Unk10,
                    Value = 0
                },
                new CDataWalletPoint() {
                    Type = WalletType.Unk11,
                    Value = 8
                },
                new CDataWalletPoint() {
                    Type = WalletType.Unk12,
                    Value = 219
                },
                new CDataWalletPoint() {
                    Type = WalletType.Unk13,
                    Value = 2
                },
                new CDataWalletPoint() {
                    Type = WalletType.Unk14,
                    Value = 2
                },
                new CDataWalletPoint() {
                    Type = WalletType.Unk15,
                    Value = 115
                },
                new CDataWalletPoint() {
                    Type = WalletType.Unk16,
                    Value = 105
                }
            };

            // Add starting storage items
            foreach (var tuple in Server.AssetRepository.StorageItemAsset)
            {
                if(tuple.Item3.ItemId != 0)
                {
                    character.Storage.addStorageItem(tuple.Item3, tuple.Item2, tuple.Item1);
                }
            }

            L2CCreateCharacterDataRes res = new L2CCreateCharacterDataRes();
            if (!Database.CreateCharacter(character))
            {
                Logger.Error(client, "Failed to create character");
                res.Result = 1;
                client.Send(res);
            }

            // Pawns
            LoadDefaultPawns(character);
            foreach (Pawn pawn in character.Pawns)
            {
                Database.CreatePawn(pawn);
            }

            L2CCreateCharacterDataNtc ntc = new L2CCreateCharacterDataNtc();
            ntc.Result = character.CharacterId; // Value will show up in DecideCharacterIdHandler as CharacterId
            client.Send(ntc);

            // Sent to client once the player queue "WaitNum" above is 0,
            // send immediately in our case.

            res.Result = 0;
            res.WaitNum = 0;
            client.Send(res);
        }

        public void LoadDefaultPawns(Character character)
        {
            character.Pawns = Server.AssetRepository.MyPawnAsset.Select(myPawnCsvData => LoadDefaultPawn(character, myPawnCsvData)).ToList();
        }

        private Pawn LoadDefaultPawn(Character character, MyPawnCsv myPawnCsvData)
        {
            S2CContextGetPartyMypawnContextNtc pcapPawn = EntitySerializer.Get<S2CContextGetPartyMypawnContextNtc>().Read(data_Dump_Pawn35_3_16); // TODO: Replace pcap data
            Pawn pawn = new Pawn(character.CharacterId);
            pawn.PawnId = myPawnCsvData.PawnId;
            pawn.CharacterId = character.CharacterId; // pawns characterId, refers to the owner
            pawn.Server = Server.AssetRepository.ServerList[0]; // TODO: is it possible for a pawn to be in a different server than its owner?
            
            pawn.HmType = myPawnCsvData.HmType;
            pawn.PawnType = myPawnCsvData.PawnType;
            pawn.Name = myPawnCsvData.Name;
            pawn.EditInfo = new CDataEditInfo()
            {
                Sex = myPawnCsvData.Sex,
                Voice = myPawnCsvData.Voice,
                VoicePitch = myPawnCsvData.VoicePitch,
                Personality = myPawnCsvData.Personality,
                SpeechFreq = myPawnCsvData.SpeechFreq,
                BodyType = myPawnCsvData.BodyType,
                Hair = myPawnCsvData.Hair,
                Beard = myPawnCsvData.Beard,
                Makeup = myPawnCsvData.Makeup,
                Scar = myPawnCsvData.Scar,
                EyePresetNo = myPawnCsvData.EyePresetNo,
                NosePresetNo = myPawnCsvData.NosePresetNo,
                MouthPresetNo = myPawnCsvData.MouthPresetNo,
                EyebrowTexNo = myPawnCsvData.EyebrowTexNo,
                ColorSkin = myPawnCsvData.ColorSkin,
                ColorHair = myPawnCsvData.ColorHair,
                ColorBeard = myPawnCsvData.ColorBeard,
                ColorEyebrow = myPawnCsvData.ColorEyebrow,
                ColorREye = myPawnCsvData.ColorREye,
                ColorLEye = myPawnCsvData.ColorLEye,
                ColorMakeup = myPawnCsvData.ColorMakeup,
                Sokutobu = myPawnCsvData.Sokutobu,
                Hitai = myPawnCsvData.Hitai,
                MimiJyouge = myPawnCsvData.MimiJyouge,
                Kannkaku = myPawnCsvData.Kannkaku,
                MabisasiJyouge = myPawnCsvData.MabisasiJyouge,
                HanakuchiJyouge = myPawnCsvData.HanakuchiJyouge,
                AgoSakiHaba = myPawnCsvData.AgoSakiHaba,
                AgoZengo = myPawnCsvData.AgoZengo,
                AgoSakiJyouge = myPawnCsvData.AgoSakiJyouge,
                HitomiOokisa = myPawnCsvData.HitomiOokisa,
                MeOokisa = myPawnCsvData.MeOokisa,
                MeKaiten = myPawnCsvData.MeKaiten,
                MayuKaiten = myPawnCsvData.MayuKaiten,
                MimiOokisa = myPawnCsvData.MimiOokisa,
                MimiMuki = myPawnCsvData.MimiMuki,
                ElfMimi = myPawnCsvData.ElfMimi,
                MikenTakasa = myPawnCsvData.MikenTakasa,
                MikenHaba = myPawnCsvData.MikenHaba,
                HohoboneRyou = myPawnCsvData.HohoboneRyou,
                HohoboneJyouge = myPawnCsvData.HohoboneJyouge,
                Hohoniku = myPawnCsvData.Hohoniku,
                ErahoneJyouge = myPawnCsvData.ErahoneJyouge,
                ErahoneHaba = myPawnCsvData.ErahoneHaba,
                HanaJyouge = myPawnCsvData.HanaJyouge,
                HanaHaba = myPawnCsvData.HanaHaba,
                HanaTakasa = myPawnCsvData.HanaTakasa,
                HanaKakudo = myPawnCsvData.HanaKakudo,
                KuchiHaba = myPawnCsvData.KuchiHaba,
                KuchiAtsusa = myPawnCsvData.KuchiAtsusa,
                EyebrowUVOffsetX = myPawnCsvData.EyebrowUVOffsetX,
                EyebrowUVOffsetY = myPawnCsvData.EyebrowUVOffsetY,
                Wrinkle = myPawnCsvData.Wrinkle,
                WrinkleAlbedoBlendRate = myPawnCsvData.WrinkleAlbedoBlendRate,
                WrinkleDetailNormalPower = myPawnCsvData.WrinkleDetailNormalPower,
                MuscleAlbedoBlendRate = myPawnCsvData.MuscleAlbedoBlendRate,
                MuscleDetailNormalPower = myPawnCsvData.MuscleDetailNormalPower,
                Height = myPawnCsvData.Height,
                HeadSize = myPawnCsvData.HeadSize,
                NeckOffset = myPawnCsvData.NeckOffset,
                NeckScale = myPawnCsvData.NeckScale,
                UpperBodyScaleX = myPawnCsvData.UpperBodyScaleX,
                BellySize = myPawnCsvData.BellySize,
                TeatScale = myPawnCsvData.TeatScale,
                TekubiSize = myPawnCsvData.TekubiSize,
                KoshiOffset = myPawnCsvData.KoshiOffset,
                KoshiSize = myPawnCsvData.KoshiSize,
                AnkleOffset = myPawnCsvData.AnkleOffset,
                Fat = myPawnCsvData.Fat,
                Muscle = myPawnCsvData.Muscle,
                MotionFilter = myPawnCsvData.MotionFilter
            };
            pawn.Job = myPawnCsvData.Job;
            pawn.HideEquipHead = character.HideEquipHeadPawn;
            pawn.HideEquipLantern = character.HideEquipLanternPawn;
            pawn.StatusInfo = new CDataStatusInfo()
            {
                HP = (uint) pcapPawn.Context.PlayerInfo.HP,
                Stamina = (uint) pcapPawn.Context.PlayerInfo.Stamina,
                RevivePoint = pcapPawn.Context.PlayerInfo.RevivePoint,
                MaxHP = (uint) pcapPawn.Context.PlayerInfo.MaxHP,
                MaxStamina = (uint) pcapPawn.Context.PlayerInfo.MaxStamina,
                WhiteHP = (uint) pcapPawn.Context.PlayerInfo.WhiteHP,
                GainHP = pcapPawn.Context.PlayerInfo.GainHp,
                GainStamina = pcapPawn.Context.PlayerInfo.GainStamina,
                GainAttack = pcapPawn.Context.PlayerInfo.GainAttack,
                GainDefense = pcapPawn.Context.PlayerInfo.GainDefense,
                GainMagicAttack = pcapPawn.Context.PlayerInfo.GainMagicAttack,
                GainMagicDefense = pcapPawn.Context.PlayerInfo.GainMagicDefense
            };
            //pawn.Character.PlayPointList
            pawn.CharacterJobDataList = new List<CDataCharacterJobData>(){ new CDataCharacterJobData {
                    Job = myPawnCsvData.Job,
                    //Exp = myPawnCsvData.Exp,
                    //JobPoint = myPawnCsvData.JobPoint,
                    Lv = myPawnCsvData.JobLv,
                    Atk = (ushort) pcapPawn.Context.PlayerInfo.Atk,
                    Def = (ushort) pcapPawn.Context.PlayerInfo.Def,
                    MAtk = (ushort) pcapPawn.Context.PlayerInfo.MAtk,
                    MDef = (ushort) pcapPawn.Context.PlayerInfo.MDef,
                    Strength = (ushort) pcapPawn.Context.PlayerInfo.Strength,
                    DownPower = (ushort) pcapPawn.Context.PlayerInfo.DownPower,
                    ShakePower = (ushort) pcapPawn.Context.PlayerInfo.ShakePower,
                    //StunPower = (ushort) pcapPawn.Context.PlayerInfo.StunPower,
                    Consitution = (ushort) pcapPawn.Context.PlayerInfo.Constitution,
                    Guts = (ushort) pcapPawn.Context.PlayerInfo.Guts,
                    FireResist = pcapPawn.Context.ResistInfo.FireResist,
                    IceResist = pcapPawn.Context.ResistInfo.IceResist,
                    ThunderResist = pcapPawn.Context.ResistInfo.ThunderResist,
                    HolyResist = pcapPawn.Context.ResistInfo.HolyResist,
                    DarkResist = pcapPawn.Context.ResistInfo.DarkResist,
                    SpreadResist = pcapPawn.Context.ResistInfo.SpreadResist,
                    FreezeResist = pcapPawn.Context.ResistInfo.FreezeResist,
                    ShockResist = pcapPawn.Context.ResistInfo.ShockResist,
                    AbsorbResist = pcapPawn.Context.ResistInfo.AbsorbResist,
                    DarkElmResist = pcapPawn.Context.ResistInfo.DarkElmResist,
                    PoisonResist = pcapPawn.Context.ResistInfo.PoisonResist,
                    SlowResist = pcapPawn.Context.ResistInfo.SlowResist,
                    SleepResist = pcapPawn.Context.ResistInfo.SleepResist,
                    StunResist = pcapPawn.Context.ResistInfo.StunResist,
                    WetResist = pcapPawn.Context.ResistInfo.WetResist,
                    OilResist = pcapPawn.Context.ResistInfo.OilResist,
                    SealResist = pcapPawn.Context.ResistInfo.SealResist,
                    CurseResist = pcapPawn.Context.ResistInfo.CurseResist,
                    SoftResist = pcapPawn.Context.ResistInfo.SoftResist,
                    StoneResist = pcapPawn.Context.ResistInfo.StoneResist,
                    GoldResist = pcapPawn.Context.ResistInfo.GoldResist,
                    FireReduceResist = pcapPawn.Context.ResistInfo.FireReduceResist,
                    IceReduceResist = pcapPawn.Context.ResistInfo.IceReduceResist,
                    ThunderReduceResist = pcapPawn.Context.ResistInfo.ThunderReduceResist,
                    HolyReduceResist = pcapPawn.Context.ResistInfo.HolyReduceResist,
                    DarkReduceResist = pcapPawn.Context.ResistInfo.DarkReduceResist,
                    AtkDownResist = pcapPawn.Context.ResistInfo.AtkDownResist,
                    DefDownResist = pcapPawn.Context.ResistInfo.DefDownResist,
                    MAtkDownResist = pcapPawn.Context.ResistInfo.MAtkDownResist,
                    MDefDownResist = pcapPawn.Context.ResistInfo.MDefDownResist,
            }};
            pawn.Equipment = new Equipment(
                new Dictionary<JobId, Dictionary<EquipType, List<Item>>>() 
                { 
                    { 
                        myPawnCsvData.Job, 
                        new Dictionary<EquipType, List<Item>> 
                        {
                            {
                                EquipType.Performance,
                                new List<Item>() {
                                    new Item {
                                        ItemId = myPawnCsvData.Primary,
                                        Unk3 = 0,
                                        Color = 0,
                                        PlusValue = 0,
                                        WeaponCrestDataList = new List<CDataWeaponCrestData>(),
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
                                    new Item {
                                        ItemId = myPawnCsvData.Secondary,
                                        Unk3 = 0,
                                        Color = 0
                                    },
                                    new Item {
                                        ItemId = myPawnCsvData.Head,
                                        Unk3 = 0,
                                        Color = 0,
                                        PlusValue = 3,
                                        WeaponCrestDataList = new List<CDataWeaponCrestData>(),
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
                                    new Item {
                                        ItemId = myPawnCsvData.Body,
                                        Unk3 = 0,
                                        Color = 0,
                                        PlusValue = 4,
                                        WeaponCrestDataList = new List<CDataWeaponCrestData>(),
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
                                    new Item {
                                        ItemId = myPawnCsvData.BodyClothing,
                                        Unk3 = 0,
                                        Color = 0
                                    },
                                    new Item {
                                        ItemId = myPawnCsvData.Arm,
                                        Unk3 = 0,
                                        Color = 0,
                                        PlusValue = 3,
                                        WeaponCrestDataList = new List<CDataWeaponCrestData>(),
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
                                    new Item {
                                        ItemId = myPawnCsvData.Leg,
                                        Unk3 = 0,
                                        Color = 0,
                                        PlusValue = 3,
                                        WeaponCrestDataList = new List<CDataWeaponCrestData>(),
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
                                    new Item {
                                        ItemId = myPawnCsvData.LegWear,
                                        Unk3 = 0,
                                        Color = 0
                                    },
                                    new Item {
                                        ItemId = myPawnCsvData.OverWear,
                                        Unk3 = 0,
                                        Color = 0
                                    },
                                    new Item {
                                        ItemId = myPawnCsvData.JewelrySlot1,
                                        Unk3 = 0,
                                        Color = 0,
                                        PlusValue = 0,
                                        WeaponCrestDataList = new List<CDataWeaponCrestData>(),
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
                                    new Item {
                                        ItemId = myPawnCsvData.JewelrySlot2,
                                        Unk3 = 0,
                                        Color = 0,
                                        PlusValue = 0,
                                        WeaponCrestDataList = new List<CDataWeaponCrestData>(),
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
                                    new Item {
                                        ItemId = myPawnCsvData.JewelrySlot3,
                                        Unk3 = 0,
                                        Color = 0,
                                        PlusValue = 0,
                                        WeaponCrestDataList = new List<CDataWeaponCrestData>(),
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
                                    new Item {
                                        ItemId = myPawnCsvData.JewelrySlot4,
                                        Unk3 = 0,
                                        Color = 0,
                                        PlusValue = 0,
                                        WeaponCrestDataList = new List<CDataWeaponCrestData>(),
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
                                    new Item {
                                        ItemId = myPawnCsvData.JewelrySlot5,
                                        Unk3 = 0,
                                        Color = 0,
                                        PlusValue = 0,
                                        WeaponCrestDataList = new List<CDataWeaponCrestData>(),
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
                                    new Item {
                                        ItemId = myPawnCsvData.Lantern,
                                        Unk3 = 0,
                                    }
                                }.Select(item => (item == null || item.ItemId == 0) ? null : item).ToList()
                            },
                            {
                                EquipType.Visual,
                                new List<Item>() {
                                    new Item {
                                        ItemId = myPawnCsvData.VPrimary,
                                        Unk3 = 0,
                                        Color = 0
                                    },
                                    new Item {
                                        ItemId = myPawnCsvData.VSecondary,
                                        Unk3 = 0,
                                        Color = 0
                                    },
                                    new Item {
                                        ItemId = myPawnCsvData.VHead,
                                        Unk3 = 0,
                                        Color = 0
                                    },
                                    new Item {
                                        ItemId = myPawnCsvData.VBody,
                                        Unk3 = 0,
                                        Color = 0
                                    },
                                    new Item {
                                        ItemId = myPawnCsvData.VBodyClothing,
                                        Unk3 = 0,
                                        Color = 0
                                    },
                                    new Item {
                                        ItemId = myPawnCsvData.VArm,
                                        Unk3 = 0,
                                        Color = 0
                                    },
                                    new Item {
                                        ItemId = myPawnCsvData.VLeg,
                                        Unk3 = 0,
                                        Color = 0
                                    },
                                    new Item {
                                        ItemId = myPawnCsvData.VLegWear,
                                        Unk3 = 0,
                                        Color = 0
                                    },
                                    new Item {
                                        ItemId = myPawnCsvData.VOverWear,
                                        Unk3 = 0,
                                        Color = 0,
                                    },
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null
                                }.Select(item => (item == null || item.ItemId == 0) ? null : item).ToList()
                            }
                        }
                    }
                },
                new Dictionary<JobId, List<Item>>()
                { 
                    { 
                        myPawnCsvData.Job, 
                        new List<Item>() {
                                new Item()
                                {
                                    ItemId = myPawnCsvData.JobItem1
                                },
                                new Item()
                                {
                                    ItemId = myPawnCsvData.JobItem2
                                }
                        }
                    }
                }
            );
            pawn.LearnedNormalSkills = new List<CDataNormalSkillParam>();
            pawn.EquippedCustomSkillsDictionary = new Dictionary<JobId, List<CustomSkill>>() 
            {
                {
                    myPawnCsvData.Job,
                    new List<CustomSkill>() {
                        // Main Palette
                        new CustomSkill() {
                            Job = myPawnCsvData.Job,
                            SkillId = myPawnCsvData.CustomSkillId1,
                            SkillLv = myPawnCsvData.CustomSkillLv1
                        },
                        new CustomSkill() {
                            Job = myPawnCsvData.Job,
                            SkillId = myPawnCsvData.CustomSkillId2,
                            SkillLv = myPawnCsvData.CustomSkillLv2
                        },
                        new CustomSkill() {
                            Job = myPawnCsvData.Job,
                            SkillId = myPawnCsvData.CustomSkillId3,
                            SkillLv = myPawnCsvData.CustomSkillLv3
                        },
                        new CustomSkill() {
                            Job = myPawnCsvData.Job,
                            SkillId = myPawnCsvData.CustomSkillId4,
                            SkillLv = myPawnCsvData.CustomSkillLv4
                        }
                    }.Select(skill => skill?.SkillId == 0 ? null : skill).ToList()
                }
            };
            pawn.LearnedCustomSkills = pawn.EquippedCustomSkillsDictionary.SelectMany(skills => skills.Value).Where(skill => skill != null).ToList();
            pawn.EquippedAbilitiesDictionary = new Dictionary<JobId, List<Ability>>()
            {
                {
                    myPawnCsvData.Job,
                    new List<Ability>() {
                        new Ability() {
                            Job = (JobId) myPawnCsvData.AbilityJob1,
                            AbilityId = myPawnCsvData.AbilityId1,
                            AbilityLv = myPawnCsvData.AbilityLv1
                        },
                        new Ability() {
                            Job = (JobId) myPawnCsvData.AbilityJob2,
                            AbilityId = myPawnCsvData.AbilityId2,
                            AbilityLv = myPawnCsvData.AbilityLv2
                        },
                        new Ability() {
                            Job = (JobId) myPawnCsvData.AbilityJob3,
                            AbilityId = myPawnCsvData.AbilityId3,
                            AbilityLv = myPawnCsvData.AbilityLv3
                        },
                        new Ability() {
                            Job = (JobId) myPawnCsvData.AbilityJob4,
                            AbilityId = myPawnCsvData.AbilityId4,
                            AbilityLv = myPawnCsvData.AbilityLv4
                        },
                        new Ability() {
                            Job = (JobId) myPawnCsvData.AbilityJob5,
                            AbilityId = myPawnCsvData.AbilityId5,
                            AbilityLv = myPawnCsvData.AbilityLv5
                        },
                        new Ability() {
                            Job = (JobId) myPawnCsvData.AbilityJob6,
                            AbilityId = myPawnCsvData.AbilityId6,
                            AbilityLv = myPawnCsvData.AbilityLv6
                        },
                        new Ability() {
                            Job = (JobId) myPawnCsvData.AbilityJob7,
                            AbilityId = myPawnCsvData.AbilityId7,
                            AbilityLv = myPawnCsvData.AbilityLv7
                        },
                        new Ability() {
                            Job = (JobId) myPawnCsvData.AbilityJob8,
                            AbilityId = myPawnCsvData.AbilityId8,
                            AbilityLv = myPawnCsvData.AbilityLv8
                        },
                        new Ability() {
                            Job = (JobId) myPawnCsvData.AbilityJob9,
                            AbilityId = myPawnCsvData.AbilityId9,
                            AbilityLv = myPawnCsvData.AbilityLv9
                        },
                        new Ability() {
                            Job = (JobId) myPawnCsvData.AbilityJob10,
                            AbilityId = myPawnCsvData.AbilityId10,
                            AbilityLv = myPawnCsvData.AbilityLv10
                        }
                    }.Select(aug => aug?.AbilityId == 0 ? null : aug).ToList()
                }
            };
            pawn.LearnedAbilities = pawn.EquippedAbilitiesDictionary.SelectMany(augs => augs.Value).Where(aug => aug != null).ToList();
            pawn.PawnReactionList = new List<CDataPawnReaction>()
            {
                new CDataPawnReaction()
                {
                    ReactionType = 1,
                    MotionNo = myPawnCsvData.MetPartyMembersId
                },
                new CDataPawnReaction()
                {
                    ReactionType = 2,
                    MotionNo = myPawnCsvData.QuestClearId
                },
                new CDataPawnReaction()
                {
                    ReactionType = 10,
                    MotionNo = myPawnCsvData.SpecialSkillInspirationMomentId
                },
                new CDataPawnReaction()
                {
                    ReactionType = 4,
                    MotionNo = myPawnCsvData.LevelUpId
                },
                new CDataPawnReaction()
                {
                    ReactionType = 11,
                    MotionNo = myPawnCsvData.SpecialSkillUseId
                },
                new CDataPawnReaction()
                {
                    ReactionType = 6,
                    MotionNo = myPawnCsvData.PlayerDeathId
                },
                new CDataPawnReaction()
                {
                    ReactionType = 7,
                    MotionNo = myPawnCsvData.WaitingOnLobbyId
                },
                new CDataPawnReaction()
                {
                    ReactionType = 8,
                    MotionNo = myPawnCsvData.WaitingOnAdventureId
                },
                new CDataPawnReaction()
                {
                    ReactionType = 9,
                    MotionNo = myPawnCsvData.EndOfCombatId
                }
            };
            pawn.SpSkillList = new List<CDataSpSkill>()
            {
                new CDataSpSkill()
                {
                    SpSkillId = myPawnCsvData.SpSkillSlot1Id,
                    SpSkillLv = myPawnCsvData.SpSkillSlot1Lv
                },
                new CDataSpSkill()
                {
                    SpSkillId = myPawnCsvData.SpSkillSlot2Id,
                    SpSkillLv = myPawnCsvData.SpSkillSlot2Lv
                },
                new CDataSpSkill()
                {
                    SpSkillId = myPawnCsvData.SpSkillSlot3Id,
                    SpSkillLv = myPawnCsvData.SpSkillSlot3Lv
                }
            }.Where(spSkill => spSkill.SpSkillId != 0).ToList();
            return pawn;
        }

        private static byte[] data_Dump_Pawn35_3_16 = new byte[] /* 35.3.16 */
        {
            0x0, 0xDA, 0x5D, 0x4E, 0x0, 0x0, 0x0, 0x3, 0x0, 0xDA, 0x5D, 0x4E, 0x0, 0x0, 0x0, 0xC8,
            0x0, 0x0, 0x0, 0x22, 0x40, 0x7C, 0x1A, 0x6F, 0x40, 0x0, 0x0, 0x0, 0x46, 0x40, 0x39, 0xA5,
            0x40, 0x86, 0x1F, 0xFB, 0x0, 0x0, 0x0, 0x0, 0xBF, 0xB0, 0xA3, 0x6E, 0x2, 0x0, 0x0, 0x7,
            0x53, 0x65, 0x72, 0x65, 0x6C, 0x69, 0x61, 0x0, 0x0, 0x0, 0x0, 0x0, 0xF, 0x61, 0xAD, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x60, 0x7A, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0xF, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x21, 0xB9,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x22, 0x36, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x1, 0x0, 0x1, 0x1, 0x0, 0x20, 0x4F, 0xD8, 0x0,
            0x0, 0x0, 0x0, 0x3, 0x1, 0x2, 0x3, 0x0, 0x0, 0x0, 0x4, 0x1, 0x0, 0x0, 0x0, 0x3,
            0xA, 0x2, 0x0, 0x0, 0x0, 0x5, 0xA, 0x3, 0x0, 0x0, 0x0, 0x4, 0xA, 0x4, 0x0, 0x0,
            0x0, 0x6E, 0x1, 0x0, 0x0, 0x0, 0x2, 0x1, 0x0, 0x0, 0x0, 0x83, 0x6, 0x2, 0x0, 0x0,
            0x0, 0x88, 0x6, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x6,
            0x44, 0x3E, 0x0, 0x0, 0x44, 0x3E, 0x0, 0x0, 0x44, 0x3E, 0x0, 0x0, 0x43, 0xE1, 0x0, 0x0,
            0x43, 0xE1, 0x0, 0x0, 0x47, 0x43, 0x50, 0x0, 0x0, 0x63, 0x0, 0x0, 0x0, 0x0, 0x7, 0x6E,
            0x9, 0x4B, 0x0, 0x0, 0x0, 0xD8, 0x0, 0x0, 0x0, 0x57, 0x0, 0x0, 0x1, 0x44, 0x0, 0x0,
            0x0, 0xC6, 0x0, 0x0, 0x0, 0x1E, 0x0, 0x0, 0x0, 0x32, 0x0, 0x0, 0x0, 0x32, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x9, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x5,
            0xF6, 0x90, 0x0, 0x0, 0x2, 0x9E, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x10, 0x0, 0x0,
            0x0, 0x10, 0x0, 0x0, 0x0, 0xF, 0x0, 0x0, 0x0, 0x10, 0xFF, 0xFF, 0xFF, 0xFF, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x5, 0x1, 0x0, 0x33, 0x0, 0x0, 0x0, 0x0, 0x0, 0x3A, 0x49, 0xE0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x3, 0x8A, 0x40, 0xB, 0x0, 0x1, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x3, 0x0, 0x6, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x1A, 0x71, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x5, 0x78, 0x6, 0x0, 0x63,
            0x0, 0x0, 0x0, 0x0, 0x7, 0x6E, 0x9, 0x4B, 0x0, 0x0, 0x0, 0x0, 0x0, 0x5, 0xF6, 0x90,
            0xA, 0x0, 0x5D, 0x0, 0x0, 0x0, 0x0, 0x5, 0xD7, 0x6D, 0x38, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x7, 0x86, 0x90, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x3F, 0x80, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0xC2, 0xC8, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0xC2, 0xC8, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x9, 0x1, 0x0, 0x0, 0x0, 0x2,
            0x2, 0x0, 0x0, 0x0, 0x6, 0x3, 0x0, 0x0, 0x0, 0x1, 0x4, 0x0, 0x0, 0x0, 0x6, 0x5,
            0x0, 0x0, 0x0, 0x1, 0x6, 0x0, 0x0, 0x0, 0xA, 0x7, 0x0, 0x0, 0x0, 0x6, 0x8, 0x0,
            0x0, 0x0, 0x6, 0x9, 0x0, 0x0, 0x0, 0x6, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x3, 0x3, 0x1, 0x8, 0x2,
            0x9, 0x1, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x2, 0x7, 0xA4, 0x10, 0x3, 0x1, 0x1, 0x5B, 0x12, 0x0, 0x0, 0x2, 0x3, 0x5, 0x8, 0x0,
            0x45, 0x0, 0x45, 0x37, 0x37, 0xC, 0x74, 0x9A, 0x74, 0x7C, 0x76, 0xD4, 0x73, 0x85, 0x73, 0xB7,
            0x74, 0x13, 0x71, 0xCF, 0x75, 0xDC, 0x74, 0x82, 0x77, 0x1C, 0x75, 0x32, 0x74, 0xD1, 0x7C, 0x38,
            0x72, 0xD8, 0x75, 0x30, 0x75, 0x30, 0x75, 0xC3, 0x74, 0xF2, 0x74, 0x4F, 0x75, 0x2, 0x74, 0xE0,
            0x74, 0x9A, 0x72, 0x10, 0x72, 0xDC, 0x74, 0x4, 0x73, 0x20, 0x77, 0xD0, 0x71, 0x48, 0x73, 0x50,
            0x77, 0xB5, 0x75, 0xA2, 0x75, 0x30, 0x75, 0x30, 0x75, 0x30, 0x75, 0x30, 0x75, 0x30, 0xBB, 0xB2,
            0x9B, 0x35, 0x71, 0xFC, 0x8A, 0x98, 0x96, 0x96, 0x99, 0xF2, 0xA9, 0xB3, 0x96, 0xC8, 0x77, 0x88,
            0x72, 0x95, 0x70, 0x1C, 0x51, 0xA4, 0x75, 0x30, 0x61, 0x44, 0x0, 0x2, 0xC5, 0x0, 0x0, 0x3,
            0x2E, 0x0, 0x0, 0x2, 0xC9, 0x0, 0x0, 0x2
        };

        private static Packet Pawn35_3_16 = new Packet(new PacketId(35, 3, 16, "S2C_CONTEXT_35_3_16_NTC"), data_Dump_Pawn35_3_16);
    }
}