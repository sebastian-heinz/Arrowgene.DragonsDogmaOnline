using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.LoginServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class GetCharacterListHandler : PacketHandler<LoginClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GetCharacterListHandler));


        public GetCharacterListHandler(DdonLoginServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_GET_CHARACTER_LIST_REQ;

        public override void Handle(LoginClient client, IPacket packet)
        {
            IBuffer buffer = new StreamBuffer();
            buffer.WriteInt32(0, Endianness.Big); // error
            buffer.WriteInt32(0, Endianness.Big); // result

            List<CDataCharacterListInfo> characterListResponse = new List<CDataCharacterListInfo>();
            List<Character> characters = Database.SelectCharactersByAccountId(client.Account.Id);
            Logger.Info(client, $"Found: {characters.Count} Characters");
            foreach (Character c in characters)
            {
                CDataCharacterListInfo cResponse = new CDataCharacterListInfo();
                cResponse.CharacterListElement.CommunityCharacterBaseInfo.CharacterId = (uint)c.Id;
                cResponse.CharacterListElement.CommunityCharacterBaseInfo.CharacterName.FirstName = c.FirstName;
                cResponse.CharacterListElement.CommunityCharacterBaseInfo.CharacterName.LastName = c.LastName;
                cResponse.CharacterListElement.CurrentJobBaseInfo.Job = Server.AssetRepository.ArisenAsset[0].Job;
                cResponse.CharacterListElement.CurrentJobBaseInfo.Level = (byte) Server.AssetRepository.ArisenAsset[0].Lv;
                cResponse.EditInfo = c.Visual;
                cResponse.EquipItemInfo = new List<CDataEquipItemInfo>() {
                    new CDataEquipItemInfo {
                        ItemId = Server.AssetRepository.ArisenAsset[0].VPrimaryWeapon,
                        Unk0 = 0,
                        EquipType = 2,
                        EquipSlot = 1,
                        Color = Server.AssetRepository.ArisenAsset[0].VPrimaryWeaponColour
                    },
                    new CDataEquipItemInfo {
                        ItemId = Server.AssetRepository.ArisenAsset[0].VSecondaryWeapon,
                        Unk0 = 0,
                        EquipType = 2,
                        EquipSlot = 2,
                        Color = Server.AssetRepository.ArisenAsset[0].VSecondaryWeaponColour
                    },
                    new CDataEquipItemInfo {
                        ItemId = Server.AssetRepository.ArisenAsset[0].VHead,
                        Unk0 = 0,
                        EquipType = 2,
                        EquipSlot = 3,
                        Color = Server.AssetRepository.ArisenAsset[0].VHeadColour
                    },
                    new CDataEquipItemInfo {
                        ItemId = Server.AssetRepository.ArisenAsset[0].VBody,
                        Unk0 = 0,
                        EquipType = 2,
                        EquipSlot = 4,
                        Color = Server.AssetRepository.ArisenAsset[0].VBodyColour
                    },
                    new CDataEquipItemInfo {
                        ItemId = Server.AssetRepository.ArisenAsset[0].VClothing,
                        Unk0 = 0,
                        EquipType = 2,
                        EquipSlot = 5,
                        Color = Server.AssetRepository.ArisenAsset[0].VClothingColour
                    },
                    new CDataEquipItemInfo {
                        ItemId = Server.AssetRepository.ArisenAsset[0].VArm,
                        Unk0 = 0,
                        EquipType = 2,
                        EquipSlot = 6,
                        Color = Server.AssetRepository.ArisenAsset[0].VArmColour
                    },
                    new CDataEquipItemInfo {
                        ItemId = Server.AssetRepository.ArisenAsset[0].VLeg,
                        Unk0 = 0,
                        EquipType = 2,
                        EquipSlot = 7,
                        Color = Server.AssetRepository.ArisenAsset[0].VLegColour
                    },
                    new CDataEquipItemInfo {
                        ItemId = Server.AssetRepository.ArisenAsset[0].VLegwear,
                        Unk0 = 0,
                        EquipType = 2,
                        EquipSlot = 8,
                        Color = Server.AssetRepository.ArisenAsset[0].VLegwearColour
                    },
                    new CDataEquipItemInfo {
                        ItemId = Server.AssetRepository.ArisenAsset[0].VOverwear,
                        Unk0 = 0,
                        EquipType = 2,
                        EquipSlot = 9,
                        Color = Server.AssetRepository.ArisenAsset[0].VOverwearColour
                    },
                    new CDataEquipItemInfo {
                        ItemId = Server.AssetRepository.ArisenAsset[0].PrimaryWeapon,
                        Unk0 = 0,
                        EquipType = 1,
                        EquipSlot = 1,
                        Color = Server.AssetRepository.ArisenAsset[0].PrimaryWeaponColour,
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
                        ItemId = Server.AssetRepository.ArisenAsset[0].SecondaryWeapon,
                        Unk0 = 0,
                        EquipType = 1,
                        EquipSlot = 2,
                        Color = Server.AssetRepository.ArisenAsset[0].SecondaryWeaponColour
                    },
                    new CDataEquipItemInfo {
                        ItemId = Server.AssetRepository.ArisenAsset[0].Head,
                        Unk0 = 0,
                        EquipType = 1,
                        EquipSlot = 3,
                        Color = Server.AssetRepository.ArisenAsset[0].HeadColour,
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
                        ItemId = Server.AssetRepository.ArisenAsset[0].Body,
                        Unk0 = 0,
                        EquipType = 1,
                        EquipSlot = 4,
                        Color = Server.AssetRepository.ArisenAsset[0].BodyColour,
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
                        ItemId = Server.AssetRepository.ArisenAsset[0].Clothing,
                        Unk0 = 0,
                        EquipType = 1,
                        EquipSlot = 5,
                        Color = Server.AssetRepository.ArisenAsset[0].ClothingColour
                    },
                    new CDataEquipItemInfo {
                        ItemId = Server.AssetRepository.ArisenAsset[0].Arm,
                        Unk0 = 0,
                        EquipType = 1,
                        EquipSlot = 6,
                        Color = Server.AssetRepository.ArisenAsset[0].ArmColour,
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
                        ItemId = Server.AssetRepository.ArisenAsset[0].Leg,
                        Unk0 = 0,
                        EquipType = 1,
                        EquipSlot = 7,
                        Color = Server.AssetRepository.ArisenAsset[0].LegColour,
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
                        ItemId = Server.AssetRepository.ArisenAsset[0].Legwear,
                        Unk0 = 0,
                        EquipType = 1,
                        EquipSlot = 8,
                        Color = Server.AssetRepository.ArisenAsset[0].LegwearColour
                    },
                    new CDataEquipItemInfo {
                        ItemId = Server.AssetRepository.ArisenAsset[0].Overwear,
                        Unk0 = 0,
                        EquipType = 1,
                        EquipSlot = 9,
                        Color = Server.AssetRepository.ArisenAsset[0].OverwearColour
                    },
                    new CDataEquipItemInfo {
                        ItemId = Server.AssetRepository.ArisenAsset[0].Jewelry1,
                        Unk0 = 0,
                        EquipType = 1,
                        EquipSlot = 10,
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
                        ItemId = Server.AssetRepository.ArisenAsset[0].Jewelry2,
                        Unk0 = 0,
                        EquipType = 1,
                        EquipSlot = 11,
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
                        ItemId = Server.AssetRepository.ArisenAsset[0].Jewelry3,
                        Unk0 = 0,
                        EquipType = 1,
                        EquipSlot = 12,
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
                        ItemId = Server.AssetRepository.ArisenAsset[0].Jewelry4,
                        Unk0 = 0,
                        EquipType = 1,
                        EquipSlot = 13,
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
                        ItemId = Server.AssetRepository.ArisenAsset[0].Jewelry5,
                        Unk0 = 0,
                        EquipType = 1,
                        EquipSlot = 14,
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
                        ItemId = Server.AssetRepository.ArisenAsset[0].Lantern,
                        Unk0 = 0,
                        EquipType = 1,
                        EquipSlot = 15
                    }
                };

                characterListResponse.Add(cResponse);
            }
            
            EntitySerializer.Get<CDataCharacterListInfo>().WriteList(buffer, characterListResponse);
            Packet response = new Packet(PacketId.L2C_GET_CHARACTER_LIST_RES, buffer.GetAllBytes());
            client.Send(response);
        }
    }
}
