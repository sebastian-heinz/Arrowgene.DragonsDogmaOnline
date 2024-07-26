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
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Model.Quest;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class CreateCharacterHandler : StructurePacketHandler<LoginClient, C2LCreateCharacterDataReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CreateCharacterHandler));

        private readonly AssetRepository _AssetRepository;

        public CreateCharacterHandler(DdonLoginServer server) : base(server)
        {
            _AssetRepository = server.AssetRepository;
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
            character.EquipmentTemplate = new EquipmentTemplate(
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
            // Every new character starts with 1 jewlery slot, rest are bought from the dragon.
            character.JewelrySlotNum = 1; // packet.Structure.CharacterInfo.JewelrySlotNum;
            //character.CharacterItemSlotInfoList = packet.Structure.CharacterInfo.CharacterItemSlotInfoList;
            //character.UnkCharData0 = packet.Structure.CharacterInfo.UnkCharData0;
            //character.UnkCharData1 = packet.Structure.CharacterInfo.UnkCharData1;

            character.MyPawnSlotNum = 2;
            character.RentalPawnSlotNum = 0;

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
            character.EquipmentTemplate = new EquipmentTemplate(
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
                    Type = WalletType.SilverTickets,
                    Value = 0
                },
                new CDataWalletPoint() {
                    Type = WalletType.GoldenGemstones,
                    Value = 0
                },
                new CDataWalletPoint() {
                    Type = WalletType.RentalPoints,
                    Value = 0
                },
                new CDataWalletPoint() {
                    Type = WalletType.ResetJobPoints,
                    Value = 0
                },
                new CDataWalletPoint() {
                    Type = WalletType.ResetCraftSkills,
                    Value = 0
                },
                new CDataWalletPoint() {
                    Type = WalletType.HighOrbs,
                    Value = 0
                },
                new CDataWalletPoint() {
                    Type = WalletType.DominionPoints,
                    Value = 0
                },
                new CDataWalletPoint() {
                    Type = WalletType.AdventurePassPoints,
                    Value = 0
                },
                new CDataWalletPoint() {
                    Type = WalletType.UnknownTickets,
                    Value = 0
                },
                new CDataWalletPoint() {
                    Type = WalletType.CustomMadeServiceTickets,
                    Value = 0
                },
                new CDataWalletPoint() {
                    Type = WalletType.GoldenDragonMark,
                    Value = 0
                },
                new CDataWalletPoint() {
                    Type = WalletType.SilverDragonMark,
                    Value = 0
                },
                new CDataWalletPoint() {
                    Type = WalletType.RedDragonMark,
                    Value = 0
                }
            };
            // Value taken from pcaps
            // TODO: Not hardcode it
            character.FavWarpSlotNum = 10;

            // TODO: Figure out a proper default value and not hardcode it
            character.MaxBazaarExhibits = 5;

            // Add starting storage items
            foreach (var tuple in Server.AssetRepository.StorageItemAsset)
            {
                if(tuple.Item3.ItemId != 0)
                {
                    character.Storage.GetStorage(tuple.Item1).AddItem(tuple.Item3, tuple.Item2);
                }
            }

            // Add current job's equipment to the equipment storage
            List<Item?> performanceEquipItems = character.EquipmentTemplate.GetEquipment(character.Job, EquipType.Performance);
            for (int i = 0; i < performanceEquipItems.Count; i++)
            {
                Item? item = performanceEquipItems[i];
                ushort slot = (ushort)(i+1);
                character.Storage.GetStorage(StorageType.CharacterEquipment).SetItem(item, 1, slot);
            }

            List<Item?> visualEquipItems = character.EquipmentTemplate.GetEquipment(character.Job, EquipType.Visual);
            for (int i = 0; i < visualEquipItems.Count; i++)
            {
                Item? item = visualEquipItems[i];
                ushort slot = (ushort)(i+EquipmentTemplate.TOTAL_EQUIP_SLOTS+1);
                character.Storage.GetStorage(StorageType.CharacterEquipment).SetItem(item, 1, slot);
            }

            L2CCreateCharacterDataRes res = new L2CCreateCharacterDataRes();
            if (!Database.CreateCharacter(character))
            {
                Logger.Error(client, "Failed to create character");
                res.Result = 1;
                client.Send(res);
            }

            // Populate extra tables for the characters
            CDataOrbGainExtendParam ExtendParams = new CDataOrbGainExtendParam();
            if (!Database.InsertGainExtendParam(character.CommonId, ExtendParams))
            {
                Logger.Error(client, "Failed to create orb extend params");
                res.Result = 1;
                client.Send(res);
            }

            // Default unlock some secret abilities based on server admin desires
            foreach (var ability in _AssetRepository.SecretAbilitiesAsset.DefaultSecretAbilities)
            {
                Database.InsertSecretAbilityUnlock(character.CommonId, ability);
            }

            // Insert the first main quest to start the chain
            if (!Database.InsertQuestProgress(character.CommonId, QuestId.ResolutionsAndOmens, QuestType.Main, 0))
            {
                Logger.Error("Failed to seed first MSQ for player");
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
    }
}
