using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class GpCourseManager
    {
        private DdonGameServer _Server;
        private Timer _CourseTimer;
        private CourseBonus _CourseBonus;
        private Dictionary<uint, bool> _CourseIsActive;

        public GpCourseManager(DdonGameServer server)
        {
            _Server = server;
            _CourseTimer = null;
            _CourseBonus = new CourseBonus();
            _CourseIsActive = new Dictionary<uint, bool>();
        }

        internal class CourseBonus
        {
            public double PlayerEnemyExpBonus = 0.0;
            public double PawnEnemyExpBonus = 0.0;
            public double WorldQuestExpBonus = 0.0;
            public double EnemyPlayPointBonus = 0.0;
            public double PawnCraftBonus = 0.0;
            public uint DisablePartyExpAdjustment = 0;
            public double EnemyBloodOrbMultiplier = 0.0;
            public uint InfiniteRevive = 0;
            public uint BazaarExhibitExtend = 0;
            public ulong BazaarReExhibitShorten = 0;
            public double BoardQuestApBonus = 0.0;
            public double WorldQuestApBonus = 0.0;
            public uint AreaMasterSupply = 0;
        };

        private void ApplyCourseEffects(uint courseId)
        {
            lock (_CourseBonus)
            {
                var courseDescription = _Server.AssetRepository.GPCourseInfoAsset.Courses[courseId];
                foreach (var effectUId in courseDescription.Effects)
                {
                    var effect = _Server.AssetRepository.GPCourseInfoAsset.Effects[effectUId];

                    GPCourseId courseEffectId = (GPCourseId)effect.Id;
                    switch (courseEffectId)
                    {
                        case GPCourseId.EnemyExpUp:
                            _CourseBonus.PlayerEnemyExpBonus += (effect.Param0 / 100.0);
                            break;
                        case GPCourseId.PawnEnemyExpUp:
                            _CourseBonus.PawnEnemyExpBonus += (effect.Param0 / 100.0);
                            break;
                        case GPCourseId.WQRewardExpUp:
                            _CourseBonus.WorldQuestExpBonus += (effect.Param0 / 100.0);
                            break;
                        case GPCourseId.EnemyPpUp:
                            _CourseBonus.EnemyPlayPointBonus += (effect.Param0 / 100.0);
                            break;
                        case GPCourseId.PawnCraftExpUp:
                            _CourseBonus.PawnCraftBonus += (effect.Param0 / 100.0);
                            break;
                        case GPCourseId.AreaPointBQRewardUp:
                            _CourseBonus.BoardQuestApBonus += (effect.Param0 / 100.0);
                            break;
                        case GPCourseId.AreaPointWQRewardUp:
                            _CourseBonus.WorldQuestApBonus += (effect.Param0 / 100.0);
                            break;
                        case GPCourseId.DisablePartyAdjustEnemyExp:
                            _CourseBonus.DisablePartyExpAdjustment += 1;
                            break;
                        case GPCourseId.BloodOrbUp:
                            _CourseBonus.EnemyBloodOrbMultiplier += (effect.Param0 / 100.0);
                            break;
                        case GPCourseId.InfiniteRevive:
                            _CourseBonus.InfiniteRevive += 1;
                            break;
                        case GPCourseId.BazaarExhibitExtend:
                            _CourseBonus.BazaarExhibitExtend += effect.Param0;
                            break;
                        case GPCourseId.BazaarReExhibitShorten:
                            _CourseBonus.BazaarReExhibitShorten += effect.Param0;
                            break;
                        case GPCourseId.AreaMasterSupply:
                            _CourseBonus.AreaMasterSupply += 1;
                            break;
                    }
                }
            }
        }

        private void RemoveCourseEffects(uint courseId)
        {
            lock (_CourseBonus)
            {
                var courseDescription = _Server.AssetRepository.GPCourseInfoAsset.Courses[courseId];
                foreach (var effectUId in courseDescription.Effects)
                {
                    var effect = _Server.AssetRepository.GPCourseInfoAsset.Effects[effectUId];

                    GPCourseId courseEffectId = (GPCourseId)effect.Id;
                    switch (courseEffectId)
                    {
                        case GPCourseId.EnemyExpUp:
                            _CourseBonus.PlayerEnemyExpBonus -= (effect.Param0 / 100.0);
                            break;
                        case GPCourseId.PawnEnemyExpUp:
                            _CourseBonus.PawnEnemyExpBonus -= (effect.Param0 / 100.0);
                            break;
                        case GPCourseId.WQRewardExpUp:
                            _CourseBonus.WorldQuestExpBonus -= (effect.Param0 / 100.0);
                            break;
                        case GPCourseId.EnemyPpUp:
                            _CourseBonus.EnemyPlayPointBonus -= (effect.Param0 / 100.0);
                            break;
                        case GPCourseId.AreaPointBQRewardUp:
                            _CourseBonus.BoardQuestApBonus -= (effect.Param0 / 100.0);
                            break;
                        case GPCourseId.AreaPointWQRewardUp:
                            _CourseBonus.WorldQuestApBonus -= (effect.Param0 / 100.0);
                            break;
                        case GPCourseId.PawnCraftExpUp:
                            _CourseBonus.PawnCraftBonus -= (effect.Param0 / 100.0);
                            break;
                        case GPCourseId.DisablePartyAdjustEnemyExp:
                            _CourseBonus.DisablePartyExpAdjustment -= 1;
                            break;
                        case GPCourseId.BloodOrbUp:
                            _CourseBonus.EnemyBloodOrbMultiplier -= (effect.Param0 / 100.0);
                            break;
                        case GPCourseId.InfiniteRevive:
                            _CourseBonus.InfiniteRevive -= 1;
                            break;
                        case GPCourseId.BazaarExhibitExtend:
                            _CourseBonus.BazaarExhibitExtend -= effect.Param0;
                            break;
                        case GPCourseId.BazaarReExhibitShorten:
                            _CourseBonus.BazaarReExhibitShorten -= effect.Param0;
                            break;
                        case GPCourseId.AreaMasterSupply:
                            _CourseBonus.AreaMasterSupply -= 1;
                            break;
                    }
                }
            }
        }

        private static int COURSE_TIMER_TICK = 1 * 1000; // 1 second in ms

        public void EvaluateCourses()
        {
            ulong now = (ulong) DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            foreach (var (id, course) in _Server.AssetRepository.GPCourseInfoAsset.ValidCourses)
            {
                if (now >= course.StartTime && now <= course.EndTime)
                {
                    _CourseIsActive[id] = true;
                    ApplyCourseEffects(id);
                }
                else
                {
                    _CourseIsActive[id] = false;
                }
            }

            _CourseTimer = new Timer(state =>
            {
                lock (_CourseIsActive)
                {
                    ulong now = (ulong)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    foreach (var (id, course) in _Server.AssetRepository.GPCourseInfoAsset.ValidCourses)
                    {
                        if (now >= course.StartTime && now <= course.EndTime && !_CourseIsActive[id])
                        {
                            _CourseIsActive[id] = true;
                            ApplyCourseEffects(id);

                            var ntc = new S2CGPCourseStartNtc()
                            {
                                CourseID = id,
                                ExpiryTimestamp = course.EndTime,
                                AnnounceType = 0
                            };
                            foreach (var client in _Server.ClientLookup.GetAll())
                            {
                                client.Send(ntc);
                            }
                        }
                        else if (now >= course.EndTime && _CourseIsActive[id])
                        {
                            
                            _CourseIsActive[id] = false;
                            RemoveCourseEffects(id);

                            var ntc = new S2CGpCourseEndNtc()
                            {
                                CourseID = id,
                                AnnounceType = 0
                            };
                            foreach (var client in _Server.ClientLookup.GetAll())
                            {
                                client.Send(ntc);
                            }
                        }
                    }
                }
            }, null, COURSE_TIMER_TICK, COURSE_TIMER_TICK);
        }

        public double EnemyExpBonus(CharacterCommon characterCommon)
        {
            lock (_CourseBonus)
            {
                if (characterCommon is Character)
                {

                    return _CourseBonus.PlayerEnemyExpBonus;
                }
                else
                {
                    return _CourseBonus.PawnEnemyExpBonus;
                }
            }
        }

        public double QuestExpBonus(QuestType questType)
        {
            lock (_CourseBonus)
            {
                switch (questType)
                {
                    case QuestType.World:
                        return _CourseBonus.WorldQuestExpBonus;
                    default:
                        return 0;
                }
            }
        }

        public double QuestApBonus(QuestType questType)
        {
            lock (_CourseBonus)
            {
                switch (questType)
                {
                    case QuestType.World:
                        return _CourseBonus.WorldQuestApBonus;
                    case QuestType.Board:
                        return _CourseBonus.BoardQuestApBonus;
                    default:
                        return 0;
                }
            }
        }

        public double EnemyPlayPointBonus()
        {
            lock (_CourseBonus)
            {
                return _CourseBonus.EnemyPlayPointBonus;
            }
        }

        public double PawnCraftBonus()
        {
            lock (_CourseBonus)
            {
                return _CourseBonus.PawnCraftBonus;
            }
        }

        public bool DisablePartyExpAdjustment()
        {
            lock (_CourseBonus)
            {
                return _CourseBonus.DisablePartyExpAdjustment > 0;
            }
        }

        public double EnemyBloodOrbBonus()
        {
            lock (_CourseBonus)
            {
                return _CourseBonus.EnemyBloodOrbMultiplier;
            }
        }

        public bool InfiniteReviveRefresh()
        {
            lock (_CourseBonus)
            {
                return _CourseBonus.InfiniteRevive > 0;
            }
        }

        public uint BazaarExhibitExtend()
        {
            lock (_CourseBonus)
            {
                return _CourseBonus.BazaarExhibitExtend;
            }
        }

        public ulong BazaarReExhibitShorten()
        {
            lock (_CourseBonus)
            {
                return _CourseBonus.BazaarReExhibitShorten;
            }
        }

        public bool AreaMasterSupply()
        {
            lock (_CourseBonus)
            {
                return _CourseBonus.AreaMasterSupply > 0;
            }
        }
    }
}
