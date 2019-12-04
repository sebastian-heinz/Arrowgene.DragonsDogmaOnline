using System;
using System.Diagnostics;
using Arrowgene.Services.Logging;
using Ddo.Server.Logging;
using Ddo.Server.Packet;

namespace Ddo.Server.Model
{
    [DebuggerDisplay("{Identity,nq}")]
    public class DdoClient : ISender
    {
        private readonly DdoLogger _logger;

        public DdoClient()
        {
            _logger = LogProvider.Logger<DdoLogger>(this);
            Creation = DateTime.Now;
            Identity = "";
        }

        public DateTime Creation { get; }
        public string Identity { get; private set; }
        public Account Account { get; set; }
        public DdoConnection Connection { get; set; }


        public void Send(DdoPacket packet)
        {
            Connection.Send(packet);
        }

        public void UpdateIdentity()
        {
            Identity = "";
            if (Account != null)
            {
                Identity += $"[Acc:{Account.Id}:{Account.Name}]";
                return;
            }

            if (Connection != null)
            {
                Identity += $"[Con:{Connection.Identity}]";
            }
        }

        public void Close()
        {
            Connection?.Socket.Close();
        }
    }
}
