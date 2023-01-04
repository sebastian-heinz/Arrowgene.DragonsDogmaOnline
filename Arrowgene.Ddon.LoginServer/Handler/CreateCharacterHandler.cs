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
            character.Id = packet.Structure.CharacterInfo.CharacterId;
            character.UserId = packet.Structure.CharacterInfo.UserId;
            character.Version = packet.Structure.CharacterInfo.Version;
            character.FirstName = packet.Structure.CharacterInfo.FirstName;
            character.LastName = packet.Structure.CharacterInfo.LastName;
            character.EditInfo = packet.Structure.CharacterInfo.EditInfo;
            character.StatusInfo = packet.Structure.CharacterInfo.StatusInfo;
            character.Job = packet.Structure.CharacterInfo.Job;
            character.CharacterJobDataList = packet.Structure.CharacterInfo.CharacterJobDataList;
            //character.PlayPointList = packet.Structure.CharacterInfo.PlayPointList;
            character.Equipment = new Equipment(new Dictionary<JobId, Dictionary<EquipType, List<Item>>>()
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
                                            UId = Item.GenerateEquipItemUId(),
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
                                            UId = Item.GenerateEquipItemUId(),
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
            });
            character.CharacterEquipJobItemListDictionary = new Dictionary<JobId, List<CDataEquipJobItem>>()
            {
                { packet.Structure.CharacterInfo.Job, packet.Structure.CharacterInfo.CharacterEquipJobItemList }
            };
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
            character.Equipment = new Equipment(Server.AssetRepository.ArisenAsset.Select(arisenPreset => new Tuple<JobId, Dictionary<EquipType, List<Item>>>(arisenPreset.Job, new Dictionary<EquipType, List<Item>>() {
                {
                    EquipType.Performance,
                    new List<Item>() {
                        new Item {
                            UId = Item.GenerateEquipItemUId(),
                            ItemId = arisenPreset.PrimaryWeapon,
                            Unk3 = 0,
                            Color = arisenPreset.PrimaryWeaponColour,
                            PlusValue = 0,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = arisenPreset.PWCrest1,
                                    u2 = (ushort) (arisenPreset.PWC1Add1 << 8 | arisenPreset.PWC1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = arisenPreset.PWCrest2,
                                    u2 = (ushort) (arisenPreset.PWC2Add1 << 8 | arisenPreset.PWC2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = arisenPreset.PWCrest3,
                                    u2 = (ushort) (arisenPreset.PWC3Add1 << 8 | arisenPreset.PWC3Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 4,
                                    u1 = arisenPreset.PWCrest4,
                                    u2 = (ushort) (arisenPreset.PWC4Add1 << 8 | arisenPreset.PWC4Add2),
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
                            UId = Item.GenerateEquipItemUId(),
                            ItemId = arisenPreset.SecondaryWeapon,
                            Unk3 = 0,
                            Color = arisenPreset.SecondaryWeaponColour
                        },
                        new Item {
                            UId = Item.GenerateEquipItemUId(),
                            ItemId = arisenPreset.Head,
                            Unk3 = 0,
                            Color = arisenPreset.HeadColour,
                            PlusValue = 3,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = arisenPreset.HeadCrest1,
                                    u2 = (ushort) (arisenPreset.HC1Add1 << 8 | arisenPreset.HC1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = arisenPreset.HeadCrest2,
                                    u2 = (ushort) (arisenPreset.HC2Add1 << 8 | arisenPreset.HC2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = arisenPreset.HeadCrest3,
                                    u2 = (ushort) (arisenPreset.HC3Add1 << 8 | arisenPreset.HC3Add2),
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
                            UId = Item.GenerateEquipItemUId(),
                            ItemId = arisenPreset.Body,
                            Unk3 = 0,
                            Color = arisenPreset.BodyColour,
                            PlusValue = 4,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = arisenPreset.BodyCrest1,
                                    u2 = (ushort) (arisenPreset.BC1Add1 << 8 | arisenPreset.BC1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = arisenPreset.BodyCrest2,
                                    u2 = (ushort) (arisenPreset.BC2Add1 << 8 | arisenPreset.BC2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = arisenPreset.BodyCrest3,
                                    u2 = (ushort) (arisenPreset.BC3Add1 << 8 | arisenPreset.BC3Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 4,
                                    u1 = arisenPreset.BodyCrest4,
                                    u2 = (ushort) (arisenPreset.BC4Add1 << 8 | arisenPreset.BC4Add2),
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
                            UId = Item.GenerateEquipItemUId(),
                            ItemId = arisenPreset.Clothing,
                            Unk3 = 0,
                            Color = arisenPreset.ClothingColour
                        },
                        new Item {
                            UId = Item.GenerateEquipItemUId(),
                            ItemId = arisenPreset.Arm,
                            Unk3 = 0,
                            Color = arisenPreset.ArmColour,
                            PlusValue = 3,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = arisenPreset.ArmCrest1,
                                    u2 = (ushort) (arisenPreset.AC1Add1 << 8 | arisenPreset.AC1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = arisenPreset.ArmCrest2,
                                    u2 = (ushort) (arisenPreset.AC2Add1 << 8 | arisenPreset.AC2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = arisenPreset.ArmCrest3,
                                    u2 = (ushort) (arisenPreset.AC3Add1 << 8 | arisenPreset.AC3Add2),
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
                            UId = Item.GenerateEquipItemUId(),
                            ItemId = arisenPreset.Leg,
                            Unk3 = 0,
                            Color = arisenPreset.LegColour,
                            PlusValue = 3,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = arisenPreset.LegCrest1,
                                    u2 = (ushort) (arisenPreset.LC1Add1 << 8 | arisenPreset.LC1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = arisenPreset.LegCrest2,
                                    u2 = (ushort) (arisenPreset.LC2Add1 << 8 | arisenPreset.LC2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = arisenPreset.LegCrest3,
                                    u2 = (ushort) (arisenPreset.LC3Add1 << 8 | arisenPreset.LC3Add2),
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
                            UId = Item.GenerateEquipItemUId(),
                            ItemId = arisenPreset.Legwear,
                            Unk3 = 0,
                            Color = arisenPreset.LegwearColour
                        },
                        new Item {
                            UId = Item.GenerateEquipItemUId(),
                            ItemId = arisenPreset.Overwear,
                            Unk3 = 0,
                            Color = arisenPreset.OverwearColour
                        },
                        new Item {
                            UId = Item.GenerateEquipItemUId(),
                            ItemId = arisenPreset.Jewelry1,
                            Unk3 = 0,
                            Color = 0,
                            PlusValue = 0,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = arisenPreset.J1Crest1,
                                    u2 = (ushort) (arisenPreset.J1C1Add1 << 8 | arisenPreset.J1C1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = arisenPreset.J1Crest2,
                                    u2 = (ushort) (arisenPreset.J1C2Add1 << 8 | arisenPreset.J1C2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = arisenPreset.J1Crest3,
                                    u2 = (ushort) (arisenPreset.J1C3Add1 << 8 | arisenPreset.J1C3Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 4,
                                    u1 = arisenPreset.J1Crest4,
                                    u2 = (ushort) (arisenPreset.J1C4Add1 << 8 | arisenPreset.J1C4Add2),
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
                            UId = Item.GenerateEquipItemUId(),
                            ItemId = arisenPreset.Jewelry2,
                            Unk3 = 0,
                            Color = 0,
                            PlusValue = 0,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = arisenPreset.J2Crest1,
                                    u2 = (ushort) (arisenPreset.J2C1Add1 << 8 | arisenPreset.J2C1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = arisenPreset.J2Crest2,
                                    u2 = (ushort) (arisenPreset.J2C2Add1 << 8 | arisenPreset.J2C2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = arisenPreset.J2Crest3,
                                    u2 = (ushort) (arisenPreset.J2C3Add1 << 8 | arisenPreset.J2C3Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 4,
                                    u1 = arisenPreset.J2Crest4,
                                    u2 = (ushort) (arisenPreset.J2C4Add1 << 8 | arisenPreset.J2C4Add2),
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
                            UId = Item.GenerateEquipItemUId(),
                            ItemId = arisenPreset.Jewelry3,
                            Unk3 = 0,
                            Color = 0,
                            PlusValue = 0,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = arisenPreset.J3Crest1,
                                    u2 = (ushort) (arisenPreset.J3C1Add1 << 8 | arisenPreset.J3C1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = arisenPreset.J3Crest2,
                                    u2 = (ushort) (arisenPreset.J3C2Add1 << 8 | arisenPreset.J3C2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = arisenPreset.J3Crest3,
                                    u2 = (ushort) (arisenPreset.J3C3Add1 << 8 | arisenPreset.J3C3Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 4,
                                    u1 = arisenPreset.J3Crest4,
                                    u2 = (ushort) (arisenPreset.J3C4Add1 << 8 | arisenPreset.J3C4Add2),
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
                            UId = Item.GenerateEquipItemUId(),
                            ItemId = arisenPreset.Jewelry4,
                            Unk3 = 0,
                            Color = 0,
                            PlusValue = 0,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = arisenPreset.J4Crest1,
                                    u2 = (ushort) (arisenPreset.J4C1Add1 << 8 | arisenPreset.J4C1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = arisenPreset.J4Crest2,
                                    u2 = (ushort) (arisenPreset.J4C2Add1 << 8 | arisenPreset.J4C2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = arisenPreset.J4Crest3,
                                    u2 = (ushort) (arisenPreset.J4C3Add1 << 8 | arisenPreset.J4C3Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 4,
                                    u1 = arisenPreset.J4Crest4,
                                    u2 = (ushort) (arisenPreset.J4C4Add1 << 8 | arisenPreset.J4C4Add2),
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
                            UId = Item.GenerateEquipItemUId(),
                            ItemId = arisenPreset.Jewelry5,
                            Unk3 = 0,
                            Color = 0,
                            PlusValue = 0,
                            WeaponCrestDataList = new List<CDataWeaponCrestData>() {
                                new CDataWeaponCrestData {
                                    u0 = 1,
                                    u1 = arisenPreset.J5Crest1,
                                    u2 = (ushort) (arisenPreset.J5C1Add1 << 8 | arisenPreset.J5C1Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 2,
                                    u1 = arisenPreset.J5Crest2,
                                    u2 = (ushort) (arisenPreset.J5C2Add1 << 8 | arisenPreset.J5C2Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 3,
                                    u1 = arisenPreset.J5Crest3,
                                    u2 = (ushort) (arisenPreset.J5C3Add1 << 8 | arisenPreset.J5C3Add2),
                                },
                                new CDataWeaponCrestData {
                                    u0 = 4,
                                    u1 = arisenPreset.J5Crest4,
                                    u2 = (ushort) (arisenPreset.J5C4Add1 << 8 | arisenPreset.J5C4Add2),
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
                            UId = Item.GenerateEquipItemUId(),
                            ItemId = arisenPreset.Lantern,
                            Unk3 = 0,
                        }
                    }
                },
                {
                    EquipType.Visual,
                    new List<Item>() {
                        new Item {
                            UId = Item.GenerateEquipItemUId(),
                            ItemId = arisenPreset.VPrimaryWeapon,
                            Unk3 = 0,
                            Color = arisenPreset.VPrimaryWeaponColour
                        },
                        new Item {
                            UId = Item.GenerateEquipItemUId(),
                            ItemId = arisenPreset.VSecondaryWeapon,
                            Unk3 = 0,
                            Color = arisenPreset.VSecondaryWeaponColour
                        },
                        new Item {
                            UId = Item.GenerateEquipItemUId(),
                            ItemId = arisenPreset.VHead,
                            Unk3 = 0,
                            Color = arisenPreset.VHeadColour
                        },
                        new Item {
                            UId = Item.GenerateEquipItemUId(),
                            ItemId = arisenPreset.VBody,
                            Unk3 = 0,
                            Color = arisenPreset.VBodyColour
                        },
                        new Item {
                            UId = Item.GenerateEquipItemUId(),
                            ItemId = arisenPreset.VClothing,
                            Unk3 = 0,
                            Color = arisenPreset.VClothingColour
                        },
                        new Item {
                            UId = Item.GenerateEquipItemUId(),
                            ItemId = arisenPreset.VArm,
                            Unk3 = 0,
                            Color = arisenPreset.VArmColour
                        },
                        new Item {
                            UId = Item.GenerateEquipItemUId(),
                            ItemId = arisenPreset.VLeg,
                            Unk3 = 0,
                            Color = arisenPreset.VLegColour
                        },
                        new Item {
                            UId = Item.GenerateEquipItemUId(),
                            ItemId = arisenPreset.VLegwear,
                            Unk3 = 0,
                            Color = arisenPreset.VLegwearColour
                        },
                        new Item {
                            UId = Item.GenerateEquipItemUId(),
                            ItemId = arisenPreset.VOverwear,
                            Unk3 = 0,
                            Color = arisenPreset.VOverwearColour,
                        }
                    }
                }
            })).ToDictionary(x => x.Item1, x => x.Item2));
            character.CharacterEquipJobItemListDictionary = Server.AssetRepository.ArisenAsset.Select(arisenPreset => new Tuple<JobId, List<CDataEquipJobItem>>(arisenPreset.Job, new List<CDataEquipJobItem>() {
                new CDataEquipJobItem {
                    JobItemId = arisenPreset.ClassItem1,
                    EquipSlotNo = 1
                },
                new CDataEquipJobItem {
                    JobItemId = arisenPreset.ClassItem2,
                    EquipSlotNo = 2
                }
            })).ToDictionary(x => x.Item1, x => x.Item2);
            character.HideEquipHead = ActiveJobPreset.DisplayHelmet;
            character.HideEquipLantern = ActiveJobPreset.DisplayLantern;
            character.HideEquipHeadPawn = Server.AssetRepository.MyPawnAsset[0].HideEquipHead;
            character.HideEquipLanternPawn = Server.AssetRepository.MyPawnAsset[0].HideEquipLantern;
            // TODO: Load from Arisen.csv or something
            character.NormalSkills = Server.AssetRepository.ArisenAsset.SelectMany(arisenPreset => new List<CDataNormalSkillParam>() {
                    new CDataNormalSkillParam() {
                    Job = arisenPreset.Job,
                    SkillNo = 1,
                    Index = 0,
                    PreSkillNo = 0
                },
                new CDataNormalSkillParam() {
                    Job = arisenPreset.Job,
                    SkillNo = 2,
                    Index = 0,
                    PreSkillNo = 0
                },
                new CDataNormalSkillParam() {
                    Job = arisenPreset.Job,
                    SkillNo = 3,
                    Index = 0,
                    PreSkillNo = 0
                }
            }).ToList();
            character.CustomSkills = Server.AssetRepository.ArisenAsset.SelectMany(arisenPreset => new List<CustomSkill>() {
                // Main Palette
                new CustomSkill() {
                    Job = arisenPreset.Job,
                    SlotNo = 1,
                    SkillId = arisenPreset.Cs1MpId,
                    SkillLv = arisenPreset.Cs1MpLv
                },
                new CustomSkill() {
                    Job = arisenPreset.Job,
                    SlotNo = 2,
                    SkillId = arisenPreset.Cs2MpId,
                    SkillLv = arisenPreset.Cs2MpLv
                },
                new CustomSkill() {
                    Job = arisenPreset.Job,
                    SlotNo = 3,
                    SkillId = arisenPreset.Cs3MpId,
                    SkillLv = arisenPreset.Cs3MpLv
                },
                new CustomSkill() {
                    Job = arisenPreset.Job,
                    SlotNo = 4,
                    SkillId = arisenPreset.Cs4MpId,
                    SkillLv = arisenPreset.Cs4MpLv
                },
                // Sub Palette
                new CustomSkill() {
                    Job = arisenPreset.Job,
                    SlotNo = (1<<4) | 1,
                    SkillId = arisenPreset.Cs1SpId,
                    SkillLv = arisenPreset.Cs1SpLv
                },
                new CustomSkill() {
                    Job = arisenPreset.Job,
                    SlotNo = (1<<4) | 2,
                    SkillId = arisenPreset.Cs2SpId,
                    SkillLv = arisenPreset.Cs2SpLv
                },
                new CustomSkill() {
                    Job = arisenPreset.Job,
                    SlotNo = (1<<4) | 3,
                    SkillId = arisenPreset.Cs3SpId,
                    SkillLv = arisenPreset.Cs3SpLv
                },
                new CustomSkill() {
                    Job = arisenPreset.Job,
                    SlotNo = (1<<4) | 4,
                    SkillId = arisenPreset.Cs4SpId,
                    SkillLv = arisenPreset.Cs4SpLv
                }
            }).ToList();
            character.Abilities = Server.AssetRepository.ArisenAsset.SelectMany(arisenPreset => new List<Ability>() {
                new Ability() {
                    EquippedToJob = arisenPreset.Job,
                    Job = arisenPreset.Ab1Jb,
                    SlotNo = 1,
                    AbilityId = arisenPreset.Ab1Id,
                    AbilityLv = arisenPreset.Ab1Lv
                },
                new Ability() {
                    EquippedToJob = arisenPreset.Job,
                    Job = arisenPreset.Ab2Jb,
                    SlotNo = 2,
                    AbilityId = arisenPreset.Ab2Id,
                    AbilityLv = arisenPreset.Ab2Lv
                },
                new Ability() {
                    EquippedToJob = arisenPreset.Job,
                    Job = arisenPreset.Ab3Jb,
                    SlotNo = 3,
                    AbilityId = arisenPreset.Ab3Id,
                    AbilityLv = arisenPreset.Ab3Lv
                },
                new Ability() {
                    EquippedToJob = arisenPreset.Job,
                    Job = arisenPreset.Ab4Jb,
                    SlotNo = 4,
                    AbilityId = arisenPreset.Ab4Id,
                    AbilityLv = arisenPreset.Ab4Lv
                },
                new Ability() {
                    EquippedToJob = arisenPreset.Job,
                    Job = arisenPreset.Ab5Jb,
                    SlotNo = 5,
                    AbilityId = arisenPreset.Ab5Id,
                    AbilityLv = arisenPreset.Ab5Lv
                },
                new Ability() {
                    EquippedToJob = arisenPreset.Job,
                    Job = arisenPreset.Ab6Jb,
                    SlotNo = 6,
                    AbilityId = arisenPreset.Ab6Id,
                    AbilityLv = arisenPreset.Ab6Lv
                },
                new Ability() {
                    EquippedToJob = arisenPreset.Job,
                    Job = arisenPreset.Ab7Jb,
                    SlotNo = 7,
                    AbilityId = arisenPreset.Ab7Id,
                    AbilityLv = arisenPreset.Ab7Lv
                },
                new Ability() {
                    EquippedToJob = arisenPreset.Job,
                    Job = arisenPreset.Ab8Jb,
                    SlotNo = 8,
                    AbilityId = arisenPreset.Ab8Id,
                    AbilityLv = arisenPreset.Ab8Lv
                },
                new Ability() {
                    EquippedToJob = arisenPreset.Job,
                    Job = arisenPreset.Ab9Jb,
                    SlotNo = 9,
                    AbilityId = arisenPreset.Ab9Id,
                    AbilityLv = arisenPreset.Ab9Lv
                },
                new Ability() {
                    EquippedToJob = arisenPreset.Job,
                    Job = arisenPreset.Ab10Jb,
                    SlotNo = 10,
                    AbilityId = arisenPreset.Ab10Id,
                    AbilityLv = arisenPreset.Ab10Lv
                }
            }).ToList();
            character.Storage = new Storages(Server.AssetRepository.StorageAsset.ToDictionary(x => x.StorageType, x => x.SlotMax));

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
