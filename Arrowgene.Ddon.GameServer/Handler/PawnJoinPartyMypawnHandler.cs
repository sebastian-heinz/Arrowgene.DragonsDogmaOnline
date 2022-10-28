using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Shared.Entity;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnJoinPartyMypawnHandler : GameStructurePacketHandler<C2SPawnJoinPartyMypawnReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnJoinPartyMypawnHandler));


        public PawnJoinPartyMypawnHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPawnJoinPartyMypawnReq> req)
        {
            S2CContextGetPartyMypawnContextNtc pcapPawn = EntitySerializer.Get<S2CContextGetPartyMypawnContextNtc>().Read(SelectedDump.data_Dump_Pawn35_3_16);

            MyPawnCsv myPawnCsvData = Server.AssetRepository.MyPawnAsset[req.Structure.PawnNumber-1];
            
            Pawn pawn = new Pawn(client.Character.Id);
            pawn.Id = myPawnCsvData.PawnId;
            pawn.Character.Id = client.Character.Id; // TODO
            pawn.CharacterId = client.Character.Id; // pawns characterId, refers to the owner
            pawn.Character.Server = Server.AssetRepository.ServerList[0];
            
            pawn.HmType = myPawnCsvData.HmType;
            pawn.PawnType = myPawnCsvData.PawnType;
            pawn.Character.FirstName = myPawnCsvData.Name;
            //pawn.Character.LastName = Pawns dont have Last Name
            pawn.Character.EditInfo = new CDataEditInfo()
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
            pawn.Character.Job = myPawnCsvData.Job;
            pawn.Character.HideEquipHead = myPawnCsvData.HideEquipHead;
            pawn.Character.HideEquipLantern = myPawnCsvData.HideEquipLantern;
            pawn.Character.StatusInfo = new CDataStatusInfo()
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
            pawn.Character.CharacterJobDataList = new List<CDataCharacterJobData>(){ new CDataCharacterJobData {
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
            pawn.Character.CharacterEquipDataListDictionary = new Dictionary<JobId, List<CDataCharacterEquipData>>() { { myPawnCsvData.Job, new List<CDataCharacterEquipData>() { new CDataCharacterEquipData() {
                    Equips = new List<CDataEquipItemInfo>() {
                        new CDataEquipItemInfo {
                            ItemId = myPawnCsvData.Primary,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 1,
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
                        new CDataEquipItemInfo {
                            ItemId = myPawnCsvData.Secondary,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 2,
                            Color = 0
                        },
                        new CDataEquipItemInfo {
                            ItemId = myPawnCsvData.Head,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 3,
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
                        new CDataEquipItemInfo {
                            ItemId = myPawnCsvData.Body,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 4,
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
                        new CDataEquipItemInfo {
                            ItemId = myPawnCsvData.BodyClothing,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 5,
                            Color = 0
                        },
                        new CDataEquipItemInfo {
                            ItemId = myPawnCsvData.Arm,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 6,
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
                        new CDataEquipItemInfo {
                            ItemId = myPawnCsvData.Leg,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 7,
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
                        new CDataEquipItemInfo {
                            ItemId = myPawnCsvData.LegWear,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 8,
                            Color = 0
                        },
                        new CDataEquipItemInfo {
                            ItemId = myPawnCsvData.OverWear,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 9,
                            Color = 0
                        },
                        new CDataEquipItemInfo {
                            ItemId = myPawnCsvData.JewelrySlot1,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 10,
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
                        new CDataEquipItemInfo {
                            ItemId = myPawnCsvData.JewelrySlot2,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 11,
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
                        new CDataEquipItemInfo {
                            ItemId = myPawnCsvData.JewelrySlot3,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 12,
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
                        new CDataEquipItemInfo {
                            ItemId = myPawnCsvData.JewelrySlot4,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 13,
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
                        new CDataEquipItemInfo {
                            ItemId = myPawnCsvData.JewelrySlot5,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 14,
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
                        new CDataEquipItemInfo {
                            ItemId = myPawnCsvData.Lantern,
                            Unk0 = 0,
                            EquipType = 1,
                            EquipSlot = 15
                        }
                    }
                }
            }}};
            pawn.Character.CharacterEquipViewDataListDictionary = new Dictionary<JobId, List<CDataCharacterEquipData>>() { { myPawnCsvData.Job, new List<CDataCharacterEquipData>() { new CDataCharacterEquipData() {
                    Equips = new List<CDataEquipItemInfo>() {
                        new CDataEquipItemInfo {
                            ItemId = myPawnCsvData.VPrimary,
                            Unk0 = 0,
                            EquipType = 2,
                            EquipSlot = 1,
                            Color = 0
                        },
                        new CDataEquipItemInfo {
                            ItemId = myPawnCsvData.VSecondary,
                            Unk0 = 0,
                            EquipType = 2,
                            EquipSlot = 2,
                            Color = 0
                        },
                        new CDataEquipItemInfo {
                            ItemId = myPawnCsvData.VHead,
                            Unk0 = 0,
                            EquipType = 2,
                            EquipSlot = 3,
                            Color = 0
                        },
                        new CDataEquipItemInfo {
                            ItemId = myPawnCsvData.VBody,
                            Unk0 = 0,
                            EquipType = 2,
                            EquipSlot = 4,
                            Color = 0
                        },
                        new CDataEquipItemInfo {
                            ItemId = myPawnCsvData.VBodyClothing,
                            Unk0 = 0,
                            EquipType = 2,
                            EquipSlot = 5,
                            Color = 0
                        },
                        new CDataEquipItemInfo {
                            ItemId = myPawnCsvData.VArm,
                            Unk0 = 0,
                            EquipType = 2,
                            EquipSlot = 6,
                            Color = 0
                        },
                        new CDataEquipItemInfo {
                            ItemId = myPawnCsvData.VLeg,
                            Unk0 = 0,
                            EquipType = 2,
                            EquipSlot = 7,
                            Color = 0
                        },
                        new CDataEquipItemInfo {
                            ItemId = myPawnCsvData.VLegWear,
                            Unk0 = 0,
                            EquipType = 2,
                            EquipSlot = 8,
                            Color = 0
                        },
                        new CDataEquipItemInfo {
                            ItemId = myPawnCsvData.VOverWear,
                            Unk0 = 0,
                            EquipType = 2,
                            EquipSlot = 9,
                            Color = 0,
                        }
                    }
                }
            }}};
            pawn.Character.CharacterEquipJobItemListDictionary = new Dictionary<JobId, List<CDataEquipJobItem>>() { { myPawnCsvData.Job, new List<CDataEquipJobItem>() {
                new CDataEquipJobItem {
                    JobItemId = myPawnCsvData.JobItem1,
                    EquipSlotNo = myPawnCsvData.JobItemSlot1
                },
                new CDataEquipJobItem {
                    JobItemId = myPawnCsvData.JobItem2,
                    EquipSlotNo = myPawnCsvData.JobItemSlot2
                }
            }}};
            pawn.Character.NormalSkills = new List<CDataNormalSkillParam>() {
                new CDataNormalSkillParam() {
                    Job = myPawnCsvData.Job,
                    SkillNo = myPawnCsvData.NormalSkill1,
                    Index = 0,
                    PreSkillNo = 0
                },
                new CDataNormalSkillParam() {
                    Job = myPawnCsvData.Job,
                    SkillNo = myPawnCsvData.NormalSkill2,
                    Index = 0,
                    PreSkillNo = 0
                },
                new CDataNormalSkillParam() {
                    Job = myPawnCsvData.Job,
                    SkillNo = myPawnCsvData.NormalSkill3,
                    Index = 0,
                    PreSkillNo = 0
                }
            };
            pawn.Character.CustomSkills = new List<CustomSkill>() {
                // Main Palette
                new CustomSkill() {
                    Job = myPawnCsvData.Job,
                    SlotNo = 1,
                    SkillId = myPawnCsvData.CustomSkillId1,
                    SkillLv = myPawnCsvData.CustomSkillLv1
                },
                new CustomSkill() {
                    Job = myPawnCsvData.Job,
                    SlotNo = 2,
                    SkillId = myPawnCsvData.CustomSkillId2,
                    SkillLv = myPawnCsvData.CustomSkillLv2
                },
                new CustomSkill() {
                    Job = myPawnCsvData.Job,
                    SlotNo = 3,
                    SkillId = myPawnCsvData.CustomSkillId3,
                    SkillLv = myPawnCsvData.CustomSkillLv3
                },
                new CustomSkill() {
                    Job = myPawnCsvData.Job,
                    SlotNo = 4,
                    SkillId = myPawnCsvData.CustomSkillId4,
                    SkillLv = myPawnCsvData.CustomSkillLv4
                }
            };
            pawn.Character.Abilities = new List<Ability>() {
                new Ability() {
                    EquippedToJob = myPawnCsvData.Job,
                    Job = 0,
                    SlotNo = 1,
                    AbilityId = myPawnCsvData.AbilityId1,
                    AbilityLv = myPawnCsvData.AbilityLv1
                },
                new Ability() {
                    EquippedToJob = myPawnCsvData.Job,
                    Job = 0,
                    SlotNo = 2,
                    AbilityId = myPawnCsvData.AbilityId2,
                    AbilityLv = myPawnCsvData.AbilityLv2
                },
                new Ability() {
                    EquippedToJob = myPawnCsvData.Job,
                    Job = 0,
                    SlotNo = 3,
                    AbilityId = myPawnCsvData.AbilityId3,
                    AbilityLv = myPawnCsvData.AbilityLv3
                },
                new Ability() {
                    EquippedToJob = myPawnCsvData.Job,
                    Job = 0,
                    SlotNo = 4,
                    AbilityId = myPawnCsvData.AbilityId4,
                    AbilityLv = myPawnCsvData.AbilityLv4
                },
                new Ability() {
                    EquippedToJob = myPawnCsvData.Job,
                    Job = 0,
                    SlotNo = 5,
                    AbilityId = myPawnCsvData.AbilityId5,
                    AbilityLv = myPawnCsvData.AbilityLv5
                },
                new Ability() {
                    EquippedToJob = myPawnCsvData.Job,
                    Job = 0,
                    SlotNo = 6,
                    AbilityId = myPawnCsvData.AbilityId6,
                    AbilityLv = myPawnCsvData.AbilityLv6
                },
                new Ability() {
                    EquippedToJob = myPawnCsvData.Job,
                    Job = 0,
                    SlotNo = 7,
                    AbilityId = myPawnCsvData.AbilityId7,
                    AbilityLv = myPawnCsvData.AbilityLv7
                },
                new Ability() {
                    EquippedToJob = myPawnCsvData.Job,
                    Job = 0,
                    SlotNo = 8,
                    AbilityId = myPawnCsvData.AbilityId8,
                    AbilityLv = myPawnCsvData.AbilityLv8
                },
                new Ability() {
                    EquippedToJob = myPawnCsvData.Job,
                    Job = 0,
                    SlotNo = 9,
                    AbilityId = myPawnCsvData.AbilityId9,
                    AbilityLv = myPawnCsvData.AbilityLv9
                },
                new Ability() {
                    EquippedToJob = myPawnCsvData.Job,
                    Job = 0,
                    SlotNo = 10,
                    AbilityId = myPawnCsvData.AbilityId10,
                    AbilityLv = myPawnCsvData.AbilityLv10
                }
            };
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
            };

            PawnPartyMember partyMember = client.Party.Join(pawn);
            if (partyMember == null)
            {
                Logger.Error(client,
                    $"could not join pawn");
                // TODO error response
                return;
            }
            client.Party.SendToAll(new S2CPawnJoinPartyPawnNtc() { PartyMember = partyMember.GetCDataPartyMember() });
            client.Party.SendToAll(partyMember.GetS2CContextGetParty_ContextNtc());

            S2CPawnJoinPartyMypawnRes res = new S2CPawnJoinPartyMypawnRes();
            client.Send(res);
        }
    }
}
