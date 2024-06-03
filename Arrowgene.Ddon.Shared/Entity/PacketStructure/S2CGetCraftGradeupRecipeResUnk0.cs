using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CGetCraftGradeupRecipeResUnk0 : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CRAFT_RECIPE_GET_CRAFT_GRADEUP_RECIPE_RES;

        public uint Unk0 { get; set; }
        public uint Unk1 { get; set; }
        public uint Unk2 { get; set; }
        public uint Unk3 { get; set; }
        public uint Unk4 { get; set; }
        public uint Unk5 { get; set; }
        public bool Unk6 { get; set; }
        public CDataCraftMaterial Unk7 { get; set; }


        public S2CGetCraftGradeupRecipeResUnk0()
        {
            Unk7 = new CDataCraftMaterial();
        }

        public class Serializer : PacketEntitySerializer<S2CGetCraftGradeupRecipeResUnk0>
        {
            public override void Write(IBuffer buffer, S2CGetCraftGradeupRecipeResUnk0 obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.Unk2);
                WriteUInt32(buffer, obj.Unk3);
                WriteUInt32(buffer, obj.Unk4);
                WriteUInt32(buffer, obj.Unk5);
                WriteBool(buffer, obj.Unk6);
                WriteEntity<CDataCraftMaterial>(buffer, obj.Unk7);
            }

            public override S2CGetCraftGradeupRecipeResUnk0 Read(IBuffer buffer)
            {
                S2CGetCraftGradeupRecipeResUnk0 obj = new S2CGetCraftGradeupRecipeResUnk0();
                ReadServerResponse(buffer, obj);
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                obj.Unk3 = ReadUInt32(buffer);
                obj.Unk4 = ReadUInt32(buffer);
                obj.Unk5 = ReadUInt32(buffer);
                obj.Unk6 = ReadBool(buffer);
                obj.Unk7 = ReadEntity<CDataCraftMaterial>(buffer);
                return obj;
            }
        }
    }
}
