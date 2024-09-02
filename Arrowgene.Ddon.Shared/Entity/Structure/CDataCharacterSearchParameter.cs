using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCharacterSearchParameter
    {
        public CDataCharacterSearchParameter()
        {
        }

        public uint Job {  get; set; }
        public uint VocationMin {  get; set; }
        public uint VocationMax { get; set; }
        public byte LevelMin {  get; set; }
        public byte LevelMax { get; set; }
        public ushort ItemRankMin { get; set; }
        public ushort ItemRankMax { get; set; }

        public class Serializer : EntitySerializer<CDataCharacterSearchParameter>
        {
            public override void Write(IBuffer buffer, CDataCharacterSearchParameter obj)
            {
                WriteUInt32(buffer, obj.Job);
                WriteUInt32(buffer, obj.VocationMin);
                WriteUInt32(buffer, obj.VocationMax);
                WriteByte(buffer, obj.LevelMin);
                WriteByte(buffer, obj.LevelMax);
                WriteUInt16(buffer, obj.ItemRankMin);
                WriteUInt16(buffer, obj.ItemRankMax);
            }

            public override CDataCharacterSearchParameter Read(IBuffer buffer)
            {
                CDataCharacterSearchParameter obj = new CDataCharacterSearchParameter();
                obj.Job = ReadUInt32(buffer);
                obj.VocationMin = ReadUInt32(buffer);
                obj.VocationMax = ReadUInt32(buffer);
                obj.LevelMin = ReadByte(buffer);
                obj.LevelMax = ReadByte(buffer);
                obj.ItemRankMin = ReadUInt16(buffer);
                obj.ItemRankMax = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
