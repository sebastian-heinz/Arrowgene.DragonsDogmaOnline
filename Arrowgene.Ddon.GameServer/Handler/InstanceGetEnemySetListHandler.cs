using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetEnemySetListHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetEnemySetListHandler));


        public InstanceGetEnemySetListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_INSTANCE_GET_ENEMY_SET_LIST_REQ;

        public override void Handle(GameClient client, Packet packet)
        {   
            //Read in the detailsdw
            C2SInstanceGetEnemySetListRes req = EntitySerializer.Get<C2SInstanceGetEnemySetListRes>().Read(packet.AsBuffer());

            //Write in the packet stuff
            IBuffer resBuffer = new StreamBuffer();
            resBuffer.WriteInt16(0, Endianness.Big); // error
            resBuffer.WriteInt32(0, Endianness.Big); // result
            resBuffer.WriteInt32(0, Endianness.Big); // ??? (padding?)

            C2SInstanceGetEnemySetListRes res = new C2SInstanceGetEnemySetListRes(); //note that the enemy array is inside this and is currently hardcoded
            res.groupId = req.groupId;
            res.layerNo = req.layerNo;
            res.stageId = req.stageId;
            res.subgroupId = req.subgroupId;

            EntitySerializer.Get<C2SInstanceGetEnemySetListRes>().Write(resBuffer, res);
            Packet resPacket = new Packet(PacketId.S2C_INSTANCE_GET_ENEMY_SET_LIST_RES, resBuffer.GetAllBytes());
                
            client.Send(resPacket);
        }
    }
}
