using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;

namespace Arrowgene.Ddon.LoginServer
{
    public abstract class LoginRequestPacketQueueHandler<TReqStruct, TResStruct> : RequestPacketQueueHandler<LoginClient, TReqStruct, TResStruct>
        where TReqStruct : class, IPacketStructure, new()
        where TResStruct : ServerResponse, new()
    {
        protected LoginRequestPacketQueueHandler(DdonLoginServer server) : base(server)
        {
            Server = server;
        }

        protected new DdonLoginServer Server { get; }
    }
}
