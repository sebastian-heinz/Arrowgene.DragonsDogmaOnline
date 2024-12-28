using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataLoginSetting
    {
        public CDataLoginSetting()
        {
            JobLevelMax = 0;
            ClanMemberMax = 0;
            EnableVisualEquip = false;
            FriendListMax = 0;
            URLInfoList = new List<CDataURLInfo>();
            NoOperationTimeOutTime = 0;
        }

        public uint JobLevelMax;
        public uint ClanMemberMax;
        public byte CharacterNumMax;
        public bool EnableVisualEquip;
        public uint FriendListMax;
        public List<CDataURLInfo> URLInfoList;
        public uint NoOperationTimeOutTime;

        public class Serializer : EntitySerializer<CDataLoginSetting>
        {
            public override void Write(IBuffer buffer, CDataLoginSetting obj)
            {
                WriteUInt32(buffer, obj.JobLevelMax);
                WriteUInt32(buffer, obj.ClanMemberMax);
                WriteByte(buffer, obj.CharacterNumMax);
                WriteBool(buffer, obj.EnableVisualEquip);
                WriteUInt32(buffer, obj.FriendListMax);
                WriteEntityList(buffer, obj.URLInfoList);
                WriteUInt32(buffer, obj.NoOperationTimeOutTime);
            }

            public override CDataLoginSetting Read(IBuffer buffer)
            {
                CDataLoginSetting obj = new CDataLoginSetting();
                obj.JobLevelMax = ReadUInt32(buffer);
                obj.ClanMemberMax = ReadUInt32(buffer);
                obj.CharacterNumMax = ReadByte(buffer);
                obj.EnableVisualEquip = ReadBool(buffer);
                obj.FriendListMax = ReadUInt32(buffer);
                obj.URLInfoList = ReadEntityList<CDataURLInfo>(buffer);
                obj.NoOperationTimeOutTime = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
