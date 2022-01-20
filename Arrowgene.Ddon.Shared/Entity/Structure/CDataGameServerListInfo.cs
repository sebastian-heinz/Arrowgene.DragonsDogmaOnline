using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGameServerListInfo
    {
        public CDataGameServerListInfo(ushort id, string name, string brief, string trafficName, uint trafficLevel, uint maxLoginNum, uint loginNum, string addr, ushort port, bool isHide)
        {
            ID = id;
            Name = name;
            Brief = brief;
            TrafficName = trafficName;
            TrafficLevel = trafficLevel;
            MaxLoginNum = maxLoginNum;
            LoginNum = loginNum;
            Addr = addr;
            Port = port;
            IsHide = isHide;
        }

        public CDataGameServerListInfo()
        {
            ID = 0;
            Name = "";
            Brief = "";
            TrafficName = "";
            TrafficLevel = 0;
            MaxLoginNum = 0;
            LoginNum = 0;
            Addr = "";
            Port = 0;
            IsHide = false;
        }

        public ushort ID;
        public string Name;
        public string Brief;
        public string TrafficName;
        public uint TrafficLevel;
        public uint MaxLoginNum;
        public uint LoginNum;
        public string Addr;
        public ushort Port;
        public bool IsHide;
    }

    public class CDataGameServerListInfoSerializer : EntitySerializer<CDataGameServerListInfo>
    {
        public override void Write(IBuffer buffer, CDataGameServerListInfo obj)
        {
            WriteUInt16(buffer, obj.ID);
            WriteMtString(buffer, obj.Name);
            WriteMtString(buffer, obj.Brief);
            WriteMtString(buffer, obj.TrafficName);
            WriteUInt32(buffer, obj.TrafficLevel);
            WriteUInt32(buffer, obj.MaxLoginNum);
            WriteUInt32(buffer, obj.LoginNum);
            WriteMtString(buffer, obj.Addr);
            WriteUInt16(buffer, obj.Port);
            WriteBool(buffer, obj.IsHide);
        }

        public override CDataGameServerListInfo Read(IBuffer buffer)
        {
            CDataGameServerListInfo obj = new CDataGameServerListInfo();
            obj.ID = ReadUInt16(buffer);
            obj.Name = ReadMtString(buffer);
            obj.Brief = ReadMtString(buffer);
            obj.TrafficName = ReadMtString(buffer);
            obj.TrafficLevel = ReadUInt32(buffer);
            obj.MaxLoginNum = ReadUInt32(buffer);
            obj.LoginNum = ReadUInt32(buffer);
            obj.Addr = ReadMtString(buffer);
            obj.Port = ReadUInt16(buffer);
            obj.IsHide = ReadBool(buffer);
            return obj;
        }
    }
}
