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
            PopulateNewPawnData(pawn);
            Server.CharacterManager.UpdateCharacterExtendedParams(pawn, true);

            if (request.SlotNo > 1)
            {
                // We need to consume items for the cost
                var uidItem = new Item() { ItemId = 10133 };
                Server.ItemManager.ConsumeItemByUIdFromItemBag(Server, client.Character, uidItem.UId, 10);
            }

            if (request.SlotNo == 1)
            {
                // Update the number of allowed Pawns to 2
                // We get 1 slot for the free pawn and a second slot is allowed
                // assuming we collected 10 riftstone shards and pay it using the
                // "Myrmidon's Pledge" menu option.
                Server.Database.UpdateMyPawnSlot(client.Character.CharacterId, 2);
                client.Character.MyPawnSlotNum = 2;
            }

            Database.CreatePawn(pawn);
            Database.InsertGainExtendParam(pawn.CommonId, pawn.ExtendedParams);

            pawn = Server.Database.SelectPawnsByCharacterId(client.Character.CharacterId).Where(x => x.PawnId == pawn.PawnId).FirstOrDefault();
            Server.CharacterManager.UpdateCharacterExtendedParams(pawn, true);

            client.Character.Pawns.Add(pawn);

            return new S2CPawnCreatePawnRes();
        }

        private void PopulateNewPawnData(Pawn pawn)
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

            pawn.Equipment = new Equipment(Server.AssetRepository.ArisenAsset.Select(arisenPreset => new Tuple<JobId, Dictionary<EquipType, List<Item>>>(arisenPreset.Job, new Dictionary<EquipType, List<Item>>() {
                {
                    EquipType.Performance,
                    new List<Item>() {
                        new Item {
                            ItemId = arisenPreset.PrimaryWeapon,
                            Unk3 = 0,
                            Color = arisenPreset.PrimaryWeaponColour,
                            PlusValue = 0
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
                            PlusValue = 3
                        },
                        new Item {
                            ItemId = arisenPreset.Body,
                            Unk3 = 0,
                            Color = arisenPreset.BodyColour,
                            PlusValue = 4,
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
                            PlusValue = 3
                        },
                        new Item {
                            ItemId = arisenPreset.Leg,
                            Unk3 = 0,
                            Color = arisenPreset.LegColour,
                            PlusValue = 3
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
                        },
                        new Item {
                            ItemId = arisenPreset.Jewelry2,
                            Unk3 = 0,
                            Color = 0,
                            PlusValue = 0,
                        },
                        new Item {
                            ItemId = arisenPreset.Jewelry3,
                            Unk3 = 0,
                            Color = 0,
                            PlusValue = 0,
                        },
                        new Item {
                            ItemId = arisenPreset.Jewelry4,
                            Unk3 = 0,
                            Color = 0,
                            PlusValue = 0,
                        },
                        new Item {
                            ItemId = arisenPreset.Jewelry5,
                            Unk3 = 0,
                            Color = 0,
                            PlusValue = 0,
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
        }
    }
}
