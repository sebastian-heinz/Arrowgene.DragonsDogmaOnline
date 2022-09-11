using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Entity;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnJoinPartyMypawnHandler : StructurePacketHandler<GameClient, C2SPawnJoinPartyMypawnReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnJoinPartyMypawnHandler));


        public PawnJoinPartyMypawnHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPawnJoinPartyMypawnReq> req)
        {
            S2CContextGetPartyMypawnContextNtc pcapPawn = EntitySerializer.Get<S2CContextGetPartyMypawnContextNtc>().Read(SelectedDump.data_Dump_Pawn35_3_16);

            MyPawnCsv myPawnCsvData = Server.AssetRepository.MyPawnAsset[req.Structure.PawnNumber-1];
            Pawn pawn = new Pawn();
            pawn.Owner = client;
            pawn.Party = client.Party;
            pawn.HmType = myPawnCsvData.HmType;
            pawn.PawnType = myPawnCsvData.PawnType;
            pawn.Character.Stage = pawn.Character.Stage;
            pawn.Character.StageNo = pawn.Character.StageNo;
            pawn.Character.X = pawn.Character.X;
            pawn.Character.Y = pawn.Character.Y;
            pawn.Character.Z = pawn.Character.Z;
            pawn.Character.Id = myPawnCsvData.PawnId;
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
            pawn.Character.CustomSkills = new List<CDataSetAcquirementParam>() {
                // Main Palette
                new CDataSetAcquirementParam() {
                    Job = myPawnCsvData.Job,
                    Type = 0,
                    SlotNo = 1,
                    AcquirementNo = myPawnCsvData.CustomSkillId1,
                    AcquirementLv = myPawnCsvData.CustomSkillLv1
                },
                new CDataSetAcquirementParam() {
                    Job = myPawnCsvData.Job,
                    Type = 0,
                    SlotNo = 2,
                    AcquirementNo = myPawnCsvData.CustomSkillId2,
                    AcquirementLv = myPawnCsvData.CustomSkillLv2
                },
                new CDataSetAcquirementParam() {
                    Job = myPawnCsvData.Job,
                    Type = 0,
                    SlotNo = 3,
                    AcquirementNo = myPawnCsvData.CustomSkillId3,
                    AcquirementLv = myPawnCsvData.CustomSkillLv3
                },
                new CDataSetAcquirementParam() {
                    Job = myPawnCsvData.Job,
                    Type = 0,
                    SlotNo = 4,
                    AcquirementNo = myPawnCsvData.CustomSkillId4,
                    AcquirementLv = myPawnCsvData.CustomSkillLv4
                }
            };
            pawn.Character.Abilities = new List<CDataSetAcquirementParam>() {
                new CDataSetAcquirementParam() {
                    Job = 0,
                    Type = 0,
                    SlotNo = 1,
                    AcquirementNo = myPawnCsvData.AbilityId1,
                    AcquirementLv = myPawnCsvData.AbilityLv1
                },
                new CDataSetAcquirementParam() {
                    Job = 0,
                    Type = 0,
                    SlotNo = 2,
                    AcquirementNo = myPawnCsvData.AbilityId2,
                    AcquirementLv = myPawnCsvData.AbilityLv2
                },
                new CDataSetAcquirementParam() {
                    Job = 0,
                    Type = 0,
                    SlotNo = 3,
                    AcquirementNo = myPawnCsvData.AbilityId3,
                    AcquirementLv = myPawnCsvData.AbilityLv3
                },
                new CDataSetAcquirementParam() {
                    Job = 0,
                    Type = 0,
                    SlotNo = 4,
                    AcquirementNo = myPawnCsvData.AbilityId4,
                    AcquirementLv = myPawnCsvData.AbilityLv4
                },
                new CDataSetAcquirementParam() {
                    Job = 0,
                    Type = 0,
                    SlotNo = 5,
                    AcquirementNo = myPawnCsvData.AbilityId5,
                    AcquirementLv = myPawnCsvData.AbilityLv5
                },
                new CDataSetAcquirementParam() {
                    Job = 0,
                    Type = 0,
                    SlotNo = 6,
                    AcquirementNo = myPawnCsvData.AbilityId6,
                    AcquirementLv = myPawnCsvData.AbilityLv6
                },
                new CDataSetAcquirementParam() {
                    Job = 0,
                    Type = 0,
                    SlotNo = 7,
                    AcquirementNo = myPawnCsvData.AbilityId7,
                    AcquirementLv = myPawnCsvData.AbilityLv7
                },
                new CDataSetAcquirementParam() {
                    Job = 0,
                    Type = 0,
                    SlotNo = 8,
                    AcquirementNo = myPawnCsvData.AbilityId8,
                    AcquirementLv = myPawnCsvData.AbilityLv8
                },
                new CDataSetAcquirementParam() {
                    Job = 0,
                    Type = 0,
                    SlotNo = 9,
                    AcquirementNo = myPawnCsvData.AbilityId9,
                    AcquirementLv = myPawnCsvData.AbilityLv9
                },
                new CDataSetAcquirementParam() {
                    Job = 0,
                    Type = 0,
                    SlotNo = 10,
                    AcquirementNo = myPawnCsvData.AbilityId10,
                    AcquirementLv = myPawnCsvData.AbilityLv10
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

            client.Party.Members.Add(pawn);

            S2CPawnJoinPartyPawnNtc joinPartyPawnNtc = new S2CPawnJoinPartyPawnNtc();
            joinPartyPawnNtc.PartyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterId = client.Character.Id;
            joinPartyPawnNtc.PartyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterName.FirstName = pawn.Character.FirstName;
            joinPartyPawnNtc.PartyMember.CharacterListElement.CurrentJobBaseInfo.Job = pawn.Character.Job;
            joinPartyPawnNtc.PartyMember.CharacterListElement.CurrentJobBaseInfo.Level = (byte) pawn.Character.ActiveCharacterJobData.Lv;
            joinPartyPawnNtc.PartyMember.MemberType = 2;
            joinPartyPawnNtc.PartyMember.MemberIndex = client.Party.Members.IndexOf(pawn);
            joinPartyPawnNtc.PartyMember.PawnId = pawn.Character.Id;
            joinPartyPawnNtc.PartyMember.IsLeader = false;
            joinPartyPawnNtc.PartyMember.IsPawn = true;
            joinPartyPawnNtc.PartyMember.IsPlayEntry = false;
            joinPartyPawnNtc.PartyMember.JoinState = JoinState.On;
            joinPartyPawnNtc.PartyMember.AnyValueList = new byte[] {0x0, 0xDA, 0x5D, 0x4E, 0x0, 0x1, 0x0, 0x2}; // Taken from pcaps
            joinPartyPawnNtc.PartyMember.SessionStatus = 0;
            client.Party.SendToAll(joinPartyPawnNtc);

            S2CContextGetPartyMypawnContextNtc mypawnContextNtc = new S2CContextGetPartyMypawnContextNtc(pawn);
            mypawnContextNtc.Context.Base.MemberIndex = client.Party.Members.IndexOf(pawn);
            client.Party.SendToAll(mypawnContextNtc);

            S2CPawnJoinPartyMypawnRes res = new S2CPawnJoinPartyMypawnRes();
            client.Send(res);
        }
    }
}
