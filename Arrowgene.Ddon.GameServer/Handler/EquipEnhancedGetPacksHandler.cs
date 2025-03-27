using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.GameServer.Shop;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipEnhancedGetPacks : GameRequestPacketHandler<C2SEquipEnhancedGetPacksReq, S2CEquipEnhancedGetPacksRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipEnhancedGetPacks));
        
        public EquipEnhancedGetPacks(DdonGameServer server) : base(server)
        {
        }

        public override S2CEquipEnhancedGetPacksRes Handle(GameClient client, C2SEquipEnhancedGetPacksReq request)
        {
            S2CEquipEnhancedGetPacksRes res;
            if (request.Unk0 == 1)
            {
                var limitBreakAsset = Server.AssetRepository.LimitBreakAsset;
                res = new S2CEquipEnhancedGetPacksRes()
                {
                    ParamList = limitBreakAsset.ToLotteryExampleList()
                };
            }
            else
            {
                res = EntitySerializer.Get<S2CEquipEnhancedGetPacksRes>().Read(InGameDump.data_Dump_111);
            }
                
            return res;
        }
    }
}
