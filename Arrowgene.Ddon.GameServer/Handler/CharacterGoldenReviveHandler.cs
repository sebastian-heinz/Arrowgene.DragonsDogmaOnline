using Arrowgene.Buffers;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterGoldenReviveHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterGoldenReviveHandler));

        public CharacterGoldenReviveHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_CHARACTER_CHARACTER_GOLDEN_REVIVE_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            // Read request
            C2SCharacterGoldenReviveReq req = EntitySerializer.Get<C2SCharacterGoldenReviveReq>().Read(packet.AsBuffer());
            
            // Send response
            IBuffer resBuffer = new StreamBuffer();
            resBuffer.WriteInt32(0, Endianness.Big); // error
            resBuffer.WriteInt32(0, Endianness.Big); // result

            S2CCharacterGoldenReviveRes res = new S2CCharacterGoldenReviveRes();
            res.gp = 69;

            EntitySerializer.Get<S2CCharacterGoldenReviveRes>().Write(resBuffer, res);
            Packet resPacket = new Packet(PacketId.S2C_CHARACTER_CHARACTER_GOLDEN_REVIVE_RES, resBuffer.GetAllBytes());
            client.Send(resPacket);
        }
    }
}