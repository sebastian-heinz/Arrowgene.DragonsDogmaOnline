using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Clan;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class ClanManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanManager));

        private readonly DdonGameServer Server;

        private readonly Dictionary<uint, CDataClanParam> ClanParams;
        private readonly Dictionary<uint, List<CDataClanMemberInfo>> ClanMembers;

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
                ClanParams.Add(id, Server.Database.SelectClan(id));
            }
            return ClanParams[id];
        }

        public CDataClanParam CreateClan(GameClient client, CDataClanUserParam createParam)
        {
            var serverParam = new CDataClanServerParam()
            {
                Lv = 1,
                MemberNum = 1,
                MasterInfo = NewMaster(client.Character),
                IsSystemRestriction = false,
                IsClanBaseRelease = false,
                CanClanBaseRelease = false,
                TotalClanPoint = 0,
                MoneyClanPoint = 0,
                NextClanPoint = TOTAL_CP_FOR_ADV[1],
            };

            CDataClanParam newClan = new CDataClanParam()
            {
                ClanServerParam = serverParam,
                ClanUserParam = createParam
            };
            newClan.ClanUserParam.Created = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            Server.Database.CreateClan(newClan);

            ClanParams.Add(newClan.ClanServerParam.ID, newClan);

            client.Character.ClanId = newClan.ClanServerParam.ID;
            client.Character.ClanName.Name = newClan.ClanUserParam.Name;
            client.Character.ClanName.ShortName = newClan.ClanUserParam.ShortName;

            return newClan;
        }

        public void UpdateClan(GameClient client, CDataClanUserParam updateParam)
        {
            if (client.Character.ClanId != 0)
            {
                var clan = Server.ClanManager.GetClan(client.Character.ClanId);
                updateParam.Name = clan.ClanUserParam.Name;
                clan.ClanUserParam = updateParam;
                Server.Database.UpdateClan(clan);

                foreach (var otherClient in Server.ClientLookup.GetAll())
                {
                    if (otherClient.Character != null && otherClient.Character.ClanId == client.Character.ClanId)
                    {
                        otherClient.Send(new S2CClanClanUpdateNtc());
                    }
                }
            }
        }

        public void AddMemberToClan(Character character, uint clanId)
        {
            var clan = ClanParams[clanId];
            clan.ClanServerParam.MemberNum++;

            var newMember = NewMember(character);
            Server.Database.InsertClanMember(newMember, clanId);

            character.ClanId = clanId;
            character.ClanName.Name = clan.ClanUserParam.Name;
            character.ClanName.ShortName = clan.ClanUserParam.Name;
        }

        public void LeaveClan(Character character, uint clanId)
        {
            if (clanId == 0) return;

            var clan = GetClan(clanId);
            clan.ClanServerParam.MemberNum--;

            character.ClanId = 0;
            character.ClanName.Name = string.Empty;
            character.ClanName.ShortName = string.Empty;

            bool leaderLeaving = false;
            if (clan.ClanServerParam.MasterInfo.CharacterListElement.CommunityCharacterBaseInfo.CharacterId == character.CharacterId)
            {
                clan.ClanServerParam.MasterInfo = new CDataClanMemberInfo();
                leaderLeaving = true;
            }

            Server.Database.ExecuteInTransaction(conn =>
            {
                Server.Database.DeleteClanMember(character.CharacterId, clanId, conn);
                if (leaderLeaving)
                {
                    Server.Database.UpdateClan(clan, conn);
                }
            });

            var ntc = new S2CClanClanLeaveMemberNtc()
            {
                ClanId = clanId
            };
            GameStructure.CDataCharacterListElement(ntc.CharacterListElement, character);
            foreach (var client in Server.ClientLookup.GetAll())
            {
                client.Send(ntc);
            }
        }

        public static CDataClanMemberInfo NewMaster(Character character)
        {
            var info = new CDataClanMemberInfo()
            {
                Rank = ClanMemberRank.Master,
                Created = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                LastLoginTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                LeaveTime = 0,
                Permission = ALL_PERMISSIONS
            };
            GameStructure.CDataCharacterListElement(info.CharacterListElement, character);
            return info;
        }

        public static CDataClanMemberInfo NewMember(Character character)
        {
            var info = new CDataClanMemberInfo()
            {
                Rank = ClanMemberRank.Apprentice,
                Created = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                LastLoginTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                LeaveTime = 0,
                Permission = 0
            };
            GameStructure.CDataCharacterListElement(info.CharacterListElement, character);
            return info;
        }

        public List<CDataClanMemberInfo> MemberList(uint clanId)
        {
            Logger.Info($"Fetching memberlist for clan {clanId}");

            if (clanId == 0) return new();

            var clan = GetClan(clanId);
            var memberList = Server.Database.GetClanMemberList(clanId);
            foreach (var member in memberList)
            {
                CDataCommunityCharacterBaseInfo memberInfo = member.CharacterListElement.CommunityCharacterBaseInfo;
                memberInfo.ClanName = clan.ClanUserParam.ShortName;
                GameClient lookup = Server.ClientLookup.GetClientByCharacterId(memberInfo.CharacterId);
                if (lookup != null)
                {
                    member.CharacterListElement.OnlineStatus = lookup.Character.OnlineStatus;
                }
                else
                {
                    member.CharacterListElement.OnlineStatus = OnlineStatus.Offline;
                }
            }
            return memberList;
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
