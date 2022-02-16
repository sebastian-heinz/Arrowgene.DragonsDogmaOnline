using Arrowgene.Ddon.GameServer;

namespace Arrowgene.Ddon.Rpc.Command
{
    public interface IRpcCommand
    {
        RpcCommandResult Execute(DdonGameServer gameServer);
        string Name { get; }
    }
}
