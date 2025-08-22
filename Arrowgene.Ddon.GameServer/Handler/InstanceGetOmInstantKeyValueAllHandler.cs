using Arrowgene.Ddon.GameServer.Instance;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetOmInstantKeyValueAllHandler : GameRequestPacketHandler<C2SInstanceGetOmInstantKeyValueAllReq, S2CInstanceGetOmInstantKeyValueAllRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetOmInstantKeyValueAllHandler));

        public InstanceGetOmInstantKeyValueAllHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CInstanceGetOmInstantKeyValueAllRes Handle(GameClient client, C2SInstanceGetOmInstantKeyValueAllReq request)
        {
            Dictionary<ulong, uint> omData = OmManager.GetAllOmData(client.Party.InstanceOmData, client.Character.Stage.Id);

            S2CInstanceGetOmInstantKeyValueAllRes res = new()
            {
                StageId = client.Character.Stage.Id,
                Values = [.. omData.Select(x => new CDataOmData()
                {
                    Key = x.Key,
                    Value = x.Value
                })]
            };

            return res;
        }
    }
}
