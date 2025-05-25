using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Clan;
using Arrowgene.Ddon.Shared.Model.Rpc;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Common;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class PeriodicSync<T>
    {
        private DateTime LastSync = DateTime.MinValue;
        private TimeSpan SyncPeriod;
        internal T Obj { get; set; }

        public PeriodicSync(T obj, TimeSpan syncPeriod)
        {
            Obj = obj;
            SyncPeriod = syncPeriod;
        }

        public bool NeedSync
        {
            get
            {
                return (DateTime.UtcNow - LastSync) > SyncPeriod;
            }
        }

        public void Sync()
        {
            LastSync = DateTime.UtcNow;
        }
    }

    public class ClanManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanManager));

        private readonly DdonGameServer Server;

        private readonly Dictionary<uint, PeriodicSync<CDataClanParam>> ClanParams;
        private readonly Dictionary<uint, List<CDataClanMemberInfo>> ClanMembers;

        /// <summary>
        /// <![CDATA[<CharacterId, <QuestScheduleId, ClearCount>>]]>
        /// </summary>
        private readonly Dictionary<uint, Dictionary<uint, uint>> ClanQuestClearCount;

        private static readonly CDataClanParam NULL_CLAN = new();
        private static readonly uint ALL_PERMISSIONS = 6143;
        private static readonly uint SUBMASTER_ALL_PERMISSIONS = 5002;

        private static readonly TimeSpan CLAN_CACHE_TIME = TimeSpan.FromHours(1);

        /// <summary>
        /// Packets that get sent to all channels, even if there isn't a clan member there to recieve them.
        /// These have side-effects on the internal tracking of clans.
        /// </summary>
        public static readonly HashSet<PacketId> INTERNAL_IMPORTANT_PACKETS = new HashSet<PacketId>()
        {
            PacketId.S2C_CLAN_CLAN_UPDATE_NTC,
            PacketId.S2C_CLAN_CLAN_LEAVE_MEMBER_NTC,
            PacketId.S2C_CLAN_CLAN_JOIN_MEMBER_NTC,
            PacketId.S2C_CLAN_CLAN_LEVEL_UP_NTC,
            PacketId.S2C_CLAN_CLAN_POINT_ADD_NTC,
            PacketId.S2C_CLAN_CLAN_BASE_RELEASE_STATE_UPDATE_NTC,
            PacketId.S2C_CLAN_CLAN_QUEST_CLEAR_NTC,
            PacketId.S2C_CLAN_CLAN_SHOP_BUY_ITEM_NTC
        };

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
            /* Lv MAX */ 0,
        };
        public static uint MAX_CLAN_LEVEL = 20;

        public ClanManager(DdonGameServer server)
        {
            Server = server;
            ClanParams = new();
            ClanQuestClearCount = new();
        }

        private void AddClan(CDataClanParam clan)
        {
            var syncObject = new PeriodicSync<CDataClanParam>(clan, CLAN_CACHE_TIME);
            ClanParams.Add(clan.ClanServerParam.ID, syncObject);
        }

        public CDataClanParam GetClan(uint id, DbConnection? connectionIn = null)
        {
            if (id == 0) return NULL_CLAN;

            lock(ClanParams)
            {
                if (!ClanParams.ContainsKey(id))
                {
                    var clan = Server.Database.SelectClan(id, connectionIn);

                    if (clan.ClanServerParam.ID == 0)
                    {
                        // Failed to fetch the clan, because it doesn't exist.
                        // Throw an exception?
                        throw new ResponseErrorException(ErrorCode.ERROR_CODE_CLAN_NOT_EXIST);
                    }

                    clan.ClanServerParam.NextClanPoint = TOTAL_CP_FOR_ADV[clan.ClanServerParam.Lv];
                    AddClan(clan);

                    return clan;
                }
                else
                {
                    if (ClanParams[id].NeedSync)
                    {
                        var clan = Server.Database.SelectClan(id, connectionIn);
                        clan.ClanServerParam.NextClanPoint = TOTAL_CP_FOR_ADV[clan.ClanServerParam.Lv];
                        ClanParams[id].Obj = clan;
                        ClanParams[id].Sync();
                    }
                    return ClanParams[id].Obj;
                }
            }
        }

        public bool HasClan(uint id)
        {
            lock (ClanParams)
            {
                return ClanParams.ContainsKey(id);
            }
        }
    
        public void ResyncClan(uint id, DbConnection? connectionIn = null)
        {
            lock (ClanParams)
            {
                if (ClanParams.ContainsKey(id))
                {
                    var clan = Server.Database.SelectClan(id, connectionIn);
                    clan.ClanServerParam.NextClanPoint = TOTAL_CP_FOR_ADV[clan.ClanServerParam.Lv];
                    ClanParams[id].Obj = clan;
                    ClanParams[id].Sync();
                }
            }
        }

        public CDataClanParam CreateClan(GameClient client, CDataClanUserParam createParam)
        {
            var masterInfo = NewMaster(client.Character);
            masterInfo.CharacterListElement.CommunityCharacterBaseInfo.ClanName = createParam.ShortName;

            var serverParam = new CDataClanServerParam()
            {
                Lv = 1,
                MemberNum = 0,
                MasterInfo = masterInfo,
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
            newClan.ClanUserParam.Created = DateTimeOffset.UtcNow;
            Server.Database.CreateClan(newClan);

            AddClan(newClan);
            newClan.ClanServerParam.MemberNum = 1;

            client.Character.ClanId = newClan.ClanServerParam.ID;
            client.Character.ClanName.Name = newClan.ClanUserParam.Name;
            client.Character.ClanName.ShortName = newClan.ClanUserParam.ShortName;

            var joinNtc = new S2CClanClanJoinNtc()
            {
                CharacterId = client.Character.CharacterId
            };

            var selfNtc = new S2CClanClanJoinSelfNtc()
            {
                ClanParam = newClan,
                SelfInfo = masterInfo,
                MemberList = new List<CDataClanMemberInfo>() { masterInfo }
            };
            client.Send(selfNtc);

            foreach (GameClient otherClient in Server.ClientLookup.GetAll())
            {
                otherClient.Send(joinNtc);
            }
            Server.RpcManager.AnnouncePlayerList();

            return newClan;
        }

        public void UpdateClan(GameClient client, CDataClanUserParam updateParam, DbConnection? connectionIn = null)
        {
            if (client.Character.ClanId != 0)
            {
                var clan = Server.ClanManager.GetClan(client.Character.ClanId);
                updateParam.Name = clan.ClanUserParam.Name;
                updateParam.Created = clan.ClanUserParam.Created;
                clan.ClanUserParam = updateParam;
                Server.Database.UpdateClan(clan, connectionIn);

                SendToClan(client.Character.ClanId, new S2CClanClanUpdateNtc());
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
            character.ClanName.ShortName = clan.ClanUserParam.ShortName;
            memberInfo.CharacterListElement.CommunityCharacterBaseInfo.ClanName = clan.ClanUserParam.ShortName;

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
            foreach (var otherClient in Server.ClientLookup.GetAll())
            {
                otherClient.Send(joinNtc);
            }
            SendToClan(clanId, memberNtc);
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
            var memberInfo = memberList.Where(x => x.CharacterListElement.CommunityCharacterBaseInfo.CharacterId == characterId).FirstOrDefault();

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

            if (memberInfo != null)
            {
                memberInfo.CharacterListElement.CommunityCharacterBaseInfo.ClanName = string.Empty;
                var ntc = new S2CClanClanLeaveMemberNtc()
                {
                    ClanId = 0,
                    CharacterListElement = memberInfo.CharacterListElement
                };
                foreach (var client in Server.ClientLookup.GetAll())
                {
                    client.Send(ntc);
                }
                Server.RpcManager.AnnounceClanPacket(clanId, ntc);
            }

            Character characterLookup = Server.ClientLookup.GetClientByCharacterId(characterId)?.Character;
            if (characterLookup != null)
            {
                characterLookup.ClanId = 0;
                characterLookup.ClanName.Name = string.Empty;
                characterLookup.ClanName.ShortName = string.Empty;
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

            var rankNtc = new S2CClanClanSetMemberRankNtc()
            {
                ClanId = clanId,
                CharacterId = characterId,
                Rank = rank,
                Permission = permission
            };

            SendToClan(clanId, rankNtc);
            Server.RpcManager.AnnounceClanPacket(clanId, rankNtc);
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
            Server.RpcManager.AnnounceClanPacket(clanId, masterNtc);

            if (prevMasterInfo != null)
            {
                var submasterNtc = new S2CClanClanSetMemberRankNtc()
                {
                    ClanId = clanId,
                    CharacterId = prevMasterId,
                    Rank = (uint)prevMasterInfo.Rank,
                    Permission = prevMasterInfo.Permission
                };
                SendToClan(clanId, submasterNtc);
                Server.RpcManager.AnnounceClanPacket(clanId, submasterNtc);
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
                Created = DateTimeOffset.UtcNow,
                LastLoginTime = DateTimeOffset.UtcNow,
                Permission = ALL_PERMISSIONS
            };
            GameStructure.CDataCharacterListElement(info.CharacterListElement, character);
            return info;
        }

        public static CDataClanMemberInfo NewMember(Character character)
        {
            var info = new CDataClanMemberInfo()
            {
                Rank = ClanMemberRank.Member,
                Created = DateTimeOffset.UtcNow,
                LastLoginTime = DateTimeOffset.UtcNow,
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
                    var channelId = Server.RpcManager.FindPlayerById(memberInfo.CharacterId);
                    if (channelId > 0)
                    {
                        member.CharacterListElement.OnlineStatus = OnlineStatus.Online;
                        member.CharacterListElement.ServerId = channelId;
                    }
                    else
                    {
                        member.CharacterListElement.OnlineStatus = OnlineStatus.Offline;
                    }
                }
            }
            return memberList;
        }

        public (uint ClanId, CDataClanMemberInfo MemberInfo) ClanMembership(uint characterId)
        {
            uint clanId = 0;
            CDataClanMemberInfo membership = new();
            Server.Database.ExecuteInTransaction(conn =>
            {
                clanId = Server.Database.SelectClanMembershipByCharacterId(characterId, conn);
                membership = Server.Database.GetClanMember(characterId, conn);
            });

            return (clanId, membership);
        }

        public PacketQueue CompleteClanQuest(Quest quest, GameClient client)
        {
            PacketQueue queue = new PacketQueue();
            var character = client.Character;
            if (character.ClanId == 0) return queue;

            uint characterId = character.CharacterId;
            if (!ClanQuestClearCount.ContainsKey(characterId))
            {
                ClanQuestClearCount[characterId] = new();
            }

            var ntc = new S2CClanClanQuestClearNtc() { QuestId = (uint)quest.QuestId };

            lock (ClanQuestClearCount[characterId])
            {
                if (ClanQuestClearCount[characterId].GetValueOrDefault(quest.QuestScheduleId) < quest.LightQuestDetail.OrderLimit)
                {
                    ClanQuestClearCount[characterId][quest.QuestScheduleId] = ClanQuestClearCount[characterId].GetValueOrDefault(quest.QuestScheduleId) + 1;
                    
                    Server.RpcManager.AnnounceOthers("internal/command", RpcInternalCommand.NotifyClanQuestCompletion, new RpcQuestCompletionData()
                    {
                        CharacterId = characterId,
                        QuestStatus = ClanQuestClearCount[characterId]
                    });
                }

                if (ClanQuestClearCount[characterId].GetValueOrDefault(quest.QuestScheduleId) == quest.LightQuestDetail.OrderLimit)
                {
                    client.Enqueue(ntc, queue);
                }
            }
            
            return queue;
        }

        // For syncing across channels.
        public void UpdateClanQuestCompletion(uint characterId, Dictionary<uint, uint> questStatus)
        {
            ClanQuestClearCount[characterId] = questStatus;
        }

        public uint ClanQuestCompletionStatistics(uint characterId, uint questScheduleId)
        {
            if (!QuestManager.IsClanQuest(QuestManager.GetQuestByScheduleId(questScheduleId))
                || !ClanQuestClearCount.ContainsKey(characterId))
            { 
                return 0; 
            }
            return ClanQuestClearCount[characterId].GetValueOrDefault(questScheduleId);
        }

        public List<CDataLightQuestClearList> ClanQuestCompletionStatistics(uint characterId)
        {
            if(!ClanQuestClearCount.ContainsKey(characterId))
            {
                return new();
            }
            return ClanQuestClearCount[characterId].Select(x => new CDataLightQuestClearList()
            {
                ScheduleId = x.Key,
                ClearNum = x.Value
            }).ToList();
        }

        public bool CanGetClanQuestRewards(Quest quest, uint characterId)
        {
            uint count = ClanQuestClearCount.GetValueOrDefault(characterId)?.GetValueOrDefault(quest.QuestScheduleId) ?? 0;
            return count < quest.LightQuestDetail.GetCp;
        }

        public PacketQueue AddClanPoint(uint clanId, uint amount, DbConnection? connectionIn = null)
        {
            PacketQueue queue = new();
            if (clanId == 0) return queue;

            var clan = GetClan(clanId, connectionIn);
            lock(clan)
            {
                clan.ClanServerParam.TotalClanPoint += amount;
                clan.ClanServerParam.MoneyClanPoint += amount;

                var pointNtc = new S2CClanClanPointAddNtc()
                {
                    ClanPoint = amount,
                    MoneyClanPoint = clan.ClanServerParam.MoneyClanPoint,
                    TotalClanPoint = clan.ClanServerParam.TotalClanPoint
                };
                EnqueueToClan(clanId, pointNtc, queue);
                Server.RpcManager.AnnounceClanPacket(clanId, pointNtc);

                if (clan.ClanServerParam.TotalClanPoint >= clan.ClanServerParam.NextClanPoint
                    && clan.ClanServerParam.Lv < MAX_CLAN_LEVEL)
                {
                    clan.ClanServerParam.Lv = (ushort)(clan.ClanServerParam.Lv + 1);
                    clan.ClanServerParam.NextClanPoint = TOTAL_CP_FOR_ADV[clan.ClanServerParam.Lv];

                    // TODO: Clan History
                    var levelupNtc = new S2CClanClanLevelUpNtc()
                    {
                        ClanLevel = clan.ClanServerParam.Lv,
                        NextClanPoint = clan.ClanServerParam.NextClanPoint
                    };
                    EnqueueToClan(clanId, levelupNtc, queue);
                    Server.RpcManager.AnnounceClanPacket(clanId, levelupNtc);

                    if (clan.ClanServerParam.Lv >= 3 && !clan.ClanServerParam.IsClanBaseRelease)
                    {
                        clan.ClanServerParam.CanClanBaseRelease = true;
                        var releaseNtc = new S2CClanClanBaseReleaseStateUpdateNtc()
                        {
                            State = 2
                        };
                        EnqueueToClanPermission(clanId, releaseNtc, ClanPermission.BaseRelease, queue);
                        Server.RpcManager.AnnounceClanPacket(clanId, releaseNtc);
                    }
                }

                Server.Database.UpdateClan(clan, connectionIn);
            }

            return queue;
        }

        public void BaseRelease(uint clanId)
        {
            if (clanId == 0) return;

            var clan = GetClan(clanId);

            lock(clan)
            {
                clan.ClanServerParam.IsClanBaseRelease = true;
                clan.ClanServerParam.CanClanBaseRelease = false;

                Server.Database.UpdateClan(clan);
            }

            var ntc = new S2CClanClanBaseReleaseStateUpdateNtc()
            {
                State = 3
            };
            SendToClan(clanId, ntc);
            Server.RpcManager.AnnounceClanPacket(clanId, ntc);
        }

        public void SendToClan<T>(uint clanId, T packet)
            where T : class, IPacketStructure, new()
        {
            if (clanId == 0) return;

            foreach (var client in Server.ClientLookup.GetAll())
            {
                if (client.Character != null && client.Character.ClanId == clanId)
                {
                    client.Send(packet);
                }
            }
            Server.RpcManager.AnnounceClanPacket(clanId, packet);
        }

        public void SendToClanPermission<T>(uint clanId, T packet, ClanPermission perm)
            where T : class, IPacketStructure, new()
        {
            if (clanId == 0) return;
            var memberList = MemberList(clanId);

            foreach (GameClient client in Server.ClientLookup.GetAll())
            {
                if (client.Character.ClanId == clanId)
                {
                    var member = memberList.Find(x => x.CharacterListElement.CommunityCharacterBaseInfo.CharacterId == client.Character.CharacterId);
                    var splitPerms = IntToEnums<ClanPermission>((int)member.Permission);
                    if (splitPerms.Contains(perm))
                    {
                        client.Send(packet);
                    }
                }
            }
        }

        public void EnqueueToClan<T>(uint clanId, T res, PacketQueue queue)
            where T : class, IPacketStructure, new()
        {
            StructurePacket<T> packet = new StructurePacket<T>(res);
            foreach (GameClient client in Server.ClientLookup.GetAll())
            {
                if (client.Character is not null && client.Character.ClanId == clanId)
                {
                    queue.Enqueue((client, packet));
                }
            }
        }

        public void EnqueueToClanPermission<T>(uint clanId, T res, ClanPermission perm, PacketQueue queue)
            where T : class, IPacketStructure, new()
        {
            if (clanId == 0) return;
            var memberList = MemberList(clanId);
            StructurePacket<T> packet = new StructurePacket<T>(res);

            foreach (GameClient client in Server.ClientLookup.GetAll())
            {
                if (client.Character.ClanId == clanId)
                {
                    var member = memberList.Find(x => x.CharacterListElement.CommunityCharacterBaseInfo.CharacterId == client.Character.CharacterId);
                    var splitPerms = IntToEnums<ClanPermission>((int)member.Permission);
                    if (splitPerms.Contains(perm))
                    {
                        queue.Enqueue((client, packet));
                    }
                }
            }
        }

        public HashSet<GameClient> LookupClientsByPermission(uint clanId, ClanPermission perm)
        {
            var ret = new HashSet<GameClient>();
            if (clanId == 0) { return ret; }
            var memberList = MemberList(clanId);
            foreach (GameClient client in Server.ClientLookup.GetAll())
            {
                if (client.Character.ClanId == clanId)
                {
                    var member = memberList.Find(x => x.CharacterListElement.CommunityCharacterBaseInfo.CharacterId == client.Character.CharacterId);
                    var splitPerms = IntToEnums<ClanPermission>((int)member.Permission);
                    if (splitPerms.Contains(perm))
                    {
                        ret.Add(client);
                    }
                }
            }

            return ret;
        }

        // Will likely need this later for clan searching.
        private static bool ClanSearchBitmaskMatch(uint searchParam, uint match)
        {
            return (searchParam & match) == match;
        }

        private static int BitsToInt(IEnumerable<int> bitindices)
        {
            return bitindices.Aggregate(0, (sum, val) => sum + (1 << val));
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

        private static int EnumsToInt<T>(IEnumerable<T> enums)
            where T : struct, IConvertible
        {
            return BitsToInt(enums.Select(x => (int)(object)x));
        }

        private static List<T> IntToEnums<T>(int input)
            where T : struct, IConvertible
        {
            return IntToBits(input).Select(x => (T)(object)x).ToList();
        }
    }
}
