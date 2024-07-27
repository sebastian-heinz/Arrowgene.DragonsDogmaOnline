using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class PlayPointManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PlayPointManager));

        private static uint PP_MAX = 2000;

        public PlayPointManager(IDatabase database, GameClientLookup gameClientLookup)
        {
            this._database = database;
            this._gameClientLookup = gameClientLookup;
        }

        protected readonly IDatabase _database;
        protected readonly GameClientLookup _gameClientLookup;

        public void AddPlayPoint(GameClient client, uint gainedPoints, uint extraBonusPoints, byte type = 1)
        {
            CDataJobPlayPoint? activeCharacterPlayPoint = client.Character.ActiveCharacterPlayPointData;

            if (activeCharacterPlayPoint != null && activeCharacterPlayPoint.PlayPoint.PlayPoint < PP_MAX)
            {
                uint clampedNew = (uint)Math.Clamp((long)activeCharacterPlayPoint.PlayPoint.PlayPoint + gainedPoints + extraBonusPoints, 0, PP_MAX);
                activeCharacterPlayPoint.PlayPoint.PlayPoint = clampedNew;

                S2CJobUpdatePlayPointNtc ppNtc = new S2CJobUpdatePlayPointNtc()
                {
                    JobId = activeCharacterPlayPoint.Job,
                    UpdatePoint = gainedPoints,
                    ExtraBonusPoint = extraBonusPoints,
                    TotalPoint = activeCharacterPlayPoint.PlayPoint.PlayPoint,
                    Type = type //Only pops up if type == 1.
                };
                
                client.Send(ppNtc);

                this._database.UpdateCharacterPlayPointData(client.Character.CommonId, activeCharacterPlayPoint);
            }
        }

        public void RemovePlayPoint(GameClient client, uint removedPoints, byte type = 0)
        {
            CDataJobPlayPoint? activeCharacterPlayPoint = client.Character.ActiveCharacterPlayPointData;

            if (activeCharacterPlayPoint != null && activeCharacterPlayPoint.PlayPoint.PlayPoint > 0)
            {
                uint clampedNew = (uint)Math.Clamp((long)activeCharacterPlayPoint.PlayPoint.PlayPoint - removedPoints, 0, PP_MAX);
                activeCharacterPlayPoint.PlayPoint.PlayPoint = clampedNew;

                S2CJobUpdatePlayPointNtc ppNtc = new S2CJobUpdatePlayPointNtc()
                {
                    JobId = activeCharacterPlayPoint.Job,
                    UpdatePoint = 0,
                    ExtraBonusPoint = 0,
                    TotalPoint = activeCharacterPlayPoint.PlayPoint.PlayPoint,
                    Type = type
                };

                client.Send(ppNtc);

                this._database.UpdateCharacterPlayPointData(client.Character.CommonId, activeCharacterPlayPoint);
            }
        }
    }
}
