namespace Arrowgene.Ddon.Rpc.Command
{
    public class RpcCommandResult
    {
        public RpcCommandResult(IRpcCommand command, bool success)
        {
            Command = command;
            Success = success;
            Message = string.Empty;
        }

        public IRpcCommand Command { get; }
        public string Message { get; set; }
        public bool Success { get; set; }

        public override string ToString()
        {
            return $"Cmd:{Command.Name} OK:{Success} Msg:{Message}";
        }
    }
}
