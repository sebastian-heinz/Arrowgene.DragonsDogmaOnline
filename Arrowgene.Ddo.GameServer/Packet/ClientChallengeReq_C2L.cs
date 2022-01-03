using Arrowgene.Ddo.GameServer.Network;

namespace Arrowgene.Ddo.GameServer.Paket
{
    public class ClientChallengeReq_C2L : Packet
    {
        public ClientChallengeReq_C2L() : base(PacketId.C2L_CLIENT_CHALLENGE_REQ)
        {
        }
    }
}
