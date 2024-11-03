using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
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
                        PositionIndex = (byte) packet.SetId,
                        EnemyInfo = enemyKilled.asCDataStageLayoutEnemyPresetEnemyInfoClient()
                    }
                };
                client.Send(repopNtc);
            }
            else
            {
                enemyKilled.IsKilled = true;
            }

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
                client.Party.SendToAll(groupDestroyedNtc);

                if (IsAreaBoss && client.GameMode == GameMode.BitterblackMaze)
                {
                    foreach (var memberClient in client.Party.Clients)
                    {
                        BitterblackMazeManager.HandleTierClear(_gameServer, memberClient, memberClient.Character, stageId);
                    }
                }
            }

            if (packet.IsNoBattleReward)
            {
                return new();
            }

            foreach (var partyMemberClient in client.Party.Clients)
            {
                // If the enemy is quest controlled, then either get from the quest loot drop, or the general one.
                List<InstancedGatheringItem> instancedGatheringItems = new List<InstancedGatheringItem>();

                // Items from kill an enemy normally
                instancedGatheringItems.AddRange(IsQuestControlled ?
                            partyMemberClient.InstanceQuestDropManager.GenerateEnemyLoot(quest, enemyKilled, packet.LayoutId, packet.SetId) :
                            partyMemberClient.InstanceDropItemManager.GetAssets(layoutId, (int)packet.SetId));

                // Items for any server events which might be active
                instancedGatheringItems.AddRange(partyMemberClient.InstanceEventDropItemManager.GenerateEventItems(partyMemberClient, enemyKilled, packet.LayoutId, packet.SetId));

                // If the roll was unlucky, there is a chance that no bag will show.
                if (instancedGatheringItems.Where(x => x.ItemNum > 0).Any())
                {
                    partyMemberClient.Send(new S2CInstancePopDropItemNtc()
                    {
                        LayoutId = packet.LayoutId,
                        SetId = packet.SetId,
                        MdlType = enemyKilled.DropsTable.MdlType,
                        PosX = packet.DropPosX,
                        PosY = packet.DropPosY,
                        PosZ = packet.DropPosZ
                    });
                }
            }

            foreach (PartyMember member in client.Party.Members)
            {
                if (member.JoinState != JoinState.On) continue; // Only fully joined members get rewards.

                uint gainedExp = _gameServer.ExpManager.GetAdjustedExp(client.GameMode, RewardSource.Enemy, client.Party, enemyKilled.GetDroppedExperience(), enemyKilled.Lv);

                uint gainedPP = enemyKilled.GetDroppedPlayPoints();

                GameClient memberClient;
                CharacterCommon memberCharacter;
                if (member is PlayerPartyMember playerMember)
                {
                    memberClient = playerMember.Client;
                    memberCharacter = memberClient.Character;

                    if (memberCharacter.Stage.Id != stageId.Id) continue; // Only nearby allies get XP.

                    if (memberClient.Character.ActiveCharacterPlayPointData.PlayPoint.ExpMode == ExpMode.Experience && !IsQuestControlled)
                    {
                        gainedPP = 0;
                    }
                    else if (!IsQuestControlled)
                    {
                        gainedExp = 0;
                    }

                    // TODO: Add transaction?
                    playerMember.QuestState.HandleEnemyHuntRequests(enemyKilled);

                    S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();

                    if (enemyKilled.BloodOrbs > 0)
                    {
                        // Drop BO
                        uint gainedBo = enemyKilled.BloodOrbs;
                        uint bonusBo = (uint)(enemyKilled.BloodOrbs * _gameServer.GpCourseManager.EnemyBloodOrbBonus());
                        CDataUpdateWalletPoint boUpdateWalletPoint = _gameServer.WalletManager.AddToWallet(memberClient.Character, WalletType.BloodOrbs, gainedBo + bonusBo, bonusBo);
                        updateCharacterItemNtc.UpdateWalletList.Add(boUpdateWalletPoint);
                    }

                    if (enemyKilled.HighOrbs > 0)
                    {
                        // Drop HO
                        uint gainedHo = enemyKilled.HighOrbs;
                        CDataUpdateWalletPoint hoUpdateWalletPoint = _gameServer.WalletManager.AddToWallet(memberClient.Character, WalletType.HighOrbs, gainedHo);
                    }

                    if (updateCharacterItemNtc.UpdateItemList.Count != 0 || updateCharacterItemNtc.UpdateWalletList.Count != 0)
                    {
                        memberClient.Send(updateCharacterItemNtc);
                    }

                    if (gainedPP > 0)
                    {
                        _gameServer.PPManager.AddPlayPoint(memberClient, gainedPP, type: 1);
                    }

                    if (gainedExp > 0)
                    {
                        _gameServer.ExpManager.AddExp(memberClient, memberCharacter, gainedExp, RewardSource.Enemy);
                    }
                }
                else if (member is PawnPartyMember pawnMember)
                {
                    Pawn pawn = pawnMember.Pawn;
                    memberClient = _gameServer.ClientLookup.GetClientByCharacterId(pawn.CharacterId);
                    memberCharacter = pawn;

                    if (memberClient.Character.Stage.Id != stageId.Id || pawn.IsRented)
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
                        _gameServer.ExpManager.AddExp(memberClient, memberCharacter, pawnExp, RewardSource.Enemy);
                    }
                }
                else
                {
                    throw new Exception("Unknown member type");
                }
            }

            // TODO: EnemyId and KillNum
            return new S2CInstanceEnemyKillRes()
            {
                EnemyId = enemyKilled.Id,
                KillNum = 1
            };

        }
    }
}
