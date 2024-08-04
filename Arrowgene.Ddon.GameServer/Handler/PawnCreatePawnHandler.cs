using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;
using System;
using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server.Network;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnCreatePawnHandler : GameRequestPacketHandler<C2SPawnCreatePawnReq, S2CPawnCreatePawnRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnCreatePawnHandler));

        public PawnCreatePawnHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnCreatePawnRes Handle(GameClient client, C2SPawnCreatePawnReq request)
        {
            Pawn pawn = new Pawn(client.Character.CharacterId)
            {
                Name = request.PawnInfo.Name,
                EditInfo = request.PawnInfo.EditInfo,
                HideEquipHead = request.PawnInfo.HideEquipHead,
                HideEquipLantern = request.PawnInfo.HideEquipLantern,
                Job = request.PawnInfo.JobId,
                HmType = 1,
                PawnType = 1,
                ExtendedParams = new CDataOrbGainExtendParam(),
                Server = client.Character.Server
            };
            PopulateNewPawnData(client.Character, pawn, request.SlotNo - 1);
            Server.CharacterManager.UpdateCharacterExtendedParams(pawn, true);

            if (request.SlotNo == 1)
            {
                client.Send(new S2CPawnExtendMainPawnNtc() { TotalNum = 2, AddNum = 2 });
            }

            if (request.SlotNo > 1)
            {
                // We need to consume 10 rift crystals for the cost
                var result = Server.ItemManager.ConsumeItemByIdFromMultipleStorages(Server, client.Character, ItemManager.BothStorageTypes, 10133, 10);
                if (result == null)
                {
                    return new S2CPawnCreatePawnRes()
                    {
                        Error = (uint) ErrorCode.ERROR_CODE_CHARACTER_ITEM_NOT_FOUND
                    };
                }
            }

            Database.CreatePawn(pawn);
            Database.InsertGainExtendParam(pawn.CommonId, pawn.ExtendedParams);

            pawn = Server.Database.SelectPawnsByCharacterId(client.Character.CharacterId).Where(x => x.PawnId == pawn.PawnId).FirstOrDefault();
            Server.CharacterManager.UpdateCharacterExtendedParams(pawn, true);

            pawn.Equipment = client.Character.Storage.GetPawnEquipment(request.SlotNo - 1);

            client.Character.Pawns.Add(pawn);

            return new S2CPawnCreatePawnRes();
        }

        private void PopulateNewPawnData(Character character, Pawn pawn, int pawnIndex)
        {
            ArisenCsv activeJobPreset = Server.AssetRepository.ArisenAsset.Where(x => x.Job == pawn.Job).Single();
            pawn.StatusInfo = new CDataStatusInfo()
            {
                HP = activeJobPreset.HP,
                Stamina = activeJobPreset.Stamina,
                RevivePoint = activeJobPreset.RevivePoint,
                MaxHP = activeJobPreset.MaxHP,
                MaxStamina = activeJobPreset.MaxStamina,
                WhiteHP = activeJobPreset.WhiteHP,
                GainHP = activeJobPreset.GainHP,
                GainStamina = activeJobPreset.GainStamina,
                GainAttack = activeJobPreset.GainAttack,
                GainDefense = activeJobPreset.GainDefense,
                GainMagicAttack = activeJobPreset.GainMagicAttack,
                GainMagicDefense = activeJobPreset.GainMagicDefense,
            };

            pawn.CharacterJobDataList = Server.AssetRepository.ArisenAsset.Select(arisenPreset => new CDataCharacterJobData
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

            pawn.EquipmentTemplate = new EquipmentTemplate(
                Server.AssetRepository.PawnStartGearAsset.Select(pawnGearPreset => new Tuple<JobId, Dictionary<EquipType, List<Item>>>(pawnGearPreset.Job, new Dictionary<EquipType, List<Item>>() {
                {
                    EquipType.Performance,
                    new List<Item>() {
                        new Item {
                            ItemId = pawnGearPreset.Primary,
                            Unk3 = 0,
                            Color = 0,
                            PlusValue = 0
                        },
                        new Item {
                            ItemId = pawnGearPreset.Secondary,
                            Unk3 = 0,
                            Color = 0
                        },
                        new Item {
                            ItemId = pawnGearPreset.Head,
                            Unk3 = 0,
                            Color = 0,
                            PlusValue = 0
                        },
                        new Item {
                            ItemId = pawnGearPreset.Body,
                            Unk3 = 0,
                            Color = 0,
                            PlusValue = 0,
                        },
                        new Item {
                            ItemId = pawnGearPreset.BodyClothing,
                            Unk3 = 0,
                            Color = 0
                        },
                        new Item {
                            ItemId = pawnGearPreset.Arm,
                            Unk3 = 0,
                            Color = 0,
                            PlusValue = 0
                        },
                        new Item {
                            ItemId = pawnGearPreset.Leg,
                            Unk3 = 0,
                            Color = 0,
                            PlusValue = 0
                        },
                        new Item {
                            ItemId = pawnGearPreset.LegWear,
                            Unk3 = 0,
                            Color = 0
                        },
                        new Item {
                            ItemId = pawnGearPreset.OverWear,
                            Unk3 = 0,
                            Color = 0
                        },
                        new Item {
                            ItemId = pawnGearPreset.JewelrySlot1,
                            Unk3 = 0,
                            Color = 0,
                            PlusValue = 0,
                        },
                        new Item {
                            ItemId = pawnGearPreset.JewelrySlot2,
                            Unk3 = 0,
                            Color = 0,
                            PlusValue = 0,
                        },
                        new Item {
                            ItemId = pawnGearPreset.JewelrySlot3,
                            Unk3 = 0,
                            Color = 0,
                            PlusValue = 0,
                        },
                        new Item {
                            ItemId = pawnGearPreset.JewelrySlot4,
                            Unk3 = 0,
                            Color = 0,
                            PlusValue = 0,
                        },
                        new Item {
                            ItemId = pawnGearPreset.JewelrySlot5,
                            Unk3 = 0,
                            Color = 0,
                            PlusValue = 0,
                        },
                        new Item {
                            ItemId = pawnGearPreset.Lantern,
                            Unk3 = 0,
                        }
                    }.Select(item => (item == null || item.ItemId == 0) ? null : item).ToList()
                },
                {
                    EquipType.Visual,
                    new List<Item>() {
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

            pawn.EquippedCustomSkillsDictionary = Server.AssetRepository.ArisenAsset.Select(arisenPreset => new Tuple<JobId, List<CustomSkill>>(arisenPreset.Job, new List<CustomSkill>() {
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

            pawn.LearnedCustomSkills = pawn.EquippedCustomSkillsDictionary.SelectMany(jobAndSkills => jobAndSkills.Value).Where(skill => skill != null).ToList();

            pawn.EquippedAbilitiesDictionary = Server.AssetRepository.ArisenAsset.Select(arisenPreset => new Tuple<JobId, List<Ability>>(arisenPreset.Job, new List<Ability>() {
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

            pawn.LearnedAbilities = pawn.EquippedAbilitiesDictionary.SelectMany(jobAndAugs => jobAndAugs.Value).Where(aug => aug != null).ToList();
            pawn.TrainingPoints = int.MaxValue;
            pawn.AvailableTraining = uint.MaxValue;

            // Add current job's equipment to the equipment storage
            // EquipmentTemplate.TOTAL_EQUIP_SLOTS * 2
            List<Item?> performanceEquipItems = pawn.EquipmentTemplate.GetEquipment(pawn.Job, EquipType.Performance);
            for (int i = 0; i < performanceEquipItems.Count; i++)
            {
                Item? item = performanceEquipItems[i];
                if (item != null)
                {
                    ushort slot = (ushort)((i + 1) + (pawnIndex * (EquipmentTemplate.TOTAL_EQUIP_SLOTS * 2)));
                    character.Storage.GetStorage(StorageType.PawnEquipment).SetItem(item, 1, slot);
                    Database.InsertStorageItem(character.CharacterId, StorageType.PawnEquipment, slot, 1, item);
                }
            }
        }
    }
}
