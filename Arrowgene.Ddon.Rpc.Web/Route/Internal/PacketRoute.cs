using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.Rpc.Command;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Clan;
using Arrowgene.Ddon.Shared.Model.Rpc;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.WebServer;
using System;
using System.Threading.Tasks;
using static Arrowgene.Ddon.GameServer.RpcManager;

namespace Arrowgene.Ddon.Rpc.Web.Route.Internal
{
    public class PacketRoute : RpcRouteTemplate
    {
        public class InternalPacketCommand : RpcBodyCommand<RpcUnwrappedObject>
        {
            private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(InternalPacketCommand));

            public InternalPacketCommand(RpcUnwrappedObject entry) : base(entry)
            {
            }

            public override string Name => "InternalPacketCommand";

            public override RpcCommandResult Execute(DdonGameServer gameServer)
            {
                switch (_entry.Command)
                {
                    case RpcInternalCommand.AnnouncePacketAll:
                        {
                            RpcPacketData data = _entry.GetData<RpcPacketData>();
                            Packet packet = data.ToPacket();

                            foreach (var otherClient in gameServer.ClientLookup.GetAll())
                            {
                                otherClient.Send(packet);
                            }

                            return new RpcCommandResult(this, true)
                            {
                                Message = $"AnnouncePacketAll Ch.{_entry.Origin} -> {packet.Id}"
                            };
                        }
                    case RpcInternalCommand.AnnouncePacketClan:
                        {
                            RpcPacketData data = _entry.GetData<RpcPacketData>();
                            Packet packet = data.ToPacket();

                            Func<GameClient, bool> checkFunc = x => true;

                            if (packet.Id == PacketId.S2C_CLAN_CLAN_UPDATE_NTC)
                            {
                                gameServer.ClanManager.ResyncClan(data.ClanId);
                                // TODO: Fix the PlayerSummary/RpcPlayerData/Character objects when the clan tag changes.
                            }
                            else if (packet.Id == PacketId.S2C_CLAN_CLAN_JOIN_MEMBER_NTC)
                            {
                                var parsedPacket = new S2CClanClanJoinMemberNtc.Serializer().Read(data.Data);
                                uint characterId = parsedPacket.MemberInfo.CharacterListElement.CommunityCharacterBaseInfo.CharacterId;
                                Character characterLookup = gameServer.ClientLookup.GetClientByCharacterId(characterId)?.Character;
                                if (characterLookup != null)
                                {
                                    CDataClanParam clan = gameServer.ClanManager.GetClan(parsedPacket.ClanId);
                                    characterLookup.ClanId = parsedPacket.ClanId;
                                    characterLookup.ClanName.Name = clan.ClanUserParam.Name;
                                    characterLookup.ClanName.ShortName = clan.ClanUserParam.ShortName;
                                }
                                gameServer.RpcManager.UpdatePlayerSummaryClan(characterId, parsedPacket.ClanId);
                            }
                            else if (packet.Id == PacketId.S2C_CLAN_CLAN_POINT_ADD_NTC)
                            {
                                var parsedPacket = new S2CClanClanPointAddNtc.Serializer().Read(data.Data);
                                if (gameServer.ClanManager.HasClan(data.ClanId))
                                {
                                    var clan = gameServer.ClanManager.GetClan(data.ClanId);
                                    lock (clan)
                                    {
                                        clan.ClanServerParam.TotalClanPoint = parsedPacket.TotalClanPoint;
                                        clan.ClanServerParam.MoneyClanPoint = parsedPacket.MoneyClanPoint;
                                    }
                                }
                            }
                            else if (packet.Id == PacketId.S2C_CLAN_CLAN_LEVEL_UP_NTC)
                            {
                                var parsedPacket = new S2CClanClanLevelUpNtc.Serializer().Read(data.Data);
                                if (gameServer.ClanManager.HasClan(data.ClanId))
                                {
                                    var clan = gameServer.ClanManager.GetClan(data.ClanId);
                                    lock (clan)
                                    {
                                        clan.ClanServerParam.Lv = (ushort)parsedPacket.ClanLevel;
                                        clan.ClanServerParam.NextClanPoint = parsedPacket.NextClanPoint;
                                    }
                                }
                            }
                            else if (packet.Id == PacketId.S2C_CLAN_CLAN_BASE_RELEASE_STATE_UPDATE_NTC)
                            {
                                var parsedPacket = new S2CClanClanBaseReleaseStateUpdateNtc.Serializer().Read(data.Data);
                                if (gameServer.ClanManager.HasClan(data.ClanId))
                                {
                                    var clan = gameServer.ClanManager.GetClan(data.ClanId);
                                    lock (clan)
                                    {
                                        if (parsedPacket.State == 2)
                                        {
                                            clan.ClanServerParam.CanClanBaseRelease = true;
                                            var memberList = gameServer.ClanManager.LookupClientsByPermission(data.ClanId, ClanPermission.BaseRelease);
                                            checkFunc = x => memberList.Contains(x);
                                        }
                                        else if (parsedPacket.State == 3)
                                        {
                                            clan.ClanServerParam.CanClanBaseRelease = false;
                                            clan.ClanServerParam.IsClanBaseRelease = true;
                                        }
                                    }
                                }
                            }
                            else if (packet.Id == PacketId.S2C_CLAN_CLAN_SHOP_BUY_ITEM_NTC)
                            {
                                var parsedPacket = new S2CClanClanShopBuyItemNtc.Serializer().Read(data.Data);
                                if (gameServer.ClanManager.HasClan(data.ClanId))
                                {
                                    var clan = gameServer.ClanManager.GetClan(data.ClanId);
                                    lock (clan)
                                    {
                                        clan.ClanServerParam.MoneyClanPoint = parsedPacket.ClanPoint;
                                    }
                                }
                            }

                            foreach (var client in gameServer.ClientLookup.GetAll())
                            {
                                if (client.Character != null && client.Character.ClanId == data.ClanId)
                                {
                                    if (!checkFunc(client))
                                    {
                                        continue;
                                    }

                                    client.Send(packet);
                                }
                            }

                            // This has to occur after so that the player getting booted will actually get it.
                            if (packet.Id == PacketId.S2C_CLAN_CLAN_LEAVE_MEMBER_NTC)
                            {
                                var parsedPacket = new S2CClanClanLeaveMemberNtc.Serializer().Read(data.Data);
                                uint characterId = parsedPacket.CharacterListElement.CommunityCharacterBaseInfo.CharacterId;
                                Character characterLookup = gameServer.ClientLookup.GetClientByCharacterId(characterId)?.Character;
                                if (characterLookup != null)
                                {
                                    characterLookup.ClanId = 0;
                                    characterLookup.ClanName.Name = string.Empty;
                                    characterLookup.ClanName.ShortName = string.Empty;
                                }
                                gameServer.RpcManager.UpdatePlayerSummaryClan(characterId, 0);
                            }

                            return new RpcCommandResult(this, true)
                            {
                                Message = $"AnnouncePacketClan Ch.{_entry.Origin} ClanID {data.ClanId} -> {packet.Id}"
                            };
                        }
                    
                    default:
                        return new RpcCommandResult(this, false);
                }
            }
        }

        public PacketRoute(IRpcExecuter executer) : base(executer)
        {
        }

        public override string Route => "/rpc/internal/packet";

        public async override Task<WebResponse> Post(WebRequest request)
        {
            return await HandleBody<RpcUnwrappedObject, InternalPacketCommand>(request);
        }
    }
}
