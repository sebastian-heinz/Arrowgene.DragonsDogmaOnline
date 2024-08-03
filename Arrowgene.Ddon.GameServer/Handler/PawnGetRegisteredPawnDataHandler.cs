using System;
using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetRegisteredPawnDataHandler : GameRequestPacketHandler<C2SPawnGetRegisteredPawnDataReq, S2CPawnGetRegisteredPawnDataRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetRegisteredPawnDataHandler));

        public PawnGetRegisteredPawnDataHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnGetRegisteredPawnDataRes Handle(GameClient client, C2SPawnGetRegisteredPawnDataReq request)
        {
            S2CPawnGetRegisteredPawnDataRes res = new S2CPawnGetRegisteredPawnDataRes();
            res.PawnId = (uint)request.PawnId;

            // TODO: Figure out what registered pawns are and why this seems to be requested only sometimes
            // These are the pawns that show up in the Pawn Cathedral and towns at random
            if (Server.AssetRepository.MyPawnAsset.Count == 0)
            {
                Logger.Error("No pawns found in MyPawn asset");
            }
            else
            {
                MyPawnCsv pawnAsset = Server.AssetRepository.MyPawnAsset[Random.Shared.Next(Server.AssetRepository.MyPawnAsset.Count)];
                res.PawnInfo.Version = 0;
                res.PawnInfo.Name = pawnAsset.Name;
                res.PawnInfo.EditInfo = new CDataEditInfo()
                {
                    Sex = pawnAsset.Sex,
                    Voice = pawnAsset.Voice,
                    VoicePitch = pawnAsset.VoicePitch,
                    Personality = pawnAsset.Personality,
                    SpeechFreq = pawnAsset.SpeechFreq,
                    BodyType = pawnAsset.BodyType,
                    Hair = pawnAsset.Hair,
                    Beard = pawnAsset.Beard,
                    Makeup = pawnAsset.Makeup,
                    Scar = pawnAsset.Scar,
                    EyePresetNo = pawnAsset.EyePresetNo,
                    NosePresetNo = pawnAsset.NosePresetNo,
                    MouthPresetNo = pawnAsset.MouthPresetNo,
                    EyebrowTexNo = pawnAsset.EyebrowTexNo,
                    ColorSkin = pawnAsset.ColorSkin,
                    ColorHair = pawnAsset.ColorHair,
                    ColorBeard = pawnAsset.ColorBeard,
                    ColorEyebrow = pawnAsset.ColorEyebrow,
                    ColorREye = pawnAsset.ColorREye,
                    ColorLEye = pawnAsset.ColorLEye,
                    ColorMakeup = pawnAsset.ColorMakeup,
                    Sokutobu = pawnAsset.Sokutobu,
                    Hitai = pawnAsset.Hitai,
                    MimiJyouge = pawnAsset.MimiJyouge,
                    Kannkaku = pawnAsset.Kannkaku,
                    MabisasiJyouge = pawnAsset.MabisasiJyouge,
                    HanakuchiJyouge = pawnAsset.HanakuchiJyouge,
                    AgoSakiHaba = pawnAsset.AgoSakiHaba,
                    AgoZengo = pawnAsset.AgoZengo,
                    AgoSakiJyouge = pawnAsset.AgoSakiJyouge,
                    HitomiOokisa = pawnAsset.HitomiOokisa,
                    MeOokisa = pawnAsset.MeOokisa,
                    MeKaiten = pawnAsset.MeKaiten,
                    MayuKaiten = pawnAsset.MayuKaiten,
                    MimiOokisa = pawnAsset.MimiOokisa,
                    MimiMuki = pawnAsset.MimiMuki,
                    ElfMimi = pawnAsset.ElfMimi,
                    MikenTakasa = pawnAsset.MikenTakasa,
                    MikenHaba = pawnAsset.MikenHaba,
                    HohoboneRyou = pawnAsset.HohoboneRyou,
                    HohoboneJyouge = pawnAsset.HohoboneJyouge,
                    Hohoniku = pawnAsset.Hohoniku,
                    ErahoneJyouge = pawnAsset.ErahoneJyouge,
                    ErahoneHaba = pawnAsset.ErahoneHaba,
                    HanaJyouge = pawnAsset.HanaJyouge,
                    HanaHaba = pawnAsset.HanaHaba,
                    HanaTakasa = pawnAsset.HanaTakasa,
                    HanaKakudo = pawnAsset.HanaKakudo,
                    KuchiHaba = pawnAsset.KuchiHaba,
                    KuchiAtsusa = pawnAsset.KuchiAtsusa,
                    EyebrowUVOffsetX = pawnAsset.EyebrowUVOffsetX,
                    EyebrowUVOffsetY = pawnAsset.EyebrowUVOffsetY,
                    Wrinkle = pawnAsset.Wrinkle,
                    WrinkleAlbedoBlendRate = pawnAsset.WrinkleAlbedoBlendRate,
                    WrinkleDetailNormalPower = pawnAsset.WrinkleDetailNormalPower,
                    MuscleAlbedoBlendRate = pawnAsset.MuscleAlbedoBlendRate,
                    MuscleDetailNormalPower = pawnAsset.MuscleDetailNormalPower,
                    Height = pawnAsset.Height,
                    HeadSize = pawnAsset.HeadSize,
                    NeckOffset = pawnAsset.NeckOffset,
                    NeckScale = pawnAsset.NeckScale,
                    UpperBodyScaleX = pawnAsset.UpperBodyScaleX,
                    BellySize = pawnAsset.BellySize,
                    TeatScale = pawnAsset.TeatScale,
                    TekubiSize = pawnAsset.TekubiSize,
                    KoshiOffset = pawnAsset.KoshiOffset,
                    KoshiSize = pawnAsset.KoshiSize,
                    AnkleOffset = pawnAsset.AnkleOffset,
                    Fat = pawnAsset.Fat,
                    Muscle = pawnAsset.Muscle,
                    MotionFilter = pawnAsset.MotionFilter
                };
                // TODO: State
                res.PawnInfo.MaxHp = 1000;
                res.PawnInfo.MaxStamina = 1000;
                res.PawnInfo.JobId = pawnAsset.Job;
                res.PawnInfo.CharacterJobDataList = new List<CDataCharacterJobData>(){ new CDataCharacterJobData {
                    Job = pawnAsset.Job,
                    Lv = pawnAsset.JobLv,
                    // TODO: Etc.
                }};
                res.PawnInfo.CharacterEquipDataList = new List<CDataCharacterEquipData>() {
                    new CDataCharacterEquipData() {
                        Equips = new List<CDataEquipItemInfo>() {
                            new CDataEquipItemInfo { EquipSlot = 1, ItemId = pawnAsset.Primary },
                            new CDataEquipItemInfo { EquipSlot = 2, ItemId = pawnAsset.Secondary },
                            new CDataEquipItemInfo { EquipSlot = 3, ItemId = pawnAsset.Head },
                            new CDataEquipItemInfo { EquipSlot = 4, ItemId = pawnAsset.Body },
                            new CDataEquipItemInfo { EquipSlot = 5, ItemId = pawnAsset.BodyClothing },
                            new CDataEquipItemInfo { EquipSlot = 6, ItemId = pawnAsset.Arm },
                            new CDataEquipItemInfo { EquipSlot = 7, ItemId = pawnAsset.Leg },
                            new CDataEquipItemInfo { EquipSlot = 8, ItemId = pawnAsset.LegWear },
                            new CDataEquipItemInfo { EquipSlot = 9, ItemId = pawnAsset.OverWear },
                            new CDataEquipItemInfo { EquipSlot = 10, ItemId = pawnAsset.JewelrySlot1 },
                            new CDataEquipItemInfo { EquipSlot = 11, ItemId = pawnAsset.JewelrySlot2 },
                            new CDataEquipItemInfo { EquipSlot = 12, ItemId = pawnAsset.JewelrySlot3 },
                            new CDataEquipItemInfo { EquipSlot = 13, ItemId = pawnAsset.JewelrySlot4 },
                            new CDataEquipItemInfo { EquipSlot = 14, ItemId = pawnAsset.JewelrySlot5 },
                            new CDataEquipItemInfo { EquipSlot = 15, ItemId = pawnAsset.Lantern }
                        }
                    }
                };
                res.PawnInfo.CharacterEquipViewDataList = new List<CDataCharacterEquipData>() {
                    new CDataCharacterEquipData() {
                        Equips = new List<CDataEquipItemInfo>() {
                            new CDataEquipItemInfo { EquipSlot = 1, ItemId = pawnAsset.VPrimary },
                            new CDataEquipItemInfo { EquipSlot = 2, ItemId = pawnAsset.VSecondary },
                            new CDataEquipItemInfo { EquipSlot = 3, ItemId = pawnAsset.VHead },
                            new CDataEquipItemInfo { EquipSlot = 4, ItemId = pawnAsset.VBody },
                            new CDataEquipItemInfo { EquipSlot = 5, ItemId = pawnAsset.VBodyClothing },
                            new CDataEquipItemInfo { EquipSlot = 6, ItemId = pawnAsset.VArm },
                            new CDataEquipItemInfo { EquipSlot = 7, ItemId = pawnAsset.VLeg },
                            new CDataEquipItemInfo { EquipSlot = 8, ItemId = pawnAsset.VLegWear },
                            new CDataEquipItemInfo { EquipSlot = 9, ItemId = pawnAsset.VOverWear },
                            new CDataEquipItemInfo { EquipSlot = 10, ItemId = 0 },
                            new CDataEquipItemInfo { EquipSlot = 111, ItemId = 0 },
                            new CDataEquipItemInfo { EquipSlot = 112, ItemId = 0 },
                            new CDataEquipItemInfo { EquipSlot = 113, ItemId = 0 },
                            new CDataEquipItemInfo { EquipSlot = 114, ItemId = 0 },
                            new CDataEquipItemInfo { EquipSlot = 115, ItemId = 0 }
                        }
                    }
                };
                // TODO: CharacterEquipJobItemList, JewelrySlotNum, CharacterItemSlotInfoList, CraftData, PawnReactionList
                res.PawnInfo.HideEquipHead = pawnAsset.HideEquipHead;
                res.PawnInfo.HideEquipLantern = pawnAsset.HideEquipLantern;
                // TODO: AdventureCount, CraftCount, MaxAdventureCount, MaxCraftCount, ContextNormalSkillList, ContextSkillList, ContextAbilityList
                // TODO: AbilityCostMax, ExtendParam
                res.PawnInfo.PawnType = pawnAsset.PawnType;
                // TODO: ShareRange, Likability, TrainingStatus, Unk1, SpSkillList
            }
            return res;
        }
    }
}