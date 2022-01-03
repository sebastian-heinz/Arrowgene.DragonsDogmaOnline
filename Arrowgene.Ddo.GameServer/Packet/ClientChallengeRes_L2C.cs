using Arrowgene.Ddo.GameServer.Network;

namespace Arrowgene.Ddo.GameServer.Paket
{
    public class ClientChallengeRes_C2L : Packet
    {
        public ClientChallengeRes_C2L() : base(PacketId.L2C_CLIENT_CHALLENGE_RES)
        {
        }
    }
}
