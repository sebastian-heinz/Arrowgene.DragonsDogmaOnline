using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity;
using System.Diagnostics;
using Arrowgene.Ddon.Shared.Entity.Structure;
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
            //S2CStampBonusGetListRes foo = EntitySerializer.Get<S2CStampBonusGetListRes>().Read(GameFull.data_Dump_700);
            //Logger.Debug($"TotalStampNum : {foo.TotalStampNum}");
            //foreach (CDataStampBonusDaily item in foo.StampBonusDaily)
            //{
            //    Logger.Debug($"\tDaily: {item.StampNum} | {item.RecieveState}");
            //    foreach (CDataStampBonus inner in item.StampBonus)
            //    {
            //        Logger.Debug($"\t\t {inner.BonusType} | {inner.BonusType}");
            //    }
            //}
            //foreach (CDataStampBonusTotal item in foo.StampBonusTotal)
            //{
            //    Logger.Debug($"\tTotal: {item.StampNum} | {item.RecieveState}");
            //    foreach (CDataStampBonus inner in item.StampBonus)
            //    {
            //        Logger.Debug($"\t\t {inner.BonusType} | {inner.BonusType}");
            //    }
            //}


            //var res = new S2CStampBonusGetListRes();
            //res.StampBonusTotal.Add(new CDataStampBonusTotal()
            //{
            //    StampNum = 1,
            //    RecieveState = 1,
            //    StampBonus = new List<CDataStampBonus>
            //    {
            //        new CDataStampBonus() 
            //        {
            //            BonusType = 1,
            //            BonusValue = 10
            //        }
            //    }
            //});
            //res.TotalStampNum = 1;
            //client.Send(res);

            var res = new S2CStampBonusGetListRes();
            res.TotalStampNum = 0;
            for (int i = 0; i < 8; i++)
            {
                res.StampBonusDaily.Add(new CDataStampBonusDaily
                {
                    StampNum = (ushort)(i + 1),
                    RecieveState = 2,
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
                    RecieveState = 3,
                    StampBonus = new List<CDataStampBonus> {
                        new CDataStampBonus { BonusType = 5, BonusValue = (uint)(10*(i+1))}
                    }
                });
            }

            //var resbytes = new S2CStampBonusGetListRes.Serializer().Write(res);
            //Packet resPacket = new Packet(res.Id, resbytes);
            //Packet resReal = GameFull.Dump_700;

            //Logger.Info(resReal.ToString());

            //Logger.Info("\n\n\n");

            //Logger.Info(resPacket.ToString());

            //client.Send(resReal);
            client.Send(res);
        }
    }
} 
