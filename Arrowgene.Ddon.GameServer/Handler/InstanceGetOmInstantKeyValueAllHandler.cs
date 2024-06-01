using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Instance;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetOmInstantKeyValueAllHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetOmInstantKeyValueAllHandler));

        public InstanceGetOmInstantKeyValueAllHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_INSTANCE_GET_OM_INSTANT_KEY_VALUE_ALL_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            // client.Send(GameFull.Dump_112);

            Dictionary<ulong, uint> omData = OmManager.GetAllOmData(client.Party.InstanceOmData, client.Character.Stage.Id);

            S2CInstanceGetOmInstantKeyValueAllRes res = new S2CInstanceGetOmInstantKeyValueAllRes()
            {
                StageId = client.Character.Stage.Id
            };

            foreach (var datum in omData)
            {
                res.Values.Add(new CDataOmData() { Key = datum.Key, Value = datum.Value});
            }

            client.Send(res);
        }
    }
}
