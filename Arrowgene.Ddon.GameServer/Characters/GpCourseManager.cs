using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System;
using System.Collections.Generic;
using System.Linq;
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
            public bool DisablePartyExpAdjustment = false;
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
                        case GPCourseId.DisablePartyAdjustEnemyExp:
                            _CourseBonus.DisablePartyExpAdjustment = true;
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
                        case GPCourseId.DisablePartyAdjustEnemyExp:
                            _CourseBonus.DisablePartyExpAdjustment = false;
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

        public double EnemyPlayPointBonus()
        {
            lock (_CourseBonus)
            {
                return _CourseBonus.EnemyPlayPointBonus;
            }
        }

        public bool DisablePartyExpAdjustment()
        {
            lock (_CourseBonus)
            {
                return _CourseBonus.DisablePartyExpAdjustment;
            }
        }
    }
}
