using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemEmbodyPayCostHandler : GameRequestPacketHandler<C2SItemGetEmbodyPayCostReq, S2CItemGetEmbodyPayCostRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemEmbodyPayCostHandler));

        public ItemEmbodyPayCostHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CItemGetEmbodyPayCostRes Handle(GameClient client, C2SItemGetEmbodyPayCostReq request)
        {
            var result = new S2CItemGetEmbodyPayCostRes();

            result.EmbodyCostList.Add(new CDataItemEmbodyCostParam()
            {
                WalletType = WalletType.GoldenDragonMark,
                WalletPoints = new List<CDataWalletPoint>()
                {
                    new CDataWalletPoint()
                    {
                        Type = WalletType.GoldenDragonMark,
                        Value = 3
                    }
                },
            });

            result.EmbodyCostList.Add(new CDataItemEmbodyCostParam()
            {
                WalletType = WalletType.GoldenGemstones,
                WalletPoints = new List<CDataWalletPoint>()
                {
                    new CDataWalletPoint()
                    {
                        Type = WalletType.GoldenGemstones,
                        Value = 3
                    }
                },
            });

            return result;
        }
    }
}
