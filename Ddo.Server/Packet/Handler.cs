using Arrowgene.Services.Logging;
using Ddo.Server.Database;
using Ddo.Server.Logging;
using Ddo.Server.Model;
using Ddo.Server.Setting;

namespace Ddo.Server.Packet
{
    public abstract class Handler : IHandler
    {
        protected Handler(DdoServer server)
        {
            Logger = LogProvider.Logger<DdoLogger>(this);
            Server = server;
            Router = server.Router;
            Database = server.Database;
            Settings = server.Setting;
            Clients = server.Clients;
        }

        public abstract ushort Id { get; }
        public virtual int ExpectedSize => QueueConsumer.NoExpectedSize;
        protected DdoServer Server { get; }
        protected DdoSetting Settings { get; }
        protected DdoLogger Logger { get; }
        protected PacketRouter Router { get; }
        protected ClientLookup Clients { get; }
        protected IDatabase Database { get; }
    }
}
