using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BinarySaveSetCharacterBinSavedataHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BinarySaveSetCharacterBinSavedataHandler));


        public BinarySaveSetCharacterBinSavedataHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_BINARY_SAVE_SET_CHARACTER_BIN_SAVEDATA_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            client.Send(SelectedDump.AntiDc9_1_2);
        }
    }
}
