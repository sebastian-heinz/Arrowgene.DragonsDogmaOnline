using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Clan;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class ClanManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanManager));

        private readonly DdonGameServer Server;

        private readonly Dictionary<uint, ClanParam> ClanParams;

        private static readonly CDataClanParam NULL_CLAN = new();
        private static readonly uint ALL_PERMISSIONS = 2047;

        public static readonly uint[] TOTAL_CP_FOR_ADV = new uint[] {
            /********/   0,
            /* Lv 1 */   500,
            /* Lv 2 */   2000,
            /* Lv 3 */   5000,
            /* Lv 4 */   9000,
            /* Lv 5 */   16000,
            /* Lv 6 */   26000,
            /* Lv 7 */   40000,
            /* Lv 8 */   58000,
            /* Lv 9 */   80000,
            /* Lv 10 */  106000,
            /* Lv 11 */  136000,
            /* Lv 12 */  170000,
            /* Lv 13 */  208000,
            /* Lv 14 */  250000,
            /* Lv 15 */  350000,
            /* Lv 16 */  450000, // Missing from wiki, guessed this value.
            /* Lv 17 */  600000,
            /* Lv 18 */  800000,
            /* Lv 19 */  1000000,
        };

        private bool IsHead
        {
            get
            {
                return Server.ChannelManager.IsHead;
            }
        }

        public ClanManager(DdonGameServer server)
        {
            Server = server;
            ClanParams = new();
        }

        public CDataClanParam GetClan(uint id)
        {
            if (id == 0) return NULL_CLAN;

            if (!ClanParams.ContainsKey(id))
            {
                ClanParams.Add(id, new ClanParam(Server.Database.SelectClan(id)));
            }
            return ClanParams[id].ToCDataClanParam();
        }

        public CDataClanParam CreateClan(GameClient client, CDataClanUserParam createParam)
        {
            var memberInfo = new CDataClanMemberInfo()
            {
                Rank = ClanMemberRank.Master,
                Created = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                LastLoginTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                LeaveTime = 0,
                Permission = ALL_PERMISSIONS
            };

            GameStructure.CDataCharacterListElement(memberInfo.CharacterListElement, client.Character);

            var serverParam = new CDataClanServerParam()
            {
                Lv = 1,
                MemberNum = 1,
                MasterInfo = memberInfo,
                IsSystemRestriction = false,
                IsClanBaseRelease = false,
                CanClanBaseRelease = false,
                TotalClanPoint = 0,
                MoneyClanPoint = 0,
                NextClanPoint = TOTAL_CP_FOR_ADV[1],
            };

            ClanParam newClan = new(createParam, serverParam)
            {
                Created = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };
            Server.Database.CreateClan(newClan);

            ClanParams.Add(newClan.ID, newClan);
            client.Character.ClanId = newClan.ID;
            client.Character.ClanName = newClan.ClanName;

            return newClan.ToCDataClanParam();
        }

        private static int BitsToInt(List<int> bitindices)
        {
            return bitindices.Aggregate(0, (sum, val) => sum + 2 ^ (val));
        }

        private static List<int> IntToBits(int input)
        {
            var res = new List<int>();
            BitVector32 bitvector = new BitVector32(input);
            for (int i = 0; i < 32; i++)
            {
                if (bitvector[i])
                {
                    res.Add(i);
                }
            }
            return res;
        }
    }
}
