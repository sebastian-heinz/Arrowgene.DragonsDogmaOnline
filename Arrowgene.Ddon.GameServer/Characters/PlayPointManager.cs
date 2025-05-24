using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
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
                return Server.GameSettings.GameServerSettings.PlayPointMax;
            }
        }

        public PlayPointManager(DdonGameServer server)
        {
            Server = server;
        }

        private readonly DdonGameServer Server;

        public void AddPlayPointNtc(GameClient client, (uint BasePoints, uint BonusPoints) gainedPoints, JobId? job = null, byte type = 1, DbConnection? connectionIn = null)
        {
            var ntc = AddPlayPoint(client, gainedPoints, job, type, connectionIn);
            client.Send(ntc);
        }

        public S2CJobUpdatePlayPointNtc AddPlayPoint(GameClient client, (uint BasePoints, uint BonusPoints) gainedPoints, JobId? job = null, byte type = 1, DbConnection? connectionIn = null)
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

            if (targetPlayPoint != null && targetPlayPoint.PlayPoint.PlayPoint < PP_MAX)
            {
                uint clampedNew = Math.Min(targetPlayPoint.PlayPoint.PlayPoint + gainedPoints.BasePoints + gainedPoints.BonusPoints, PP_MAX);
                targetPlayPoint.PlayPoint.PlayPoint = clampedNew;

                S2CJobUpdatePlayPointNtc ppNtc = new S2CJobUpdatePlayPointNtc()
                {
                    JobId = targetPlayPoint.Job,
                    UpdatePoint = gainedPoints.BasePoints + gainedPoints.BonusPoints,
                    ExtraBonusPoint = gainedPoints.BonusPoints,
                    TotalPoint = targetPlayPoint.PlayPoint.PlayPoint,
                    Type = type //Type == 1 (default) is "loud" and will show the UpdatePoint amount to the user, as both a chat log and floating text.
                };

                Server.Database.UpdateCharacterPlayPointData(client.Character.CharacterId, targetPlayPoint, connectionIn);
                return ppNtc;
            }

            return new();
        }

        public void RemovePlayPoint(GameClient client, uint removedPoints, JobId? jobId = null, byte type = 0)
        {
            if (jobId is null)
            {
                jobId = client.Character.ActiveCharacterJobData.Job;
            }
            client.Send(RemovePlayPoint2(client, jobId.Value, removedPoints, type));
        }

        public S2CJobUpdatePlayPointNtc RemovePlayPoint2(GameClient client, JobId jobId, uint amount, byte type = 0, DbConnection? connectionIn = null)
        {
            var targetPlayPoint = client.Character.PlayPointList.Where(x => x.Job == jobId).FirstOrDefault()
                    ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_CONTENTS_RELEASE_NOT_PLAY_POINT);

            if (targetPlayPoint.PlayPoint.PlayPoint < amount)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_JOB_VALUE_SHOP_VALUE_LACK, "Player does not have enough play points for the transaction");
            }

            targetPlayPoint.PlayPoint.PlayPoint = Math.Min(targetPlayPoint.PlayPoint.PlayPoint - amount, PP_MAX);

            Server.Database.UpdateCharacterPlayPointData(client.Character.CharacterId, targetPlayPoint, connectionIn);

            return new()
            {
                JobId = jobId,
                UpdatePoint = 0,
                ExtraBonusPoint = 0,
                TotalPoint = targetPlayPoint.PlayPoint.PlayPoint,
                Type = type
            };
        }
    }
}
