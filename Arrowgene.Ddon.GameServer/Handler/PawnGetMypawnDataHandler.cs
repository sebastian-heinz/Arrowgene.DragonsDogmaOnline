using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetMypawnDataHandler : StructurePacketHandler<GameClient, C2SPawnGetMypawnDataReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetMypawnDataHandler));


        public PawnGetMypawnDataHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPawnGetMypawnDataReq> req)
        {
                MyPawnCsv myPawnCsvData = Server.AssetRepository.MyPawnAsset[req.Structure.SlotNo-1];
                client.Send(new S2CPawnGetMypawnDataRes() {
                    PawnId = (uint) req.Structure.PawnId,
                    PawnInfo = new CDataPawnInfo()
                    {
                        Version = 0,
                        Name = myPawnCsvData.Name,
                        EditInfo = new CDataEditInfo()
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
                        },
                        State = 0,
                        MaxHp = 767,
                        MaxStamina = 451,
                        JobId = myPawnCsvData.Job,
                        CharacterJobDataList = new List<CDataCharacterJobData>() {
                            new CDataCharacterJobData() {
                                Job = myPawnCsvData.Job,
                                Lv = myPawnCsvData.JobLv
                            }
                        },
                        CharacterEquipDataList = new List<CDataCharacterEquipData>() {
                            new CDataCharacterEquipData() {
                                Equips = new List<CDataEquipItemInfo>() {
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.Primary,
                                        Unk0 = 0,
                                        EquipType = 1,
                                        EquipSlot = 1
                                    },
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.Secondary,
                                        Unk0 = 0,
                                        EquipType = 1,
                                        EquipSlot = 2
                                    },
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.Head,
                                        Unk0 = 0,
                                        EquipType = 1,
                                        EquipSlot = 3
                                    },
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.Body,
                                        Unk0 = 0,
                                        EquipType = 1,
                                        EquipSlot = 4
                                    },
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.BodyClothing,
                                        Unk0 = 0,
                                        EquipType = 1,
                                        EquipSlot = 5
                                    },
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.Arm,
                                        Unk0 = 0,
                                        EquipType = 1,
                                        EquipSlot = 6
                                    },
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.Leg,
                                        Unk0 = 0,
                                        EquipType = 1,
                                        EquipSlot = 7
                                    },
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.LegWear,
                                        Unk0 = 0,
                                        EquipType = 1,
                                        EquipSlot = 8
                                    },
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.OverWear,
                                        Unk0 = 0,
                                        EquipType = 1,
                                        EquipSlot = 9
                                    },
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.JewelrySlot1,
                                        Unk0 = 0,
                                        EquipType = 1,
                                        EquipSlot = 10
                                    },
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.JewelrySlot2,
                                        Unk0 = 0,
                                        EquipType = 1,
                                        EquipSlot = 11
                                    },
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.JewelrySlot3,
                                        Unk0 = 0,
                                        EquipType = 1,
                                        EquipSlot = 12
                                    },
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.JewelrySlot4,
                                        Unk0 = 0,
                                        EquipType = 1,
                                        EquipSlot = 13
                                    },
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.JewelrySlot5,
                                        Unk0 = 0,
                                        EquipType = 1,
                                        EquipSlot = 14
                                    },
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.Lantern,
                                        Unk0 = 0,
                                        EquipType = 1,
                                        EquipSlot = 15
                                    }
                                }
                            }
                        },
                        CharacterEquipViewDataList = new List<CDataCharacterEquipData>() {
                            new CDataCharacterEquipData() {
                                Equips = new List<CDataEquipItemInfo>() {
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.VPrimary,
                                        Unk0 = 0,
                                        EquipType = 2,
                                        EquipSlot = 1
                                    },
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.VSecondary,
                                        Unk0 = 0,
                                        EquipType = 2,
                                        EquipSlot = 2
                                    },
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.VHead,
                                        Unk0 = 0,
                                        EquipType = 2,
                                        EquipSlot = 3
                                    },
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.VBody,
                                        Unk0 = 0,
                                        EquipType = 2,
                                        EquipSlot = 4
                                    },
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.VBodyClothing,
                                        Unk0 = 0,
                                        EquipType = 2,
                                        EquipSlot = 5
                                    },
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.VArm,
                                        Unk0 = 0,
                                        EquipType = 2,
                                        EquipSlot = 6
                                    },
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.VLeg,
                                        Unk0 = 0,
                                        EquipType = 2,
                                        EquipSlot = 7
                                    },
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.VLegWear,
                                        Unk0 = 0,
                                        EquipType = 2,
                                        EquipSlot = 8
                                    },
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.VOverWear,
                                        Unk0 = 0,
                                        EquipType = 2,
                                        EquipSlot = 9
                                    },
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.VJewelrySlot1,
                                        Unk0 = 0,
                                        EquipType = 2,
                                        EquipSlot = 10
                                    },
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.VJewelrySlot2,
                                        Unk0 = 0,
                                        EquipType = 2,
                                        EquipSlot = 11
                                    },
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.VJewelrySlot3,
                                        Unk0 = 0,
                                        EquipType = 2,
                                        EquipSlot = 12
                                    },
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.VJewelrySlot4,
                                        Unk0 = 0,
                                        EquipType = 2,
                                        EquipSlot = 13
                                    },
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.VJewelrySlot5,
                                        Unk0 = 0,
                                        EquipType = 2,
                                        EquipSlot = 14
                                    },
                                    new CDataEquipItemInfo() {
                                        ItemId = myPawnCsvData.VLantern,
                                        Unk0 = 0,
                                        EquipType = 2,
                                        EquipSlot = 15
                                    }
                                }
                            }
                        },
                        CharacterEquipJobItemList = new List<CDataEquipJobItem>() {
                            new CDataEquipJobItem() {JobItemId = myPawnCsvData.JobItem1, EquipSlotNo = 1},
                            new CDataEquipJobItem() {JobItemId = myPawnCsvData.JobItem2, EquipSlotNo = 2}
                        },
                        JewelrySlotNum = 5,
                        CharacterItemSlotInfoList = new List<CDataCharacterItemSlotInfo>(),
                        CraftData = new CDataPawnCraftData() {
                            CraftExp = 391,
                            CraftRank = 4,
                            CraftRankLimit = 8,
                            CraftPoint = 0,
                            PawnCraftSkillList = new List<CDataPawnCraftSkill>() {
                                new CDataPawnCraftSkill() {Type = 1, Level = 0},
                                new CDataPawnCraftSkill() {Type = 2, Level = 3},
                                new CDataPawnCraftSkill() {Type = 3, Level = 0},
                                new CDataPawnCraftSkill() {Type = 4, Level = 0},
                                new CDataPawnCraftSkill() {Type = 5, Level = 0},
                                new CDataPawnCraftSkill() {Type = 6, Level = 0},
                                new CDataPawnCraftSkill() {Type = 7, Level = 0},
                                new CDataPawnCraftSkill() {Type = 8, Level = 0},
                                new CDataPawnCraftSkill() {Type = 9, Level = 0},
                                new CDataPawnCraftSkill() {Type = 10, Level = 0}
                            }
                        },
                        PawnReactionList = new List<CDataPawnReaction>() {
                            new CDataPawnReaction() {ReactionType = 1, MotionNo = myPawnCsvData.MetPartyMembersId},
                            new CDataPawnReaction() {ReactionType = 2, MotionNo = myPawnCsvData.QuestClearId},
                            new CDataPawnReaction() {ReactionType = 10, MotionNo = myPawnCsvData.SpecialSkillInspirationMomentId},
                            new CDataPawnReaction() {ReactionType = 4, MotionNo = myPawnCsvData.LevelUpId},
                            new CDataPawnReaction() {ReactionType = 11, MotionNo = myPawnCsvData.SpecialSkillUseId},
                            new CDataPawnReaction() {ReactionType = 6, MotionNo = myPawnCsvData.PlayerDeathId},
                            new CDataPawnReaction() {ReactionType = 7, MotionNo = myPawnCsvData.WaitingOnLobbyId},
                            new CDataPawnReaction() {ReactionType = 8, MotionNo = myPawnCsvData.WaitingOnAdventureId},
                            new CDataPawnReaction() {ReactionType = 9, MotionNo = myPawnCsvData.EndOfCombatId}
                        },
                        HideEquipHead = myPawnCsvData.HideEquipHead,
                        HideEquipLantern = myPawnCsvData.HideEquipLantern,
                        AdventureCount = 5,
                        CraftCount = 10,
                        MaxAdventureCount = 5,
                        MaxCraftCount = 10,
                        ContextNormalSkillList = new List<CDataContextNormalSkillData>() {
                            new CDataContextNormalSkillData() {SkillNo = myPawnCsvData.NormalSkill1},
                            new CDataContextNormalSkillData() {SkillNo = myPawnCsvData.NormalSkill2},
                            new CDataContextNormalSkillData() {SkillNo = myPawnCsvData.NormalSkill3},
                        },
                        ContextSkillList = new List<CDataContextAcquirementData>() {
                            new CDataContextAcquirementData() {
                                SlotNo = 1,
                                AcquirementNo = myPawnCsvData.CustomSkillId1,
                                AcquirementLv = myPawnCsvData.CustomSkillLv1
                            },
                            new CDataContextAcquirementData() {
                                SlotNo = 2,
                                AcquirementNo = myPawnCsvData.CustomSkillId2,
                                AcquirementLv = myPawnCsvData.CustomSkillLv2
                            },
                            new CDataContextAcquirementData() {
                                SlotNo = 3,
                                AcquirementNo = myPawnCsvData.CustomSkillId3,
                                AcquirementLv = myPawnCsvData.CustomSkillLv3
                            },
                            new CDataContextAcquirementData() {
                                SlotNo = 4,
                                AcquirementNo = myPawnCsvData.CustomSkillId4,
                                AcquirementLv = myPawnCsvData.CustomSkillLv4
                            },
                        },
                        ContextAbilityList = new List<CDataContextAcquirementData> () {
                            new CDataContextAcquirementData() {
                                SlotNo = 1,
                                AcquirementNo = myPawnCsvData.AbilityId1,
                                AcquirementLv = myPawnCsvData.AbilityLv1
                            },
                            new CDataContextAcquirementData() {
                                SlotNo = 2,
                                AcquirementNo = myPawnCsvData.AbilityId2,
                                AcquirementLv = myPawnCsvData.AbilityLv2
                            },
                            new CDataContextAcquirementData() {
                                SlotNo = 3,
                                AcquirementNo = myPawnCsvData.AbilityId3,
                                AcquirementLv = myPawnCsvData.AbilityLv3
                            },
                            new CDataContextAcquirementData() {
                                SlotNo = 4,
                                AcquirementNo = myPawnCsvData.AbilityId4,
                                AcquirementLv = myPawnCsvData.AbilityLv4
                            },
                            new CDataContextAcquirementData() {
                                SlotNo = 5,
                                AcquirementNo = myPawnCsvData.AbilityId5,
                                AcquirementLv = myPawnCsvData.AbilityLv5
                            },
                            new CDataContextAcquirementData() {
                                SlotNo = 6,
                                AcquirementNo = myPawnCsvData.AbilityId6,
                                AcquirementLv = myPawnCsvData.AbilityLv6
                            },
                            new CDataContextAcquirementData() {
                                SlotNo = 7,
                                AcquirementNo = myPawnCsvData.AbilityId7,
                                AcquirementLv = myPawnCsvData.AbilityLv7
                            },
                            new CDataContextAcquirementData() {
                                SlotNo = 8,
                                AcquirementNo = myPawnCsvData.AbilityId8,
                                AcquirementLv = myPawnCsvData.AbilityLv8
                            },
                            new CDataContextAcquirementData() {
                                SlotNo = 9,
                                AcquirementNo = myPawnCsvData.AbilityId9,
                                AcquirementLv = myPawnCsvData.AbilityLv9
                            },
                            new CDataContextAcquirementData() {
                                SlotNo = 10,
                                AcquirementNo = myPawnCsvData.AbilityId10,
                                AcquirementLv = myPawnCsvData.AbilityLv10
                            }                    
                        },
                        AbilityCostMax = 15,
                        ExtendParam = new CDataOrbGainExtendParam() {
                            HpMax = 0x29E,
                            StaminaMax = 0x0,
                            Attack = 0x10,
                            Defence = 0x10,
                            MagicAttack = 0xF,
                            MagicDefence = 0x10,
                            AbilityCost = 0x0,
                            JewelrySlot = 0x0,
                            UseItemSlot = 0x0,
                            MaterialItemSlot = 0x0,
                            EquipItemSlot = 0x0,
                            MainPawnSlot = 0x0,
                            SupportPawnSlot = 0x0
                        },
                        PawnType = 0,
                        ShareRange = 1,
                        Likability = 2,
                        Unk0 = new byte[64],
                        Unk1 = new CData_772E80() {Unk0 = 0x7530, Unk1 = 0x3, Unk2 = 0x3, Unk3 = 0x1, Unk4 = 0x3},
                        Unk2 = new List<CDataSpSkill>() {
                            new CDataSpSkill() {SpSkillId = myPawnCsvData.SpSkillSlot1Id, SpSkillLv = myPawnCsvData.SpSkillSlot1Lv},
                            new CDataSpSkill() {SpSkillId = myPawnCsvData.SpSkillSlot2Id, SpSkillLv = myPawnCsvData.SpSkillSlot2Lv},
                            new CDataSpSkill() {SpSkillId = myPawnCsvData.SpSkillSlot3Id, SpSkillLv = myPawnCsvData.SpSkillSlot3Lv}
                        }
                    }
                });
        }
    }
}
