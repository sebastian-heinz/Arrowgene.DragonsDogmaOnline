using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class PartnerPawnManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartnerPawnManager));

        public static uint MAX_PARTNER_PAWN_LIKABILITY_RATING = 26;

        private DdonGameServer Server;

        public PartnerPawnManager(DdonGameServer server)
        {
            Server = server;
        }

        public Pawn GetPartnerPawn(GameClient client)
        {
            if (client.Character.PartnerPawnId == 0)
            {
                return null;
            }
            return client.Character.Pawns.Where(x => x.PawnId == client.Character.PartnerPawnId).FirstOrDefault();
        }

        public PartnerPawnData? GetPartnerPawnData(GameClient client, DbConnection? connectionIn = null)
        {
            if (client.Character.PartnerPawnId == 0)
            {
                return null;
            }
            return Server.Database.GetPartnerPawnRecord(client.Character.CharacterId, client.Character.PartnerPawnId, connectionIn);
        }

        public CDataPartnerPawnData? GetCDataPartnerPawnData(GameClient client, DbConnection? connectionIn = null)
        {
            var pawn = GetPartnerPawn(client);
            if (pawn == null)
            {
                return null;
            }

            var partnerPawnData = GetPartnerPawnData(client, connectionIn);
            return partnerPawnData.ToCDataPartnerPawnData(pawn);
        }

        public PacketQueue UpdateLikabilityIncreaseAction(GameClient client, PartnerPawnAffectionAction action, DbConnection? connectionIn = null)
        {
            PacketQueue packets = new();

            if (client.Character.PartnerPawnId == 0)
            {
                return new();
            }

            if (Server.Database.HasPartnerPawnLastAffectionIncreaseRecord(client.Character.CharacterId, client.Character.PartnerPawnId, action, connectionIn))
            {
                // MAX contributed for the day, do nothing
                return new();
            }

            Pawn partnerPawn = client.Character.Pawns.Find(x => x.PawnId == client.Character.PartnerPawnId);
            if (partnerPawn == null)
            {
                return new();
            }

            var previousLikability = partnerPawn.PartnerPawnData.CalculateLikability();
            switch (action)
            {
                case PartnerPawnAffectionAction.Gift:
                    partnerPawn.PartnerPawnData.NumGifts += 1;
                    break;
                case PartnerPawnAffectionAction.Craft:
                    partnerPawn.PartnerPawnData.NumCrafts += 1;
                    break;
                case PartnerPawnAffectionAction.Adventure:
                    partnerPawn.PartnerPawnData.NumAdventures += 1;
                    break;
            }
            Server.Database.UpdatePartnerPawnRecord(client.Character.CharacterId, partnerPawn.PartnerPawnData, connectionIn);
            Server.Database.InsertPartnerPawnLastAffectionIncreaseRecord(client.Character.CharacterId, client.Character.PartnerPawnId, action, connectionIn);

            var currentLikability = partnerPawn.PartnerPawnData.CalculateLikability();
            if (previousLikability < currentLikability)
            {
                packets.Enqueue(client, new S2CPartnerPawnLikabilityUpNtc()
                {
                    PawnId = client.Character.PartnerPawnId
                });

                if (gLikabilityRewards.ContainsKey(currentLikability))
                {
                    Server.Database.InsertPartnerPawnPendingReward(client.Character.CharacterId, client.Character.PartnerPawnId, currentLikability, connectionIn);
                }

                packets.AddRange(Server.AchievementManager.HandlePawnAffection(client, connectionIn));
            }

            return packets;
        }

        public bool IsActionConsumedForDay(GameClient client, PartnerPawnAffectionAction action, DbConnection? connectionIn = null)
        {
            if (client.Character.PartnerPawnId == 0)
            {
                return true;
            }
            return Server.Database.HasPartnerPawnLastAffectionIncreaseRecord(client.Character.CharacterId, client.Character.PartnerPawnId, action, connectionIn);
        }

        public bool IsPartnerPawnInParty(GameClient client)
        {
            return client.Party.Members.Where(x => x.IsPawn).Any(x => x.PawnId == client.Character.PartnerPawnId);
        }

        public uint GetLikabilityForCurrentPartnerPawn(GameClient client, DbConnection? connectionIn = null)
        {
            var partnerPawnData = GetPartnerPawnData(client, connectionIn);
            if (partnerPawnData == null)
            {
                return 0;
            }
            return partnerPawnData.CalculateLikability();
        }

        public List<CDataPartnerPawnReward> GetUnclaimedRewardsForCurrentPartnerPawn(GameClient client, DbConnection? connectionIn = null)
        {
            if (client.Character.PartnerPawnId == 0)
            {
                return new();
            }

            return Server.Database.GetPartnerPawnPendingRewards(client.Character.CharacterId, client.Character.PartnerPawnId, connectionIn)
                    .Where(x => gLikabilityRewards.ContainsKey(x))
                    .Select(x => gLikabilityRewards[x])
                    .ToList();
        }

        public List<CDataPartnerPawnReward> GetReleasedRewards(GameClient client, DbConnection? connectionIn = null)
        {
            var likeability = Server.PartnerPawnManager.GetLikabilityForCurrentPartnerPawn(client, connectionIn);
            if (likeability == 0)
            {
                return new();
            }

            var filteredTypes = new List<PawnLikabilityRewardType>()
            {
                PawnLikabilityRewardType.Talk,
                PawnLikabilityRewardType.Emotion,
                PawnLikabilityRewardType.Hair,
            };

            return gLikabilityRewards
                .Where(x => x.Key <= likeability)
                .Where(x => filteredTypes.Contains(x.Value.Type))
                .Select(x => x.Value)
                .ToList();
        }

        public uint GetLevelForReward(CDataPartnerPawnReward reward)
        {
            return gLikabilityRewards
                .Where(x => x.Value.Type == reward.Type)
                .Where(x => x.Value.Value.UID == reward.Value.UID)
                .Select(x => x.Key)
                .FirstOrDefault();
        }

        public bool CreateAdventureTimer(GameClient client)
        {
            lock (client.Character.PartnerTimerLockObj)
            {
                if (client.Character.PartnerPawnId == 0 || IsActionConsumedForDay(client, PartnerPawnAffectionAction.Adventure))
                {
                    // No partner pawn or
                    // can't start another adventure for credit
                    // until the next reset
                    return true;
                }

                if (client.Character.PartnerPawnAdventureTimerId != 0 || !StageManager.IsSafeArea(client.Character.Stage))
                {
                    Logger.Error(client, "Attempted to create an adventure timer in an invalid state");
                    return false;
                }

                client.Character.PartnerPawnAdventureTimerId = Server.TimerManager.CreateTimer(Server.GameSettings.GameServerSettings.PartnerPawnAdventureDurationInSeconds, () =>
                {
                    Logger.Info($"(PartnerPawn) Adventure timer for PartnerPawnId={client.Character.PartnerPawnAdventureTimerId} met");
                    CancelAdventureTimer(client);
                    UpdateLikabilityIncreaseAction(client, PartnerPawnAffectionAction.Adventure);
                });
                Logger.Info(client, $"(PartnerPawn) Adventure timer for PartnerPawnId={client.Character.PartnerPawnAdventureTimerId} created");
            }

            return true;
        }

        public void HandleStageAreaChange(GameClient client)
        {
            lock (client.Character.PartnerTimerLockObj)
            {
                var timerId = client.Character.PartnerPawnAdventureTimerId;
                if (timerId == 0)
                {
                    return;
                }

                // Handle transitioning from safe to dangerous area
                // Handle transitioning from dangerous to safe area
                // Dangerous to Dangerous can be considered ignored since the timer should be running still
                if (!Server.TimerManager.IsTimerStarted(timerId) && !StageManager.IsSafeArea(client.Character.Stage) ||
                    Server.TimerManager.IsTimerPaused(timerId) && !StageManager.IsSafeArea(client.Character.Stage))
                {
                    StartAdventureTimer(client);
                }
                else if (Server.TimerManager.IsTimerStarted(timerId) && StageManager.IsSafeArea(client.Character.Stage))
                {
                    PauseAdventureTimer(client);
                }
            }
        }

        private bool StartAdventureTimer(GameClient client)
        {
            lock (client.Character.PartnerTimerLockObj)
            {
                if (client.Character.PartnerPawnAdventureTimerId == 0)
                {
                    Logger.Error(client, "Attempted to start/resume the adventure timer but the timer id is invalid");
                    return false;
                }
                return Server.TimerManager.StartTimer(client.Character.PartnerPawnAdventureTimerId);
            }
        }

        private bool PauseAdventureTimer(GameClient client)
        {
            lock (client.Character.PartnerTimerLockObj)
            {
                if (client.Character.PartnerPawnAdventureTimerId == 0)
                {
                    Logger.Error(client, "Attempted to start/resume the adventure timer but the timer id is invalid");
                    return false;
                }

                Server.TimerManager.PauseTimer(client.Character.PartnerPawnAdventureTimerId);
                Logger.Info(client, $"(PartnerPawn) Adventure timer for PartnerPawnId={client.Character.PartnerPawnAdventureTimerId} paused");

                return true;
            }
        }

        public bool HandleLeaveFromParty(GameClient client)
        {
            lock (client.Character.PartnerTimerLockObj)
            {
                if (client.Character.PartnerPawnAdventureTimerId == 0)
                {
                    return true;
                }
                Logger.Info(client, $"(PartnerPawn) PartnerPawnId={client.Character.PartnerPawnAdventureTimerId} kicked/left party, canceling timer");
                return CancelAdventureTimer(client);
            }
        }

        private bool CancelAdventureTimer(GameClient client)
        {
            lock (client.Character.PartnerTimerLockObj)
            {
                if (client.Character.PartnerPawnAdventureTimerId == 0)
                {
                    Logger.Error(client, "Attempted to cancel the adventure timer but the timer id is invalid");
                    return false;
                }

                Server.TimerManager.CancelTimer(client.Character.PartnerPawnAdventureTimerId);
                client.Character.PartnerPawnAdventureTimerId = 0;
                return true;
            }
        }

        private static readonly Dictionary<uint,CDataPartnerPawnReward> gLikabilityRewards = new Dictionary<uint, CDataPartnerPawnReward>()
        {
            [1] = PartnerReward.CreateEmoteReward(EmoteId.ImHerePose1),
            // [2], TODO: Recipe: Achievement/Recipe: Dinner Set
            [3] = PartnerReward.CreateAbilityReward(AbilityId.CompanionHealth),
            [4] = PartnerReward.CreateAbilityReward(AbilityId.CompanionAttack),
            [5] = PartnerReward.CreateEmoteReward(EmoteId.ImHerePose2),
            // [6], TODO: Achievement/Recipe: Lestanian Puppet - Giant Cyclops
            [7] = PartnerReward.CreateCommunicationReward(2),
            [8] = PartnerReward.CreateHairstyleReward(HairStyleId.Ex1Men, 2),
            [9] = PartnerReward.CreateAbilityReward(AbilityId.CompanionDefense),
            [10] = PartnerReward.CreateEmoteReward(EmoteId.OriginalPose2),
            [11] = PartnerReward.CreateCommunicationReward(3),
            [12] = PartnerReward.CreateHairstyleReward(HairStyleId.Ex2Women, 2),
            [13] = PartnerReward.CreateAbilityReward(AbilityId.CompanionStamina),
            [14] = PartnerReward.CreateEmoteReward(EmoteId.ImHerePose3),
            [15] = PartnerReward.CreateCommunicationReward(4),
            [16] = PartnerReward.CreateHairstyleReward(HairStyleId.Ex3Women, 2),
            [17] = PartnerReward.CreateAbilityReward(AbilityId.CompanionMagick),
            [18] = PartnerReward.CreateAbilityReward(AbilityId.CompanionMagickDefense),
            [19] = PartnerReward.CreateEmoteReward(EmoteId.ImHerePose4),
            // [20] = TODO: Achievement 愛を叫んだ覚者
            [21] = PartnerReward.CreateHairstyleReward(HairStyleId.Ex4Women, 2),
            // [22], TODO: Recipe: Achievement/Recipe: Servant's Sleepwear (Type 1)
            [23] = PartnerReward.CreateCommunicationReward(5),
            // [24], TODO: Recipe: Achievement/Recipe: Servant's Sleepwear (Type 2)
            [25] = PartnerReward.CreateCommunicationReward(6),
        };
    }
}
