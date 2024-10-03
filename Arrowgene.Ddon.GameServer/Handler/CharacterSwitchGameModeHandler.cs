using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp.Consumer.BlockingQueueConsumption;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterSwitchGameModeHandler : GameRequestPacketHandler<C2SCharacterSwitchGameModeReq, S2CCharacterSwitchGameModeRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterSwitchGameModeHandler));

        public CharacterSwitchGameModeHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCharacterSwitchGameModeRes Handle(GameClient client, C2SCharacterSwitchGameModeReq packet)
        {
            Logger.Info($"GameMode={packet.GameMode}");

            client.GameMode = packet.GameMode;

            bool createCharacter = false;
            uint originalCharacterId = client.Character.CharacterId;
            var storagesA = client.Character.Storage;
            var bbmProgress = client.Character.BbmProgress;
            var walletPointList = client.Character.WalletPointList;
            var warpPointList = client.Character.ReleasedWarpPoints;

            var serverInfo = client.Character.Server;
            if (client.GameMode == GameMode.Normal)
            {
                uint characterId = Server.Database.SelectBBMNormalCharacterId(client.Character.BbmCharacterId);

                client.Character = Server.CharacterManager.SelectCharacter(client, characterId);
                client.Character.BbmCharacterId = 0;
            }
            else if (client.GameMode == GameMode.BitterblackMaze)
            {
                uint bbmCharacterId = Server.Database.SelectBBMCharacterId(client.Character.CharacterId);
                if (bbmCharacterId == 0)
                {
                    createCharacter = true;
                    client.Character = CreateNewCharacter(client.Character);
                    bbmCharacterId = client.Character.CharacterId;
                }

                client.Character = Server.CharacterManager.SelectCharacter(client, bbmCharacterId);
                client.Character.BbmCharacterId = client.Character.CharacterId;
                client.Character.CharacterId = originalCharacterId;
            }
            else
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_JOBCHANGE_INTERNAL_ERROR, "Attempted to change game mode to unexpected type");
            }

            // Populate Equipment
            client.Character.Equipment = client.Character.Storage.GetCharacterEquipment();
            client.Character.Server = serverInfo;
            client.Character.BbmProgress = bbmProgress;
            client.Character.WalletPointList = walletPointList;
            client.Character.ReleasedWarpPoints = warpPointList;
            client.Character.OnlineStatus = OnlineStatus.Online;

            S2CCharacterSwitchGameModeNtc ntc = new S2CCharacterSwitchGameModeNtc()
            {
                Unk0 = (uint)packet.GameMode, // Probably not right? int vs uint
                CreateCharacter = createCharacter,
                CharacterInfo = new CDataCharacterInfo()
                {
                    CharacterId = client.Character.CharacterId,
                    UserId = client.Character.UserId,
                    Version = client.Character.Version,
                    FirstName = client.Character.FirstName,
                    LastName = client.Character.LastName,
                    EditInfo = client.Character.EditInfo,
                    StatusInfo = client.Character.StatusInfo,
                    Job = client.Character.Job,
                    CharacterJobDataList = client.Character.CharacterJobDataList,
                    PlayPointList = client.Character.PlayPointList,
                    CharacterEquipDataList = new List<CDataCharacterEquipData>() { new CDataCharacterEquipData() {
                            Equips = client.Character.Equipment.AsCDataEquipItemInfo(EquipType.Performance)
                        }},
                    CharacterEquipViewDataList = new List<CDataCharacterEquipData>() { new CDataCharacterEquipData() {
                            Equips = client.Character.Equipment.AsCDataEquipItemInfo(EquipType.Visual)
                        }},
                    CharacterEquipJobItemList = client.Character.EquipmentTemplate.JobItemsAsCDataEquipJobItem(client.Character.Job),
                    JewelrySlotNum = client.Character.JewelrySlotNum,
                    // Unk0 = 
                    CharacterItemSlotInfoList = client.Character.Storage.GetAllStoragesAsCDataCharacterItemSlotInfoList(),
                    WalletPointList = client.Character.WalletPointList,
                    MyPawnSlotNum = client.Character.MyPawnSlotNum,
                    RentalPawnSlotNum = client.Character.RentalPawnSlotNum,
                    OrbStatusList = client.Character.OrbStatusList,
                    MsgSetList = client.Character.MsgSetList,
                    ShortCutList = client.Character.ShortCutList,
                    CommunicationShortCutList = client.Character.CommunicationShortCutList,
                    MatchingProfile = client.Character.MatchingProfile,
                    ArisenProfile = client.Character.ArisenProfile,
                    HideEquipHead = client.Character.HideEquipHead,
                    HideEquipLantern = client.Character.HideEquipLantern,
                    HideEquipHeadPawn = client.Character.HideEquipHeadPawn,
                    HideEquipLanternPawn = client.Character.HideEquipLanternPawn,
                    ArisenProfileShareRange = client.Character.ArisenProfileShareRange,
                    OnlineStatus = client.Character.OnlineStatus
                }
            };

            client.Send(ntc);

            S2CEquipChangeCharacterEquipLobbyNtc characterEquipLobbyNtc = new S2CEquipChangeCharacterEquipLobbyNtc()
            {
                CharacterId = client.Character.CharacterId,
                Job = client.Character.Job,
                EquipItemList = client.Character.Equipment.AsCDataEquipItemInfo(EquipType.Performance),
                VisualEquipItemList = client.Character.Equipment.AsCDataEquipItemInfo(EquipType.Visual),
            };
            client.Send(characterEquipLobbyNtc);

            var itemStorages = ItemManager.ItemBagStorageTypes.Concat(new List<StorageType>() { StorageType.CharacterEquipment}).ToList();
            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = ItemNoticeType.SwitchingStorage,
                UpdateItemList = SwapCharacterInventories(client.Character, storagesA, client.Character.Storage, itemStorages)
            };
            client.Send(updateCharacterItemNtc);

            return new S2CCharacterSwitchGameModeRes()
            {
                GameMode = packet.GameMode,
            };
        }

        private Character CreateNewCharacter(Character normalCharacter)
        {
            Character bbmCharacter = new Character();

            bbmCharacter.AccountId = normalCharacter.AccountId;
            bbmCharacter.CharacterId = 0;
            bbmCharacter.UserId = normalCharacter.UserId;
            bbmCharacter.Version = normalCharacter.Version;
            bbmCharacter.GameMode = GameMode.BitterblackMaze;
            bbmCharacter.FirstName = normalCharacter.FirstName;
            bbmCharacter.LastName = normalCharacter.LastName;
            bbmCharacter.EditInfo = normalCharacter.EditInfo;
            bbmCharacter.StatusInfo = normalCharacter.StatusInfo;
            bbmCharacter.Job = normalCharacter.Job;
            bbmCharacter.JewelrySlotNum = 1;
            bbmCharacter.MyPawnSlotNum = 0;
            bbmCharacter.RentalPawnSlotNum = 0;
            bbmCharacter.MatchingProfile = normalCharacter.MatchingProfile;
            bbmCharacter.ArisenProfile = normalCharacter.ArisenProfile;
            bbmCharacter.HideEquipHead = normalCharacter.HideEquipHead;
            bbmCharacter.HideEquipLantern = normalCharacter.HideEquipLantern;
            bbmCharacter.HideEquipHeadPawn = normalCharacter.HideEquipHeadPawn;
            bbmCharacter.HideEquipLanternPawn = normalCharacter.HideEquipLanternPawn;
            bbmCharacter.ArisenProfileShareRange = normalCharacter.ArisenProfileShareRange;
            bbmCharacter.OnlineStatus = normalCharacter.OnlineStatus;
            bbmCharacter.Server = normalCharacter.Server;
            bbmCharacter.FavWarpSlotNum = 1;
            bbmCharacter.BbmProgress = normalCharacter.BbmProgress;

            ArisenCsv ActiveJobPreset = Server.AssetRepository.ArisenAsset.Where(x => x.Job == bbmCharacter.Job).Single();
            bbmCharacter.StatusInfo = new CDataStatusInfo()
            {
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

            bbmCharacter.CharacterJobDataList = Server.AssetRepository.ArisenAsset.Select(arisenPreset => new CDataCharacterJobData
            {
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

            bbmCharacter.EquipmentTemplate = new EquipmentTemplate(Server.AssetRepository.BitterblackMazeAsset.GenerateStarterEquipment(), Server.AssetRepository.BitterblackMazeAsset.GenerateStarterJobEquipment());

            bbmCharacter.EquippedCustomSkillsDictionary = Server.AssetRepository.ArisenAsset.Select(arisenPreset => new Tuple<JobId, List<CustomSkill>>(arisenPreset.Job, new List<CustomSkill>() {
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

            // In BBM, all custom skills are already learned so add them
            bbmCharacter.LearnedCustomSkills = SkillGetAcquirableSkillListHandler.AllSkills.Select(x => new CustomSkill()
            {
                Job = x.Job,
                SkillId = x.SkillNo,
                SkillLv = x.Params.Max(x => x.Lv)
            }).ToList();
            
            bbmCharacter.EquippedAbilitiesDictionary = Server.AssetRepository.ArisenAsset.Select(arisenPreset => new Tuple<JobId, List<Ability>>(arisenPreset.Job, new List<Ability>() {
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

            // In BBM, all abilities are already learned so add them
            bbmCharacter.LearnedAbilities = SkillGetAcquirableAbilityListHandler.AllAbilities.Select(x => new Ability()
            {
                Job = x.Job,
                AbilityId = x.AbilityNo,
                AbilityLv = x.Params.Max(x => x.Lv)
            }).ToList();

            bbmCharacter.Storage = new Storages(Server.AssetRepository.StorageAsset.ToDictionary(x => x.StorageType, x => x.SlotMax));

            bbmCharacter.WalletPointList = new List<CDataWalletPoint>()
            {
                new CDataWalletPoint() {
                    Type = WalletType.Gold,
                    Value = 0
                },
                new CDataWalletPoint() {
                    Type = WalletType.RiftPoints,
                    Value = 0
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
                    Type = WalletType.BitterblackMazeResetTicket,
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

            // Add starting storage items
            foreach (var tuple in Server.AssetRepository.StorageItemAsset)
            {
                if (tuple.Item1 == StorageType.ItemBagConsumable)
                {
                    continue;
                }

                if (tuple.Item3.ItemId != 0)
                {
                    bbmCharacter.Storage.GetStorage(tuple.Item1).AddItem(tuple.Item3, tuple.Item2);
                }
            }

            // Add current job's equipment to the equipment storage
            List<Item?> performanceEquipItems = bbmCharacter.EquipmentTemplate.GetEquipment(bbmCharacter.Job, EquipType.Performance);
            for (int i = 0; i < performanceEquipItems.Count; i++)
            {
                Item? item = performanceEquipItems[i];

                ushort slot = (ushort)(i + 1);
                if (item == null || item.ItemId == 0)
                {
                    bbmCharacter.Storage.GetStorage(StorageType.CharacterEquipment).SetItem(null, 1, slot);
                }
                else
                {
                    bbmCharacter.Storage.GetStorage(StorageType.CharacterEquipment).SetItem(item, 1, slot);
                }
            }

            Server.CharacterManager.UpdateCharacterExtendedParams(bbmCharacter, true);

            bbmCharacter.GreenHp = CharacterManager.BBM_BASE_HEALTH;
            bbmCharacter.WhiteHp = CharacterManager.BBM_BASE_HEALTH;
            if (!Database.CreateCharacter(bbmCharacter))
            {
                return null;
            }

            // In BBM, all skills are already learned so add them
            Dictionary<JobId, List<LearnedNormalSkill>> learnedNormalSkillsMap = Server.AssetRepository.LearnedNormalSkillsAsset.LearnedNormalSkills;
            foreach (var (jobId, skills) in learnedNormalSkillsMap)
            {
                foreach (var learnedSkill in skills)
                {
                    uint index = 1;
                    foreach (var skillNo in learnedSkill.SkillNo)
                    {
                        CDataNormalSkillParam newSkill = new CDataNormalSkillParam()
                        {
                            Job = jobId,
                            Index = index,
                            SkillNo = skillNo,
                            PreSkillNo = 0
                        };

                        bbmCharacter.LearnedNormalSkills.Add(newSkill);
                        Server.Database.InsertIfNotExistsNormalSkillParam(bbmCharacter.CommonId, newSkill);

                        index++;
                    }
                }
            }

            // Populate extra tables for the characters
            CDataOrbGainExtendParam ExtendParams = new CDataOrbGainExtendParam()
            {
                JewelrySlot = 3,
            };

            if (!Database.InsertGainExtendParam(bbmCharacter.CommonId, ExtendParams))
            {
                return null;
            }

            Enum.GetValues(typeof(JobId)).Cast<JobId>().Select(job => new CDataJobPlayPoint()
            {
                Job = job,
                PlayPoint = new CDataPlayPointData()
                {
                    ExpMode = ExpMode.Experience, // EXP
                    PlayPoint = 0
                }
            }).ToList().ForEach((Action<CDataJobPlayPoint>)(x => {
                Database.ReplaceCharacterPlayPointData((uint)bbmCharacter.ContentCharacterId, x);
                bbmCharacter.PlayPointList.Add(x);
            }));

            if (!Server.Database.InsertBBMCharacterId(normalCharacter.CharacterId, bbmCharacter.CharacterId))
            {
                // Failed to save character
                return null;
            }

            return bbmCharacter;
        }

        /**
         * @brief Swaps character inventories for content where the player looses access to their normal inventory such as Bitterblack Maze.
         * This swap is a visual swap in the client UI -- by this we mean the server will use different inventory storage which shows up in
         * the players storage in the client UI. It doesn't impact the actual storage during normal gameplay.
         */
        private List<CDataItemUpdateResult> SwapCharacterInventories(Character character, Storages storageA, Storages storageB, List<StorageType> storageTypes)
        {
            var results = new List<CDataItemUpdateResult>();
            foreach (var storageType in storageTypes)
            {
                for (int i = 0; i < character.Storage.GetStorage(storageType).Items.Count; i++)
                {
                    ushort slotNo = (ushort)(i + 1);

                    var storageItemA = storageA.GetStorage(storageType).GetItem(slotNo);
                    if (storageItemA != null)
                    {
                        results.Add(Server.ItemManager.CreateItemUpdateResult(null, storageItemA.Item1, storageType, slotNo, 0, 0));
                    }

                    var storageItemB = storageB.GetStorage(storageType).GetItem(slotNo);
                    if (storageItemB != null)
                    {
                        results.Add(Server.ItemManager.CreateItemUpdateResult(null, storageItemB.Item1, storageType, slotNo, storageItemB.Item2, storageItemB.Item2));
                    }
                    else if (storageItemA != null)
                    {
                        Item item = new Item()
                        {
                            ItemId = 0,
                            UId = ""
                        };
                        results.Add(Server.ItemManager.CreateItemUpdateResult(null, item, storageType, slotNo, storageItemA.Item2, storageItemA.Item2));
                    }
                }
            }
            return results;
        }
    }
}
