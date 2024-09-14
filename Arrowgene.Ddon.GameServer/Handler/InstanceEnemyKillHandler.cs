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
    public class InstanceEnemyKillHandler : StructurePacketHandler<GameClient, C2SInstanceEnemyKillReq>
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

        public override void Handle(GameClient client, StructurePacket<C2SInstanceEnemyKillReq> packet)
        {
            CDataStageLayoutId layoutId = packet.Structure.LayoutId;
            StageId stageId = StageId.FromStageLayoutId(layoutId);

            // The training room uses special handling to produce enemies that don't exist in the QuestState or InstanceEnemyManager.
            // Return an empty response here to not break the rest of the handling.
            if (_ignoreKillsInStageIds.Contains(stageId.Id))
            {
                client.Send(new S2CInstanceEnemyKillRes());
                return;
            }

            Quest quest = null;
            bool IsQuestControlled = false;
            foreach (var questId in client.Party.QuestState.StageQuests(stageId))
            {
                quest = client.Party.QuestState.GetQuest(questId);
                if (client.Party.QuestState.HasEnemiesInCurrentStageGroup(quest, stageId))
                {
                    IsQuestControlled = true;
                    break;
                }
            }

            InstancedEnemy enemyKilled = client.Party.InstanceEnemyManager.GetInstanceEnemy(stageId, (byte)packet.Structure.SetId);
            if (enemyKilled.RepopCount > 0 && enemyKilled.RepopNum < enemyKilled.RepopCount)
            {
                enemyKilled.RepopNum += 1;

                S2CInstanceEnemyRepopNtc repopNtc = new S2CInstanceEnemyRepopNtc()
                {
                    LayoutId = layoutId,
                    WaitSecond = enemyKilled.RepopWaitSecond,
                    EnemyData = new CDataLayoutEnemyData()
                    {
                        PositionIndex = (byte) packet.Structure.SetId,
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
                    LayoutId = packet.Structure.LayoutId,
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

            // TODO: EnemyId and KillNum
            client.Send(new S2CInstanceEnemyKillRes()
            {
                EnemyId = enemyKilled.Id,
                KillNum = 1
            });

            if (packet.Structure.IsNoBattleReward)
            {
                return;
            }

            foreach (var partyMemberClient in client.Party.Clients)
            {
                // If the enemy is quest controlled, then either get from the quest loot drop, or the general one.
                List<InstancedGatheringItem> instancedGatheringItems = IsQuestControlled ?
                            partyMemberClient.InstanceQuestDropManager.GenerateEnemyLoot(quest, enemyKilled, packet.Structure.LayoutId, packet.Structure.SetId) :
                            partyMemberClient.InstanceDropItemManager.GetAssets(layoutId, (int)packet.Structure.SetId);

                // If the roll was unlucky, there is a chance that no bag will show.
                if (instancedGatheringItems.Count > 0)
                {
                    partyMemberClient.Send(new S2CInstancePopDropItemNtc()
                    {
                        LayoutId = packet.Structure.LayoutId,
                        SetId = packet.Structure.SetId,
                        MdlType = enemyKilled.DropsTable.MdlType,
                        PosX = packet.Structure.DropPosX,
                        PosY = packet.Structure.DropPosY,
                        PosZ = packet.Structure.DropPosZ
                    });
                }
            }

            foreach (PartyMember member in client.Party.Members)
            {
                if (member.JoinState != JoinState.On) continue; // Only fully joined members get rewards.

                uint ho = enemyKilled.HighOrbs;
                uint gainedExp = _gameServer.ExpManager.GetAdjustedExp(client.GameMode, RewardSource.Enemy, client.Party, enemyKilled.GetDroppedExperience(), enemyKilled.Lv);

                uint gainedPP = enemyKilled.GetDroppedPlayPoints();

                GameClient memberClient;
                CharacterCommon memberCharacter;
                if (member is PlayerPartyMember)
                {
                    memberClient = ((PlayerPartyMember)member).Client;
                    memberCharacter = memberClient.Character;

                    if (memberCharacter.Stage.Id != stageId.Id) continue; // Only nearby allies get XP.

                    if (memberClient.Character.ActiveCharacterPlayPointData.PlayPoint.ExpMode == ExpMode.Experience)
                    {
                        gainedPP = 0;
                    }
                    else
                    {
                        gainedExp = 0;
                    }

                    S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();

                    if (enemyKilled.BloodOrbs > 0)
                    {
                        // Drop BO
                        CDataWalletPoint boWallet = memberClient.Character.WalletPointList.Where(wp => wp.Type == WalletType.BloodOrbs).Single();

                        uint gainedBo = enemyKilled.BloodOrbs;
                        uint bonusBo = (uint)(enemyKilled.BloodOrbs * _gameServer.GpCourseManager.EnemyBloodOrbBonus());
                        boWallet.Value += gainedBo + bonusBo;

                        CDataUpdateWalletPoint boUpdateWalletPoint = new CDataUpdateWalletPoint();
                        boUpdateWalletPoint.Type = WalletType.BloodOrbs;
                        boUpdateWalletPoint.AddPoint = (int)(gainedBo + bonusBo);
                        boUpdateWalletPoint.ExtraBonusPoint = bonusBo;
                        boUpdateWalletPoint.Value = boWallet.Value;
                        updateCharacterItemNtc.UpdateWalletList.Add(boUpdateWalletPoint);

                        // PERSIST CHANGES IN DB
                        Server.Database.UpdateWalletPoint(memberClient.Character.CharacterId, boWallet);
                    }

                    if (enemyKilled.HighOrbs > 0)
                    {
                        // Drop HO
                        CDataWalletPoint hoWallet = memberClient.Character.WalletPointList.Where(wp => wp.Type == WalletType.HighOrbs).Single();
                        hoWallet.Value += ho;

                        CDataUpdateWalletPoint hoUpdateWalletPoint = new CDataUpdateWalletPoint();
                        hoUpdateWalletPoint.Type = WalletType.HighOrbs;
                        hoUpdateWalletPoint.AddPoint = (int)ho;
                        hoUpdateWalletPoint.Value = hoWallet.Value;
                        updateCharacterItemNtc.UpdateWalletList.Add(hoUpdateWalletPoint);

                        // PERSIST CHANGES IN DB
                        Server.Database.UpdateWalletPoint(memberClient.Character.CharacterId, hoWallet);
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
                else if (member is PawnPartyMember)
                {
                    Pawn pawn = ((PawnPartyMember)member).Pawn;
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
        }
    }
}
