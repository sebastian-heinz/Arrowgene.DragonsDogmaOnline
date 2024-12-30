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
    public class S2CQuestGetPackageQuestListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_GET_PACKAGE_QUEST_LIST_RES;

        public S2CQuestGetPackageQuestListRes()
        {
            PackageQuestList = new List<CDataPackageQuestList>();
        }

        public uint Unk0 { get; set; }
        public List<CDataPackageQuestList> PackageQuestList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestGetPackageQuestListRes>
        {
            public override void Write(IBuffer buffer, S2CQuestGetPackageQuestListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.Unk0);
                WriteEntityList(buffer, obj.PackageQuestList);
            }

            public override S2CQuestGetPackageQuestListRes Read(IBuffer buffer)
            {
                S2CQuestGetPackageQuestListRes obj = new S2CQuestGetPackageQuestListRes();
                ReadServerResponse(buffer, obj);
                obj.Unk0 = ReadUInt32(buffer);
                obj.PackageQuestList = ReadEntityList<CDataPackageQuestList>(buffer);
                return obj;
            }
        }
    }
}
