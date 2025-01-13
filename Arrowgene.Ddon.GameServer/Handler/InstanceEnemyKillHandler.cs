using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.GatheringItems.Generators;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceEnemyKillHandler : GameRequestPacketHandler<C2SInstanceEnemyKillReq, S2CInstanceEnemyKillRes>
    {

        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceEnemyKillHandler));

        private readonly DdonGameServer _gameServer;

        private readonly HashSet<uint> _ignoreKillsInStageIds = new HashSet<uint>()
        {
            349, //White Dragon Temple, Training Room
        };

        public InstanceEnemyKillHandler(DdonGameServer server) : base(server)
        {
            _gameServer = server;
        }

        public override S2CInstanceEnemyKillRes Handle(GameClient client, C2SInstanceEnemyKillReq packet)
        {
            CDataStageLayoutId layoutId = packet.LayoutId;
            StageId stageId = StageId.FromStageLayoutId(layoutId);

            PacketQueue queuedPackets = new();

            // The training room uses special handling to produce enemies that don't exist in the QuestState or InstanceEnemyManager.
            // Return an empty response here to not break the rest of the handling.
            if (_ignoreKillsInStageIds.Contains(stageId.Id))
            {
                return new();
            }

            Quest quest = null;
            bool IsQuestControlled = false;
            foreach (var questScheduleId in QuestManager.CollectQuestScheduleIds(client, stageId))
            {
                quest = client.Party.QuestState.GetQuest(questScheduleId);
                if (quest != null)
                {
                    var questStateManager = QuestManager.GetQuestStateManager(client, quest);
                    if (questStateManager.HasEnemiesInCurrentStageGroup(quest, stageId))
                    {
                        IsQuestControlled = true;
                        break;
                    }
                }
            }

            InstancedEnemy enemyKilled = client.Party.InstanceEnemyManager.GetInstanceEnemy(stageId, (byte)packet.SetId);
            if (enemyKilled is null)
            {
                Logger.Error(client, $"Enemy killed data missing; {layoutId}.{packet.SetId}");
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_INSTANCE_AREA_ENEMY_UNIT_DATA_NONE);
            }

            if (enemyKilled.RepopCount > 0 && enemyKilled.RepopNum < enemyKilled.RepopCount)
            {
                enemyKilled.RepopNum += 1;

                S2CInstanceEnemyRepopNtc repopNtc = new S2CInstanceEnemyRepopNtc()
                {
                    LayoutId = layoutId,
                    WaitSecond = enemyKilled.RepopWaitSecond,
                    EnemyData = new CDataLayoutEnemyData()
                    {
                        PositionIndex = (byte)packet.SetId,
                        EnemyInfo = enemyKilled.asCDataStageLayoutEnemyPresetEnemyInfoClient()
                    }
                };
                client.Party.EnqueueToAll(repopNtc, queuedPackets);
            }
            else
            {
                enemyKilled.IsKilled = true;
            }

            bool isEpitaphEnemy = false;
            if (_gameServer.EpitaphRoadManager.TrialInProgress(client.Party))
            {
                isEpitaphEnemy = _gameServer.EpitaphRoadManager.TrialHasEnemies(client.Party, stageId, 0);
                _gameServer.EpitaphRoadManager.EvaluateEnemyKilled(client.Party, stageId, packet.SetId, enemyKilled);
            }

            Server.Database.ExecuteInTransaction(connectionIn =>
            {

                List<InstancedEnemy> group = client.Party.InstanceEnemyManager.GetInstancedEnemies(stageId);
                bool groupDestroyed = group.Where(x => x.IsRequired).All(x => x.IsKilled);
                if (groupDestroyed)
                {

                    bool IsAreaBoss = false;
                    foreach (var enemy in group)
                    {
                        IsAreaBoss = IsAreaBoss || enemy.IsAreaBoss;
                        if (IsAreaBoss)
                        {
                            break;
                        }
                    }

                    // This is used for quests and things like key door monsters
                    S2CInstanceEnemyGroupDestroyNtc groupDestroyedNtc = new S2CInstanceEnemyGroupDestroyNtc()
                    {
                        LayoutId = packet.LayoutId,
                        IsAreaBoss = IsAreaBoss && (client.GameMode == GameMode.Normal)
                    };
                    client.Party.EnqueueToAll(groupDestroyedNtc, queuedPackets);

                    if (IsAreaBoss && client.GameMode == GameMode.BitterblackMaze)
                    {
                        foreach (var memberClient in client.Party.Clients)
                        {
                            var ntcs = BitterblackMazeManager.HandleTierClear(_gameServer, memberClient, memberClient.Character, stageId, connectionIn);
                            queuedPackets.AddRange(ntcs);
                        }
                    }
                }

                if (packet.IsNoBattleReward)
                {
                    queuedPackets.Send();
                }

                foreach (var partyMemberClient in client.Party.Clients)
                {
                    var instancedGatheringItems = partyMemberClient.InstanceDropItemManager.Generate(enemyKilled);

                    uint offsetSetId = partyMemberClient.InstanceDropItemManager.Assign(layoutId, packet.SetId, instancedGatheringItems.Values.SelectMany(x => x).ToList());
                    var dropItemNtc = new S2CInstancePopDropItemNtc()
                    {
                        LayoutId = packet.LayoutId,
                        SetId = offsetSetId,
                        MdlType = enemyKilled.DropsTable?.MdlType ?? 0,
                        PosX = packet.DropPosX,
                        PosY = packet.DropPosY,
                        PosZ = packet.DropPosZ
                    };

                    if (instancedGatheringItems[typeof(EnemyEpitaphRoadDropGenerator)].Any())
                    {
                        dropItemNtc.MdlType = 1; // Make the bag appear as golden
                    }

                    // If the roll was unlucky, there is a chance that no bag will show.
                    if (instancedGatheringItems.Any(x => x.Value.Any()))
                    {
                        partyMemberClient.Enqueue(dropItemNtc, queuedPackets);
                    }
                }

                // TODO: This will be revisited so we can properly handle EXP assigned by tool and
                // TODO: EXP determined by the mixin. For now, the default behavior of the mixin
                // TODO: is the same as the original server behavior.
                var expCurveMixin = Server.ScriptManager.MixinModule.Get<IExpMixin>("exp");

                uint baseEnemyExp = expCurveMixin.GetExpValue(enemyKilled);
                baseEnemyExp = _gameServer.ExpManager.GetScaledPointAmount(client.GameMode, RewardSource.Enemy, PointType.ExperiencePoints, baseEnemyExp);
                
                uint calcExp = _gameServer.ExpManager.GetAdjustedExp(client.GameMode, RewardSource.Enemy, client.Party, baseEnemyExp, enemyKilled.Lv);
                uint calcPP = _gameServer.ExpManager.GetScaledPointAmount(client.GameMode, RewardSource.Enemy, PointType.PlayPoints, enemyKilled.GetDroppedPlayPoints());

                foreach (PartyMember member in client.Party.Members)
                {
                    if (member.JoinState != JoinState.On) continue; // Only fully joined members get rewards.

                    uint gainedExp = calcExp;
                    uint gainedPP = calcPP;

                    GameClient memberClient;
                    CharacterCommon memberCharacter;
                    if (member is PlayerPartyMember playerMember)
                    {
                        memberClient = playerMember.Client;
                        memberCharacter = memberClient.Character;

                        if (memberCharacter.Stage.Id != stageId.Id) continue; // Only nearby allies get XP.

                        if (memberClient.Character.ActiveCharacterPlayPointData.PlayPoint.ExpMode == ExpMode.Experience && !IsQuestControlled && !isEpitaphEnemy)
                        {
                            gainedPP = 0;
                        }
                        else if (!IsQuestControlled && !isEpitaphEnemy)
                        {
                            gainedExp = 0;
                        }

                        var huntPackets = playerMember.QuestState.HandleEnemyHuntRequests(enemyKilled, connectionIn);
                        queuedPackets.AddRange(huntPackets);

                        S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();

                        if (enemyKilled.BloodOrbs > 0)
                        {
                            // Drop BO
                            uint gainedBo = (uint) (enemyKilled.BloodOrbs * _gameServer.GameLogicSettings.BoModifier);
                            uint bonusBo = (uint) (gainedBo * _gameServer.GpCourseManager.EnemyBloodOrbBonus());
                            CDataUpdateWalletPoint boUpdateWalletPoint = _gameServer.WalletManager.AddToWallet(memberClient.Character, WalletType.BloodOrbs, gainedBo + bonusBo, bonusBo, connectionIn: connectionIn);
                            updateCharacterItemNtc.UpdateWalletList.Add(boUpdateWalletPoint);
                        }

                        if (enemyKilled.HighOrbs > 0)
                        {
                            // Drop HO
                            uint gainedHo = (uint)(enemyKilled.HighOrbs * _gameServer.GameLogicSettings.HoModifier);
                            CDataUpdateWalletPoint hoUpdateWalletPoint = _gameServer.WalletManager.AddToWallet(memberClient.Character, WalletType.HighOrbs, gainedHo, connectionIn: connectionIn);
                            updateCharacterItemNtc.UpdateWalletList.Add(hoUpdateWalletPoint);
                        }

                        if (updateCharacterItemNtc.UpdateItemList.Count != 0 || updateCharacterItemNtc.UpdateWalletList.Count != 0)
                        {
                            memberClient.Enqueue(updateCharacterItemNtc, queuedPackets);
                        }

                        if (gainedPP > 0)
                        {
                            var ntc = _gameServer.PPManager.AddPlayPoint(memberClient, gainedPP, type: 1, connectionIn:connectionIn);
                            memberClient.Enqueue(ntc, queuedPackets);
                        }

                        if (gainedExp > 0)
                        {
                            var ntcs = _gameServer.ExpManager.AddExp(memberClient, memberCharacter, gainedExp, RewardSource.Enemy, connectionIn: connectionIn); 
                            queuedPackets.AddRange(ntcs);
                        }
                    }
                    else if (member is PawnPartyMember pawnMember)
                    {
                        Pawn pawn = pawnMember.Pawn;
                        memberClient = _gameServer.ClientLookup.GetClientByCharacterId(pawn.CharacterId);
                        memberCharacter = pawn;

                        if (memberClient is null || memberClient.Character.Stage.Id != stageId.Id || pawn.IsRented)
                        {
                            // Only nearby allies get XP
                            // and non-rented pawns
                            continue;
                        }

                        uint pawnExp = gainedExp;
                        if (_gameServer.ExpManager.RequiresPawnCatchup(client.GameMode, client.Party, pawn))
                        {
                            pawnExp = _gameServer.ExpManager.GetAdjustedPawnExp(client.GameMode, RewardSource.Enemy, client.Party, pawn, enemyKilled.GetDroppedExperience(), enemyKilled.Lv);
                        }

                        if (pawnExp > 0)
                        {
                            var ntcs = _gameServer.ExpManager.AddExp(memberClient, memberCharacter, pawnExp, RewardSource.Enemy, connectionIn: connectionIn);
                            queuedPackets.AddRange(ntcs);
                        }
                    }
                    else
                    {
                        throw new Exception("Unknown member type");
                    }
                }
            });

            queuedPackets.Send();

            // TODO: EnemyId and KillNum
            return new S2CInstanceEnemyKillRes()
            {
                EnemyId = packet.IsNoBattleReward ? 0u : enemyKilled.Id,
                KillNum = packet.IsNoBattleReward ? 0u : 1u
            };

        }
    }
}
