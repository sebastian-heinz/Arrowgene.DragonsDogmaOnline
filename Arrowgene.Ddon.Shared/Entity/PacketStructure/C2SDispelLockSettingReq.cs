using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SDispelLockSettingReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_DISPEL_LOCK_SETTING_REQ;

        /// <summary>
        /// If true, the player is adding additional seals.
        /// If false, the player is resetting the seals.
        /// </summary>
        public bool IsNormalSeal { get; set; }
        public uint Unk1 { get; set; }
        public List<CDataDispelLockSettingUpdate> SettingUpdates { get; set; } = [];

        public class Serializer : PacketEntitySerializer<C2SDispelLockSettingReq>
        {
            public override void Write(IBuffer buffer, C2SDispelLockSettingReq obj)
            {
                WriteBool(buffer, obj.IsNormalSeal);
                WriteUInt32(buffer, obj.Unk1);
                WriteEntityList(buffer, obj.SettingUpdates);
            }

            public override C2SDispelLockSettingReq Read(IBuffer buffer)
            {
                C2SDispelLockSettingReq obj = new C2SDispelLockSettingReq();
                obj.IsNormalSeal = ReadBool(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.SettingUpdates = ReadEntityList<CDataDispelLockSettingUpdate>(buffer);
                return obj;
            }
        }
    }
}
