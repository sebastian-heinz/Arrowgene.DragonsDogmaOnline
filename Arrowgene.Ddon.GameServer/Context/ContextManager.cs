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

        public static bool RemoveContext(PartyGroup partyGroup, ulong uniqueId)
        {
            lock (partyGroup.Contexts)
            {
                return partyGroup.Contexts.Remove(uniqueId);
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

        public static bool IsOmUID(ulong value)
        {
            return ((byte) Kind.Get(value)) == (byte) UIDKind.OM;
        }

        public static uint GetStageId(ulong value)
        {
            return (uint) StageId.Get(value);
        }

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
            List<InstancedEnemy> enemies = enemyManager.GetAssets(stageLayoutId);

            List<ulong> results = new List<ulong>();
            for (int i = 0; i < enemies.Count(); i++)
            {
                results.Add(CreateEnemyUID((ulong) i, stageLayoutId));
            }

            return results;
        }

        public static void HandleEntry(GameClient client, CDataStageLayoutId stageLayout)
        {
            List<ulong> enemiesIds = CreateEnemyUIDs(client.Party.InstanceEnemyManager, stageLayout);
            var clientIndex = Math.Max(client.Party.ClientIndex(client), 0);

            foreach (var enemyId in enemiesIds)
            {
                if (client.Character.ContextOwnership.ContainsKey(enemyId))
                {
                    Logger.Error($"Attempting to double assign context for ${clientIndex}:{enemyId:x16}");
                    continue;
                }
                var otherClients = client.Party.Clients.Where(x => x != client && x.Character.ContextOwnership.ContainsKey(enemyId));
                if (otherClients.Any())
                {
                    //Somebody else got here first, so wait in line.
                    AwaitMaster(client, enemyId);
                }
                else
                {
                    //Take ownership of it
                    AssignMaster(client, enemyId, clientIndex);
                }
            }
        }

        public static void AssignMaster(GameClient client, CDataStageLayoutId stageLayout)
        {
            List<ulong> enemiesIds = CreateEnemyUIDs(client.Party.InstanceEnemyManager, stageLayout);
            var clientIndex = Math.Max(client.Party.ClientIndex(client), 0);

            foreach (var enemyId in enemiesIds)
            {
                AssignMaster(client, enemyId, clientIndex);
            }
        }

        public static void AssignMaster(GameClient client, ulong uniqueID, int clientIndex = -99)
        {
            if (clientIndex == -99)
            {
                clientIndex = client.Party.ClientIndex(client);
            }

            client.Character.ContextOwnership[uniqueID] = true;
            foreach (var partyClient in client.Party.Clients)
            {
                if (partyClient == client) continue;
                else if (partyClient.Character.ContextOwnership.ContainsKey(uniqueID))
                {
                    partyClient.Character.ContextOwnership[uniqueID] = false;
                }
            }

            client.Party.SendToAll(new S2CContextMasterChangeNtc()
            {
                Info = new List<CDataMasterInfo>()
                        {
                            new CDataMasterInfo()
                            {
                                UniqueId = uniqueID,
                                MasterIndex = (sbyte)clientIndex
                            }
                        }
            });
        }

        public static void AssignMaster(GameClient client, List<ulong> uniqueIDs, int clientIndex = -1)
        {
            if (clientIndex == -1)
            {
                clientIndex = client.Party.ClientIndex(client);
            }
            var ntc = new S2CContextMasterChangeNtc();

            foreach (var uniqueID in uniqueIDs)
            {
                client.Character.ContextOwnership[uniqueID] = true;
                ntc.Info.Add(new CDataMasterInfo()
                {
                    UniqueId = uniqueID,
                    MasterIndex = (sbyte)clientIndex
                });
            }

            client.Party.SendToAll(ntc);
        }

        public static void AwaitMaster(GameClient client, CDataStageLayoutId stageLayout)
        {
            List<ulong> enemiesIds = CreateEnemyUIDs(client.Party.InstanceEnemyManager, stageLayout);
            var clientIndex = Math.Max(client.Party.ClientIndex(client), 0);

            foreach (var enemyId in enemiesIds)
            {
                AwaitMaster(client, enemyId, clientIndex);
            }
        }

        public static void AwaitMaster(GameClient client, ulong uniqueID, int clientIndex = -1)
        {
            if (clientIndex == -1)
            {
                clientIndex = client.Party.ClientIndex(client);
            }

            client.Character.ContextOwnership[uniqueID] = false;
        }

        public static void DelegateMaster(GameClient client, CDataStageLayoutId stageLayout)
        {
            List<ulong> enemiesIds = CreateEnemyUIDs(client.Party.InstanceEnemyManager, stageLayout);

            foreach (var enemyId in enemiesIds)
            {
                DelegateMaster(client, enemyId);
            }
        }

        public static void DelegateMaster(GameClient client, ulong uniqueID)
        {
            bool isOwner = client.Character.ContextOwnership.ContainsKey(uniqueID)
                && client.Character.ContextOwnership[uniqueID];

            client.Character.ContextOwnership.Remove(uniqueID);

            if (isOwner)
            {
                var otherClients = client.Party.Clients.Where(x => x.Character.ContextOwnership.ContainsKey(uniqueID));
                if (otherClients.Any())
                {
                    GameClient newOwner = otherClients.First();
                    AssignMaster(newOwner, uniqueID);
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
            foreach (var key in client.Character.ContextOwnership.Keys)
            {
                DelegateMaster(client, key);
            }
        }
    }
}
