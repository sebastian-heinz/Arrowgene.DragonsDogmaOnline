using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Context
{
    public class ContextManager
    {
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
    }
}
