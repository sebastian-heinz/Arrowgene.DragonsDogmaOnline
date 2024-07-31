using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Chat.Command.Commands
{
    public class GivePawnCommand : ChatCommand
    {
        public override AccountStateType AccountState => AccountStateType.User;

        public override string Key => "givepawn";
        public override string HelpText => "usage: `/givepawn` - Give yourself a pawn.";

        private DdonGameServer _server;

        public GivePawnCommand(DdonGameServer server)
        {
            _server = server;
        }

        public override void Execute(string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
        {
            int pawnCount = client.Character.Pawns.Count + 1;

            Pawn pawn = new Pawn(client.Character.CharacterId)
            {
                Name = "Pawn",
                EditInfo = client.Character.EditInfo, //Copy this from the main character because we're lazy.
                HideEquipHead = client.Character.HideEquipHead,
                HideEquipLantern = client.Character.HideEquipLantern,
                Job = JobId.Fighter,
                HmType = 1,
                PawnType = 1,
                ExtendedParams = new CDataOrbGainExtendParam(),
                Server = client.Character.Server
            };

            PopulateNewPawnData(client.Character, pawn, pawnCount - 1);
            _server.CharacterManager.UpdateCharacterExtendedParams(pawn, true);

            client.Send(new S2CPawnExtendMainPawnNtc() { TotalNum = 3, AddNum = (byte)(3 - client.Character.Pawns.Count) });

            _server.Database.CreatePawn(pawn);
            _server.Database.InsertGainExtendParam(pawn.CommonId, pawn.ExtendedParams);

            pawn = _server.Database.SelectPawnsByCharacterId(client.Character.CharacterId).Where(x => x.PawnId == pawn.PawnId).FirstOrDefault();
            _server.CharacterManager.UpdateCharacterExtendedParams(pawn, true);

            pawn.Equipment = client.Character.Storage.GetPawnEquipment(pawnCount - 1);

            client.Character.Pawns.Add(pawn);

            responses.Add(ChatResponse.ServerMessage(client, $"Giving a basic pawn."));
        }

        //Copied from PawnCreatePawnHandler
        private void PopulateNewPawnData(Character character, Pawn pawn, int pawnIndex)
        {
            ArisenCsv activeJobPreset = _server.AssetRepository.ArisenAsset.Where(x => x.Job == pawn.Job).Single();
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

            pawn.CharacterJobDataList = _server.AssetRepository.ArisenAsset.Select(arisenPreset => new CDataCharacterJobData
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

            pawn.EquipmentTemplate = new EquipmentTemplate();

            pawn.EquippedCustomSkillsDictionary = _server.AssetRepository.ArisenAsset.Select(arisenPreset => new Tuple<JobId, List<CustomSkill>>(arisenPreset.Job, new List<CustomSkill>() {
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

            pawn.EquippedAbilitiesDictionary = _server.AssetRepository.ArisenAsset.Select(arisenPreset => new Tuple<JobId, List<Ability>>(arisenPreset.Job, new List<Ability>() {
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
