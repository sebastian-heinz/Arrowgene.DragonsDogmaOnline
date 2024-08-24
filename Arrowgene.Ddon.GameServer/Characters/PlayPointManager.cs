using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class PlayPointManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PlayPointManager));

        private static uint PP_MAX = 2000;

        public PlayPointManager(DdonGameServer server)
        {
            _Server = server;
        }

        private readonly DdonGameServer _Server;

        public void AddPlayPoint(GameClient client, uint gainedPoints, byte type = 1)
        {
            CDataJobPlayPoint? activeCharacterPlayPoint = client.Character.ActiveCharacterPlayPointData;

            uint extraBonusPoints = (uint) (_Server.GpCourseManager.EnemyPlayPointBonus() * gainedPoints);
            if (activeCharacterPlayPoint != null && activeCharacterPlayPoint.PlayPoint.PlayPoint < PP_MAX)
            {
                uint clampedNew = Math.Min(activeCharacterPlayPoint.PlayPoint.PlayPoint + gainedPoints + extraBonusPoints, PP_MAX);
                activeCharacterPlayPoint.PlayPoint.PlayPoint = clampedNew;

                S2CJobUpdatePlayPointNtc ppNtc = new S2CJobUpdatePlayPointNtc()
                {
                    JobId = activeCharacterPlayPoint.Job,
                    UpdatePoint = gainedPoints + extraBonusPoints,
                    ExtraBonusPoint = extraBonusPoints,
                    TotalPoint = activeCharacterPlayPoint.PlayPoint.PlayPoint,
                    Type = type //Type == 1 (default) is "loud" and will show the UpdatePoint amount to the user, as both a chat log and floating text.
                };
                
                client.Send(ppNtc);

                _Server.Database.UpdateCharacterPlayPointData(client.Character.CharacterId, activeCharacterPlayPoint);
            }
        }

        public void RemovePlayPoint(GameClient client, uint removedPoints, byte type = 0)
        {
            CDataJobPlayPoint? activeCharacterPlayPoint = client.Character.ActiveCharacterPlayPointData;

            if (activeCharacterPlayPoint != null && activeCharacterPlayPoint.PlayPoint.PlayPoint > 0)
            {
                uint clampedNew = Math.Min(activeCharacterPlayPoint.PlayPoint.PlayPoint - removedPoints, PP_MAX);
                activeCharacterPlayPoint.PlayPoint.PlayPoint = clampedNew;

                S2CJobUpdatePlayPointNtc ppNtc = new S2CJobUpdatePlayPointNtc()
                {
                    JobId = activeCharacterPlayPoint.Job,
                    UpdatePoint = 0,
                    ExtraBonusPoint = 0,
                    TotalPoint = activeCharacterPlayPoint.PlayPoint.PlayPoint,
                    Type = type //Type == 0 (default) is "silent" and will not notify the player, aside from updating some UI elements.
                };

                client.Send(ppNtc);

                _Server.Database.UpdateCharacterPlayPointData(client.Character.CharacterId, activeCharacterPlayPoint);
            }
        }
    }
}
