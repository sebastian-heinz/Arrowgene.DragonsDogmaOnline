using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SGetCraftGradeupRecipeReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CRAFT_RECIPE_GET_CRAFT_GRADEUP_RECIPE_REQ;

        public byte Category { get; set; }
        public uint Offset { get; set; }
        public int Num { get; set; }
        public List<CDataCommonU32> ItemList { get; set; }

        public C2SGetCraftGradeupRecipeReq()
        {
            ItemList = new List<CDataCommonU32>();
        }

        public class Serializer : PacketEntitySerializer<C2SGetCraftGradeupRecipeReq>
        {
            public override void Write(IBuffer buffer, C2SGetCraftGradeupRecipeReq obj)
            {
                WriteByte(buffer, obj.Category);
                WriteUInt32(buffer, obj.Offset);
                WriteInt32(buffer, obj.Num);
                WriteInt32(buffer, obj.ItemList.Count);
                foreach (var item in obj.ItemList)
                {
                    new CDataCommonU32.Serializer().Write(buffer, item);
                }
            }

            public override C2SGetCraftGradeupRecipeReq Read(IBuffer buffer)
            {
                C2SGetCraftGradeupRecipeReq obj = new C2SGetCraftGradeupRecipeReq();
                obj.Category = ReadByte(buffer);
                obj.Offset = ReadUInt32(buffer);
                obj.Num = ReadInt32(buffer);
                obj.ItemList = new List<CDataCommonU32>();
                return obj;
            }
        }
    }
}
