using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class AreaGetSpotInfoListHandler : GameRequestPacketHandler<C2SAreaGetSpotInfoListReq, S2CAreaGetSpotInfoListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(AreaGetSpotInfoListHandler));

        private static readonly byte[] PcapData = new byte[] { 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x10,0x00,0x00,0x03,0x74,0x00,0x00,0x00,0x00,0x00,0x00,0x01,0x75,0x01,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x03,0x6F,0x00,0x00,0x00,0x01,0x00,0x00,0x01,0x75,0x01,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x03,0xBF,0x00,0x00,0x00,0x02,0x00,0x00,0x01,0x90,0x01,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x03,0xB1,0x00,0x00,0x00,0x03,0x00,0x00,0x01,0x75,0x01,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x03,0xB2,0x00,0x00,0x00,0x04,0x00,0x00,0x01,0x75,0x01,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x03,0xB3,0x00,0x00,0x00,0x05,0x00,0x00,0x01,0x75,0x01,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x03,0xB4,0x00,0x00,0x00,0x06,0x00,0x00,0x01,0x75,0x01,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x03,0xB5,0x00,0x00,0x00,0x07,0x00,0x00,0x01,0x75,0x01,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x03,0xB6,0x00,0x00,0x00,0x08,0x00,0x00,0x01,0x75,0x01,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x03,0xB9,0x00,0x00,0x00,0x09,0x00,0x00,0x01,0x75,0x01,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x03,0x8F,0x00,0x00,0x00,0x0A,0x00,0x00,0x01,0x70,0x01,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x03,0x91,0x00,0x00,0x00,0x0B,0x00,0x00,0x01,0xAD,0x01,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x03,0x90,0x00,0x00,0x00,0x0C,0x00,0x00,0x01,0xC7,0x01,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x03,0x92,0x00,0x00,0x00,0x0D,0x00,0x00,0x01,0x71,0x01,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x03,0x93,0x00,0x00,0x00,0x0E,0x00,0x00,0x01,0xA6,0x01,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x03,0x94,0x00,0x00,0x00,0x0F,0x00,0x00,0x01,0xC0,0x01,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x04,0x00,0x00,0x04,0x2C,0x01,0x00,0x00,0x00,0x00,0x5D,0xC5,0xC9,0x40,0x00,0x00,0x04,0x2E,0x01,0x00,0x00,0x00,0x00,0x5D,0xC5,0xC9,0x40,0x00,0x00,0x04,0x34,0x01,0x00,0x00,0x00,0x00,0x5D,0xC5,0xC9,0x40,0x00,0x00,0x04,0x38,0x01,0x00,0x00,0x00,0x00,0x5D,0xC5,0xC9,0x40,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x02 };

        public AreaGetSpotInfoListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CAreaGetSpotInfoListRes Handle(GameClient client, C2SAreaGetSpotInfoListReq request)
        {
            // TODO: This still uses the List<CDataAreaRankSeason3> Unk1 from the Pcap, the client complains if its not present.
            var pcap = EntitySerializer.Get<S2CAreaGetSpotInfoListRes>().Read(PcapData);
            pcap.SpotInfoList = new();

            var clientRank = client.Character.AreaRanks.GetValueOrDefault(request.AreaId)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_AREAMASTER_AREA_INFO_NOT_FOUND);
            var completedQuests = client.Character.CompletedQuests;

            foreach (var spot in Server.AssetRepository.AreaRankSpotInfoAsset[request.AreaId].Where(x => !x.ReleaseOnly))
            {
                pcap.SpotInfoList.Add(new()
                {
                    SpotId = spot.SpotId,
                    TextIndex = spot.TextIndex,
                    StageId = 2, // TODO: Figure out if the client actually cares
                    IsRelease = Server.AreaRankManager.CheckSpot(client, spot)
                });
            }

            return pcap;
        }
    }
}
