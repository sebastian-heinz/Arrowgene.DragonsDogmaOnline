using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BinarySaveSetCharacterBinSavedataHandler : GameRequestPacketHandler<C2SBinarySaveSetCharacterBinSaveDataReq, S2CBinarySaveSetCharacterBinSaveDataRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BinarySaveSetCharacterBinSavedataHandler));

        public BinarySaveSetCharacterBinSavedataHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBinarySaveSetCharacterBinSaveDataRes Handle(GameClient client, C2SBinarySaveSetCharacterBinSaveDataReq request)
        {
            Logger.Debug($"Storing binary data for character {client.Character.CharacterId}");
            if (Server.TemporaryBinaryDataStorage.ContainsKey(client.Character.CharacterId))
            {
                Server.TemporaryBinaryDataStorage[client.Character.CharacterId] = request.BinaryData;
            }
            else
            {
                Server.TemporaryBinaryDataStorage.Add(client.Character.CharacterId, request.BinaryData);
            }

            return new S2CBinarySaveSetCharacterBinSaveDataRes();
        }
    }
}
