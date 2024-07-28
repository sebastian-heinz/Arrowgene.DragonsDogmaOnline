using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class StampBonusGetListHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StampBonusGetListHandler));

        public StampBonusGetListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_STAMP_BONUS_GET_LIST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            var res = new S2CStampBonusGetListRes();
            for (int i = 0; i < 8; i++)
            {
                res.StampBonusDaily.Add(new CDataStampBonusDaily
                {
                    StampNum = (ushort)(i + 1),
                    RecieveState = (byte)StampRecieveState.Claimed,
                    StampBonus = new List<CDataStampBonus> { 
                        new CDataStampBonus { BonusType = (uint)(i+7853), BonusValue = (uint)(i+1)}
                    }
                });
            }

            List<ushort> TotalStamps = new() { 15, 30, 60, 90, 120, 150, 180, 210, 240, 270 };
            for (int i = 0; i < 10; i++)
            {
                res.StampBonusTotal.Add(new CDataStampBonusTotal
                {
                    StampNum = TotalStamps[i],
                    RecieveState = (byte)StampRecieveState.Claimed,
                    StampBonus = new List<CDataStampBonus> {
                        new CDataStampBonus { BonusType = 5, BonusValue = (uint)(10*(i+1))}
                    }
                });
            }

            res.TotalStampNum = 420;

            //client.Send(GameFull.Dump_700);
            client.Send(res);
        }
    }
} 
