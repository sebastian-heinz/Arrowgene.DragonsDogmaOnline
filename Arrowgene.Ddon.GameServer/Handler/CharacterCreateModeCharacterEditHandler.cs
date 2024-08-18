using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterCreateModeCharacterEditHandler : GameRequestPacketHandler<C2SCharacterCreateModeCharacterEditParamReq, S2CCharacterCreateModeCharacterEditParamRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterCreateModeCharacterEditHandler));

        public CharacterCreateModeCharacterEditHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCharacterCreateModeCharacterEditParamRes Handle(GameClient client, C2SCharacterCreateModeCharacterEditParamReq request)
        {
            return new S2CCharacterCreateModeCharacterEditParamRes();
        }
    }
}
