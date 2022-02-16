using Arrowgene.Ddon.Rpc.Command;

namespace Arrowgene.Ddon.Rpc
{
    public interface IRpcExecuter
    {
        RpcCommandResult Execute(IRpcCommand command);
    }
}
