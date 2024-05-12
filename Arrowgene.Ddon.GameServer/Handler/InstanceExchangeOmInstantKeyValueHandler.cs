using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.GameServer.Characters;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceExchangeOmInstantKeyValueHandler : StructurePacketHandler<GameClient, C2SInstanceExchangeOmInstantKeyValueReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceExchangeOmInstantKeyValueHandler));
        private readonly CharacterManager _CharacterManager;

        public InstanceExchangeOmInstantKeyValueHandler(DdonGameServer server) : base(server)
        {
            _CharacterManager = server.CharacterManager;
        }


        public override void Handle(GameClient client, StructurePacket<C2SInstanceExchangeOmInstantKeyValueReq> req)
        {
            uint oldValue = _CharacterManager.ExchangeOmData(client.Character, client.Character.Stage.Id, req.Structure.Key, req.Structure.Value);

            S2CInstanceExchangeOmInstantKeyValueNtc ntc = new S2CInstanceExchangeOmInstantKeyValueNtc();
            ntc.StageId = client.Character.Stage.Id;
            ntc.Key = req.Structure.Key;
            ntc.Value = req.Structure.Value;
            ntc.OldValue = oldValue;
            client.Send(ntc);

            S2CInstanceExchangeOmInstantKeyValueRes res = new S2CInstanceExchangeOmInstantKeyValueRes();
            res.StageId = client.Character.Stage.Id;
            res.Key = req.Structure.Key;
            res.Value = req.Structure.Value;
            res.OldValue = oldValue;
            client.Send(res);
        }
    }
}
