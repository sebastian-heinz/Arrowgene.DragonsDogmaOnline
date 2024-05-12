using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.GameServer.Characters;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceSetOmInstantKeyValueHandler : StructurePacketHandler<GameClient, C2SInstanceSetOmInstantKeyValueReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceSetOmInstantKeyValueHandler));

        private CharacterManager _CharacterManager;

        public InstanceSetOmInstantKeyValueHandler(DdonGameServer server) : base(server)
        {
            _CharacterManager = server.CharacterManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceSetOmInstantKeyValueReq> req)
        {
            _CharacterManager.SetOmData(client.Character, client.Character.Stage.Id, req.Structure.Key, req.Structure.Value);

            S2CInstanceSetOmInstantKeyValueNtc ntc = new S2CInstanceSetOmInstantKeyValueNtc();
            ntc.StageId = client.Character.Stage.Id;
            ntc.Key = req.Structure.Key;
            ntc.Value = req.Structure.Value;
            client.Send(ntc); // Should this be sendtoall?

            S2CInstanceSetOmInstantKeyValueRes res = new S2CInstanceSetOmInstantKeyValueRes();
            res.StageId = client.Character.Stage.Id;
            res.Key = req.Structure.Key;
            res.Value = req.Structure.Value;
            client.Send(res);
        }
    }
}
