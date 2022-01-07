using System;
using System.Collections.Generic;

namespace Arrowgene.Ddo.PacketLibrary
{
    public class PlSession
    {
        private List<PlPacket> _session;
        public DateTime StartTime { get; set; }
        public bool IsLogin { get; set; }
        public string ServerIP { get; set; }
        public bool IsEncrypted { get; set; }
        public string Key { get; set; }
        public string ServerType => IsLogin ? "login" : "game";
        
        public PlSession()
        {
            _session = new List<PlPacket>();
        }
        
        public List<PlPacket> GetPackets()
        {
            return new List<PlPacket>(_session);
        }

        public void Add(PlPacket plPacket)
        {
            _session.Add(plPacket);
        }
    }
}
