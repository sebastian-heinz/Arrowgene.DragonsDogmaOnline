using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SeasonDungeonReceiveSoulOrdealBuffHandler : GameRequestPacketHandler<C2SSeasonDungeonReceiveSoulOrdealRewardBuffReq, S2CSeasonDungeonReceiveSoulOrdealRewardBuffRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SeasonDungeonReceiveSoulOrdealBuffHandler));

        public SeasonDungeonReceiveSoulOrdealBuffHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSeasonDungeonReceiveSoulOrdealRewardBuffRes Handle(GameClient client, C2SSeasonDungeonReceiveSoulOrdealRewardBuffReq request)
        {
            var rewards = Server.EpitaphRoadManager.GetRewards(client, request.LayoutId.AsStageLayoutId(), request.PosId);

            if (request.BuffId == 0)
            {
                // Player choose to not pick a buff reward
                rewards.BuffRewards.Clear();
                return new S2CSeasonDungeonReceiveSoulOrdealRewardBuffRes();
            }

            var newBuff = rewards.BuffRewards.Where(x => x.BuffId == request.BuffId).First();

            Server.EpitaphRoadManager.AddPlayerBuff(client, client.Party, newBuff.BuffId, newBuff.Level);

            var partyBuffs = Server.EpitaphRoadManager.GetPartyBuffs(client.Party);
            
            var playerNtc = new S2CSeasonDungeonAreaBuffEffectNtc();
            foreach (var buff in Server.EpitaphRoadManager.GetPlayerBuffs(client, client.Party))
            {
                playerNtc.BuffEffectParamList.Add(buff.AsCDataSeasonDungeonBuffEffectParam());
            }
            client.Send(playerNtc);

            var partyNtc = new S2C_SEASON_62_39_16_NTC()
            {
                CharacterId = client.Character.CharacterId
            };

            foreach (var buff in Server.EpitaphRoadManager.GetPlayerBuffs(client, client.Party))
            {
                partyNtc.BuffList.Add(new CDataSeasonDungeonUnk0()
                {
                    BuffId = buff.BuffId,
                    Level = buff.Increment
                });
            }
            client.Party.SendToAll(partyNtc);

            // Remove any existing rewards for buffs incase the player cancels out of the receive items menu
            rewards.BuffRewards.Clear();

            return new S2CSeasonDungeonReceiveSoulOrdealRewardBuffRes();
        }

        private readonly byte[] pcap_data0 = { 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x1B, 0xE6, 0xB0, 0x97, 0xE7, 0xB5, 0xB6, 0xE6, 0x94, 0xBB, 0xE5, 0x8A, 0x9B, 0xE3, 0x82, 0xA2, 0xE3, 0x83, 0x83, 0xE3, 0x83, 0x97, 0x20, 0x4C, 0x76, 0x2E, 0x20, 0x31, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0xD1, 0x3F, 0x80, 0x00, 0x00, 0xBF, 0xB5, 0xDB, 0xBF, 0xBF, 0xB5 };
        private readonly byte[] pcap_data1 = { 0x00, 0x21, 0x55, 0x9B, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x01, 0x97, 0xE7, 0xB5, 0xB6, 0xE6, 0x94, 0xBB };
    }
}
