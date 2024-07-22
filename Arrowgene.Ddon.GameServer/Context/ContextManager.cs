using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Context
{
    public class ContextManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ContextManager));

        public static Tuple<CDataContextSetBase, CDataContextSetAdditional> GetContext(PartyGroup partyGroup, ulong uniqueId)
        {
            Tuple<CDataContextSetBase, CDataContextSetAdditional> contextData;

            lock (partyGroup.Contexts)
            {
                if (!partyGroup.Contexts.ContainsKey(uniqueId))
                {
                    return null;
                }

                contextData = partyGroup.Contexts[uniqueId];
            }

            return contextData;
        }

        public static void SetContext(PartyGroup partyGroup, ulong uniqueId, Tuple<CDataContextSetBase, CDataContextSetAdditional> context)
        {
            lock (partyGroup.Contexts)
            {
                partyGroup.Contexts[uniqueId] = context;
            }
        }

        public static Tuple<CDataContextSetBase, CDataContextSetAdditional> SetAndGetContext(PartyGroup partyGroup, ulong uniqueId, Tuple<CDataContextSetBase, CDataContextSetAdditional> context)
        {
            lock (partyGroup.Contexts)
            {
                partyGroup.Contexts[uniqueId] = context;
            }
            return context;
        }

        public enum UIDKind : byte
        {
            None = 0,
            Player = 1,
            Enemy = 2,
            OM = 3,
            OM_SCR = 4,
            NPC = 5,
            SHL = 6,
            Quest = 7
        }

        private static readonly Bitfield Kind          = new Bitfield( 3,  0, "Kind");
        private static readonly Bitfield StageId       = new Bitfield(15,  4, "StageId");
        private static readonly Bitfield LayoutGroup   = new Bitfield(24, 16, "LayoutGroup");
        private static readonly Bitfield LayoutId      = new Bitfield(29, 25, "LayoutId");
        private static readonly Bitfield InnerId       = new Bitfield(34, 30, "InnerId");

        public static ulong CreateEnemyUID(ulong setId, CDataStageLayoutId stageLayoutId)
        {
            return (Kind.Value((ulong) UIDKind.Enemy) |
                    StageId.Value(stageLayoutId.StageId) |
                    LayoutGroup.Value(stageLayoutId.GroupId) |
                    LayoutId.Value(setId) |
                    InnerId.Value(0) |
                    0x80000000_00000000);
        }

        public static List<ulong> CreateEnemyUIDs(InstanceEnemyManager enemyManager, CDataStageLayoutId stageLayoutId)
        {   
            List<InstancedEnemy> enemies = enemyManager.GetAssets(stageLayoutId, 0);

            List<ulong> results = new List<ulong>();
            for (int i = 0; i < enemies.Count(); i++)
            {
                results.Add(CreateEnemyUID((ulong) i, stageLayoutId));
            }

            return results;
        }

        public static void AssignMaster(GameClient client, CDataStageLayoutId stageLayoutId)
        {
            client.Character.EnemyLayoutOwnership[stageLayoutId.AsTuple()] = true;
            List<ulong> enemiesIds = CreateEnemyUIDs(client.Party.InstanceEnemyManager, stageLayoutId);
            var clientIndex = client.Party.ClientIndex(client);

            Logger.Debug($"{client.Character.FirstName} taking {stageLayoutId}");
            foreach (var enemyId in enemiesIds)
            {
                var context = GetContext(client.Party, enemyId);
                var master = context != null ? context.Item2.MasterIndex : -99;

                //if (GetContext(client.Party, enemyId) == null)
                //{
                //    continue;
                //}

                Logger.Debug($"Enemy {enemyId} assigned to client {master}->{clientIndex} : {client.Character.FirstName}");
                client.Party.SendToAll(new S2CContextMasterChangeNtc()
                {
                    Info = new List<CDataMasterInfo>()
                        {
                            new CDataMasterInfo()
                            {
                                UniqueId = enemyId,
                                MasterIndex = (sbyte)clientIndex
                            }
                        }
                });
            }
            
        }

        public static void DelegateMaster(GameClient client, (uint, byte, uint) stageLayoutTuple)
        {
            bool isOwner = client.Character.EnemyLayoutOwnership.ContainsKey(stageLayoutTuple) 
                && client.Character.EnemyLayoutOwnership[stageLayoutTuple];

            client.Character.EnemyLayoutOwnership.Remove(stageLayoutTuple);

            if (isOwner)
            {
                var otherClients = client.Party.Clients.Where(x => x.Character.EnemyLayoutOwnership.ContainsKey(stageLayoutTuple));
                if (otherClients.Any())
                {
                    GameClient newOwner = otherClients.First();
                    Logger.Debug($"{client.Character.FirstName} delegating {stageLayoutTuple} to {newOwner.Character.FirstName}");

                    AssignMaster(newOwner, 
                        new CDataStageLayoutId(
                            stageLayoutTuple.Item1, 
                            stageLayoutTuple.Item2, 
                            stageLayoutTuple.Item3
                            )
                        );
                }
                else
                {
                    //TODO: Test if returning to host is correct?
                    return;
                }
            }
        }

        public static void DelegateAllMasters(GameClient client)
        {
            foreach (var key in client.Character.EnemyLayoutOwnership.Keys)
            {
                DelegateMaster(client, key);
            }
        }
    }
}
