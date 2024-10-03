using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
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
        private static readonly uint ALL_PERMISSIONS = 6143;
        private static readonly uint SUBMASTER_ALL_PERMISSIONS = 5002;

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
                MemberNum = 0,
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
            GetClan(newClan.ClanServerParam.ID).ClanServerParam.MemberNum = 1;

            client.Character.ClanId = newClan.ClanServerParam.ID;
            client.Character.ClanName.Name = newClan.ClanUserParam.Name;
            client.Character.ClanName.ShortName = newClan.ClanUserParam.ShortName;

            var joinNtc = new S2CClanClanJoinNtc()
            {
                CharacterId = client.Character.CharacterId
            };

            //var selfNtc = new S2CClanClanJoinSelfNtc()
            //{
            //    ClanParam = newClan,
            //    SelfInfo = serverParam.MasterInfo,
            //    MemberList = new List<CDataClanMemberInfo>() { serverParam.MasterInfo }
            //};
            //client.Send(selfNtc);

            //S2CUserListJoinNtc newUserNtc = new S2CUserListJoinNtc()
            //{
            //    UserList = new List<CDataLobbyMemberInfo>()
            //    {
            //        new CDataLobbyMemberInfo()
            //        {
            //            CharacterId = client.Character.CharacterId,
            //            FirstName = client.Character.FirstName,
            //            LastName = client.Character.LastName,
            //            ClanName = client.Character.ClanName.ShortName,
            //            Unk0 = 1, // Platform PC?
            //            Unk1 = 0,
            //            OnlineStatus = OnlineStatus.Online  // OnlineStatus?
            //        },
            //    }
            //};

            foreach (GameClient otherClient in Server.ClientLookup.GetAll())
            {
                otherClient.Send(joinNtc);
            }

            return newClan;
        }

        public void UpdateClan(GameClient client, CDataClanUserParam updateParam)
        {
            if (client.Character.ClanId != 0)
            {
                var clan = Server.ClanManager.GetClan(client.Character.ClanId);
                updateParam.Name = clan.ClanUserParam.Name;
                updateParam.Created = clan.ClanUserParam.Created;
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

        public void JoinClan(uint characterId, uint clanId)
        {
            var client = Server.ClientLookup.GetClientByCharacterId(characterId);
            var character = client.Character;
            var clan = GetClan(clanId);

            clan.ClanServerParam.MemberNum++;

            var memberInfo = NewMember(character);
            Server.Database.InsertClanMember(memberInfo, clanId);

            character.ClanId = clanId;
            character.ClanName.Name = clan.ClanUserParam.Name;
            character.ClanName.ShortName = clan.ClanUserParam.Name;

            S2CClanClanJoinNtc joinNtc = new()
            {
                CharacterId = characterId,
            };

            S2CClanClanJoinSelfNtc selfntc = new()
            {
                ClanParam = clan,
                SelfInfo = memberInfo,
                MemberList = MemberList(clanId)
            };

            S2CClanClanJoinMemberNtc memberNtc = new()
            {
                ClanId = clanId,
                MemberInfo = memberInfo,
            };

            client.Send(selfntc);
            SendToClan(clanId, memberNtc);
            foreach (var otherClient in Server.ClientLookup.GetAll())
            {
                otherClient.Send(memberNtc);
            }
        }

        public void LeaveClan(uint characterId, uint clanId)
        {
            if (clanId == 0) return;

            var clan = GetClan(clanId);
            clan.ClanServerParam.MemberNum--;

            bool leaderLeaving = false;
            bool clanDelete = false;
            if (clan.ClanServerParam.MasterInfo.CharacterListElement.CommunityCharacterBaseInfo.CharacterId == characterId)
            {
                clan.ClanServerParam.MasterInfo = new CDataClanMemberInfo();
                leaderLeaving = true;
            }
            if (clan.ClanServerParam.MemberNum == 0)
            {
                clanDelete = true;
            }

            var memberList = MemberList(clanId);
            var character = memberList.Where(x => x.CharacterListElement.CommunityCharacterBaseInfo.CharacterId == characterId).FirstOrDefault();

            Server.Database.ExecuteInTransaction(conn =>
            {
                Server.Database.DeleteClanMember(characterId, clanId, conn);
                if (leaderLeaving && !clanDelete)
                {
                    Server.Database.UpdateClan(clan, conn);
                }
                else if (clanDelete)
                {
                    Server.Database.DeleteClan(clan, conn);
                    ClanParams.Remove(clanId);
                }
            });

            Character characterLookup = Server.ClientLookup.GetClientByCharacterId(characterId)?.Character;
            if (character != null)
            {
                character.CharacterListElement.CommunityCharacterBaseInfo.ClanName = string.Empty;
                characterLookup.ClanId = 0;
                characterLookup.ClanName.Name = string.Empty;
                characterLookup.ClanName.ShortName = string.Empty;
            }

            var ntc = new S2CClanClanLeaveMemberNtc()
            {
                ClanId = 0,
                CharacterListElement = character.CharacterListElement
            };
            foreach (var client in Server.ClientLookup.GetAll())
            {
                client.Send(ntc);
            }
        }

        public void SetMemberRank(uint characterId, uint clanId, uint rank, uint permission)
        {
            if (clanId == 0) return;

            CDataClanMemberInfo memberInfo = null;

            Server.Database.ExecuteInTransaction(conn =>
            {
                memberInfo = Server.Database.GetClanMember(characterId, conn);

                memberInfo.Rank = (ClanMemberRank)rank;
                memberInfo.Permission = permission;

                Server.Database.UpdateClanMember(memberInfo, clanId, conn);
            });

            SendToClan(clanId, new S2CClanClanSetMemberRankNtc()
            {
                ClanId = clanId,
                CharacterId = characterId,
                Rank = rank,
                Permission = permission
            });
        }

        public void NegotiateMaster(uint characterId, uint clanId)
        {
            if (clanId == 0) return;

            uint prevMasterId = MasterId(clanId);

            CDataClanMemberInfo memberInfo = null;
            CDataClanMemberInfo prevMasterInfo = null;

            Server.Database.ExecuteInTransaction(conn =>
            {
                memberInfo = Server.Database.GetClanMember(characterId, conn);
                memberInfo.Rank = ClanMemberRank.Master;
                memberInfo.Permission = ALL_PERMISSIONS;
                Server.Database.UpdateClanMember(memberInfo, clanId, conn);

                if (prevMasterId != 0)
                {
                    prevMasterInfo = Server.Database.GetClanMember(prevMasterId, conn);
                    prevMasterInfo.Rank = ClanMemberRank.SubMaster;
                    prevMasterInfo.Permission = SUBMASTER_ALL_PERMISSIONS;
                    Server.Database.UpdateClanMember(prevMasterInfo, clanId, conn);
                }
            });

            var masterNtc = new S2CClanClanNegotiateMasterNtc()
            {
                ClanId = clanId,
                MemberInfo = memberInfo
            };

            SendToClan(clanId, masterNtc);

            if (prevMasterInfo != null)
            {
                SendToClan(clanId, new S2CClanClanSetMemberRankNtc()
                {
                    ClanId = clanId,
                    CharacterId = prevMasterId,
                    Rank = (uint)prevMasterInfo.Rank,
                    Permission = prevMasterInfo.Permission
                });
            }
        }

        public uint MasterId(uint clanId)
        {
            if (clanId == 0) return 0;

            return GetClan(clanId).ClanServerParam.MasterInfo.CharacterListElement.CommunityCharacterBaseInfo.CharacterId;
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

        public void SendToClan<T>(uint clanId, T packet)
            where T : class, IPacketStructure, new()
        {
            foreach (var client in Server.ClientLookup.GetAll())
            {
                if (client.Character != null && client.Character.ClanId == clanId)
                {
                    client.Send(packet);
                }
            }
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
