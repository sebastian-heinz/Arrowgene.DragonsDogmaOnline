using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InnStayPenaltyHealInn : GameStructurePacketHandler<C2SInnStayPenaltyHealInnReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InnStayPenaltyHealInn));
        
        public InnStayPenaltyHealInn(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInnStayPenaltyHealInnReq> packet)
        {
            WalletType priceWalletType = InnGetPenaltyHealStayPrice.PointType;
            uint price = InnGetPenaltyHealStayPrice.Point;

            // Update character wallet
            CDataWalletPoint wallet = client.Character.WalletPointList.Where(wp => wp.Type == priceWalletType).Single();
            wallet.Value = (uint) Math.Max(0, (int)wallet.Value - (int)price);
            Database.UpdateWalletPoint(client.Character.CharacterId, wallet);

            client.Send(new S2CInnStayPenaltyHealInnRes()
            {
                PointType = wallet.Type,
                Point = wallet.Value
            });
        }
    }
}