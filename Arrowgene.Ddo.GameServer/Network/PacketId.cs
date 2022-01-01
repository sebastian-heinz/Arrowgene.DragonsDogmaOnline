namespace Arrowgene.Ddo.GameServer.Network
{
    public enum PacketId : ushort
    {    
        Unknown = 0,
        ClientNetworkKey = 1,
        ClientChallengeReq_C2L = 2,
        ClientChallengeRes_C2L = 3,
    }
}
