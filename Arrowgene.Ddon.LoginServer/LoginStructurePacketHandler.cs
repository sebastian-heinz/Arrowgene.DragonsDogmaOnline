using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;

namespace Arrowgene.Ddon.LoginServer
{
    public abstract class LoginStructurePacketHandler<TReqStruct> : StructurePacketHandler<LoginClient, TReqStruct>
        where TReqStruct : class, IPacketStructure, new()
    {
        protected LoginStructurePacketHandler(DdonLoginServer server) : base(server)
        {
            Server = server;
        }

        protected new DdonLoginServer Server { get; }
    }
}
