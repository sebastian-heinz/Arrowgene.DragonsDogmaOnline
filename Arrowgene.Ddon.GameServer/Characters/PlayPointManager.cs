using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Data.Common;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class PlayPointManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PlayPointManager));

        private uint PP_MAX {  get
            {
                return _Server.Setting.GameLogicSetting.PlayPointMax;
            }
        }

        public PlayPointManager(DdonGameServer server)
        {
            _Server = server;
        }

        private readonly DdonGameServer _Server;

        public void AddPlayPoint(GameClient client, uint gainedPoints, JobId? job = null, byte type = 1, DbConnection? connectionIn = null)
        {
            CDataJobPlayPoint? targetPlayPoint;
            if (job is null)
            {
                targetPlayPoint = client.Character.ActiveCharacterPlayPointData;
            }
            else
            {
                targetPlayPoint = client.Character.PlayPointList.Where(x => x.Job == job)
                    .FirstOrDefault()
                    ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_JOB_VALUE_SHOP_INVALID_JOB);
            }

            uint extraBonusPoints = (uint) (_Server.GpCourseManager.EnemyPlayPointBonus() * gainedPoints);
            if (targetPlayPoint != null && targetPlayPoint.PlayPoint.PlayPoint < PP_MAX)
            {
                uint clampedNew = Math.Min(targetPlayPoint.PlayPoint.PlayPoint + gainedPoints + extraBonusPoints, PP_MAX);
                targetPlayPoint.PlayPoint.PlayPoint = clampedNew;

                S2CJobUpdatePlayPointNtc ppNtc = new S2CJobUpdatePlayPointNtc()
                {
                    JobId = targetPlayPoint.Job,
                    UpdatePoint = gainedPoints + extraBonusPoints,
                    ExtraBonusPoint = extraBonusPoints,
                    TotalPoint = targetPlayPoint.PlayPoint.PlayPoint,
                    Type = type //Type == 1 (default) is "loud" and will show the UpdatePoint amount to the user, as both a chat log and floating text.
                };
                
                client.Send(ppNtc);

                _Server.Database.UpdateCharacterPlayPointData(client.Character.CharacterId, targetPlayPoint, connectionIn);
            }
        }

        public void RemovePlayPoint(GameClient client, uint removedPoints, JobId? job = null, byte type = 0)
        {
            CDataJobPlayPoint? targetPlayPoint;
            if (job is null)
            {
                targetPlayPoint = client.Character.ActiveCharacterPlayPointData;
            }
            else
            {
                targetPlayPoint = client.Character.PlayPointList.Where(x => x.Job == job)
                    .FirstOrDefault()
                    ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_JOB_VALUE_SHOP_INVALID_JOB);
            }
             
            if (targetPlayPoint != null && targetPlayPoint.PlayPoint.PlayPoint > 0)
            {
                uint clampedNew = Math.Min(targetPlayPoint.PlayPoint.PlayPoint - removedPoints, PP_MAX);
                targetPlayPoint.PlayPoint.PlayPoint = clampedNew;

                S2CJobUpdatePlayPointNtc ppNtc = new S2CJobUpdatePlayPointNtc()
                {
                    JobId = targetPlayPoint.Job,
                    UpdatePoint = 0,
                    ExtraBonusPoint = 0,
                    TotalPoint = targetPlayPoint.PlayPoint.PlayPoint,
                    Type = type //Type == 0 (default) is "silent" and will not notify the player, aside from updating some UI elements.
                };

                client.Send(ppNtc);

                _Server.Database.UpdateCharacterPlayPointData(client.Character.CharacterId, targetPlayPoint);
            }
        }
    }
}
