using Arrowgene.Ddon.GameServer.Instance;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceExchangeOmInstantKeyValueHandler : GameRequestPacketHandler<C2SInstanceExchangeOmInstantKeyValueReq, S2CInstanceExchangeOmInstantKeyValueRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceExchangeOmInstantKeyValueHandler));

        public InstanceExchangeOmInstantKeyValueHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CInstanceExchangeOmInstantKeyValueRes Handle(GameClient client, C2SInstanceExchangeOmInstantKeyValueReq request)
        {
            uint oldValue = OmManager.ExchangeOmData(client.Party.InstanceOmData, client.Character.Stage.Id, request.Key, request.Value);

            Logger.Debug($"OM: Key={request.Key}, Value={request.Value}, OldValue={oldValue}");

            S2CInstanceExchangeOmInstantKeyValueNtc ntc = new()
            {
                StageId = client.Character.Stage.Id,
                Key = request.Key,
                Value = request.Value,
                OldValue = oldValue
            };

            if (client.Party.ExmInProgress)
            {
                // TODO: Hacky workaround for possible issue regarding EXM progresses?
                client.Send(ntc);
            }
            else
            {
                client.Party.SendToAll(ntc);
            }

            S2CInstanceExchangeOmInstantKeyValueRes res = new()
            {
                StageId = client.Character.Stage.Id,
                Key = request.Key,
                Value = request.Value,
                OldValue = oldValue
            };

            return res;
        }
    }
}
