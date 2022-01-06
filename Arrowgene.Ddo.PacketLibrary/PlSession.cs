using System.Collections.Generic;

namespace Arrowgene.Ddo.PacketLibrary
{
    public class PlSession
    {
        private List<PlPacket> _session;

        public bool? IsLogin { get; set; }

        public ushort ClientPort { get; set; }
        
        public PlSession()
        {
            _session = new List<PlPacket>();
            IsLogin = null;
        }

        public void Add(PlPacket plPacket)
        {
            _session.Add(plPacket);
        }
    }
}
