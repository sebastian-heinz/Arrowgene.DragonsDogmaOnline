using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer.GatheringItems;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.GameServer.Shop;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Networking.Tcp;
using System;

namespace Arrowgene.Ddon.GameServer
{
    public class GameClient : Client
    {
        public GameClient(ITcpSocket socket, PacketFactory packetFactory, DdonGameServer server) : base(socket, packetFactory)
        {
            UpdateIdentity();
            InstanceGatheringItemManager = new InstanceGatheringItemManager(this, server);
            InstanceDropItemManager = new(this, server);
            InstanceShopManager = new InstanceShopManager(server.ShopManager);
            GameMode = GameMode.Normal;
        }

        public void UpdateIdentity()
        {
            string newIdentity = $"[GameClient@{Socket.Identity}]";
            if (Account != null)
            {
                newIdentity += $"[Acc:({Account.Id}){Account.NormalName}]";
            }

            if (Character != null)
            {
                newIdentity += $"[Cha:({Character.CharacterId}){Character.FirstName} {Character.LastName}]";
            }

            Identity = newIdentity;
        }

        public Account Account { get; set; }

        public Character Character { get; set; }

        public PartyGroup Party { get; set; }
        public InstanceShopManager InstanceShopManager { get; }
        public InstanceGatheringItemManager InstanceGatheringItemManager { get; }
        public InstanceDropItemManager InstanceDropItemManager { get; }

        public GameMode GameMode { get; set; }

        public QuestStateManager QuestState { get
            {
                return ((PlayerPartyMember)Party?.GetPartyMemberByCharacter(Character))?.QuestState;
            } 
        }

        public bool IsPartyLeader()
        {
            return Party.Leader.Client == this;
        }

        // TODO: Place somewhere else more sensible
        public uint LastWarpPointId { get; set; }
        public DateTime LastWarpDateTime { get; set; }
    }
}
