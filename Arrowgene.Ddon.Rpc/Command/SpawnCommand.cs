using System;
using Arrowgene.Ddon.GameServer;

namespace Arrowgene.Ddon.Rpc.Command
{
    public class SpawnCommand : IRpcCommand
    {
        public string Name => "SpawnCommand";

        public SpawnCommand()
        {
        }

        public RpcCommandResult Execute(DdonGameServer gameServer)
        {
            throw new NotImplementedException();
        }
    }
}
