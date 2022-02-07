using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetEnemySetListHandler : StructurePacketHandler<GameClient, C2SInstanceGetEnemySetListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetEnemySetListHandler));

        public InstanceGetEnemySetListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceGetEnemySetListReq> request)
        {   
            S2CInstanceGetEnemySetListRes response = new S2CInstanceGetEnemySetListRes();
            response.LayoutId.StageID = request.Structure.LayoutId.StageID;
            response.LayoutId.LayerNo = request.Structure.LayoutId.LayerNo;
            response.LayoutId.GroupID = request.Structure.LayoutId.GroupID;
            response.SubGroupId = request.Structure.SubGroupId;
            response.RandomSeed = 0xFFFFFFFF;

            CDataLayoutEnemyData layoutEnemyData = new CDataLayoutEnemyData();
            layoutEnemyData.EnemyInfo.EnemyId = 0x010314;
            layoutEnemyData.EnemyInfo.NamedEnemyParamsId = 0x8FA;
            layoutEnemyData.EnemyInfo.Scale = 100;
            layoutEnemyData.EnemyInfo.Lv = 94;
            layoutEnemyData.EnemyInfo.EnemyTargetTypesId = 1;
            response.EnemyList.Add(layoutEnemyData);

            Logger.Debug(client, Util.ToXML(response));

            client.Send(response);
        }
    }
}
