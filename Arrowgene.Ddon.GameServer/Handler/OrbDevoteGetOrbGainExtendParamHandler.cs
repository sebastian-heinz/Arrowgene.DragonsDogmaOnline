using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class OrbDevoteGetOrbGainExtendParamHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(OrbDevoteGetOrbGainExtendParamHandler));

        private OrbUnlockManager _OrbUnlockManager;

        public OrbDevoteGetOrbGainExtendParamHandler(DdonGameServer server) : base(server)
        {
            _OrbUnlockManager = server.OrbUnlockManager;
        }

        public override PacketId Id => PacketId.C2S_ORB_DEVOTE_GET_ORB_GAIN_EXTEND_PARAM_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            // client.Send(InGameDump.Dump_50);
            S2COrbDevoteGetOrbGainExtendParamRes Result = new S2COrbDevoteGetOrbGainExtendParamRes();
            Result.ExtendParam = client.Character.ExtendedParams;
            client.Send(Result);

            /*
            S2CItemUpdateCharacterItemNtc UpdateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            UpdateCharacterItemNtc.UpdateType = 0x2a;
            UpdateCharacterItemNtc.UpdateItemList = _OrbUnlockManager.GetDragonForceUpgradeUpdateList(client.Character);
            client.Send(UpdateCharacterItemNtc);
            */
        }
    }
}
