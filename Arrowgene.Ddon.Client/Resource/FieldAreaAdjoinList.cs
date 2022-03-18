using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Client.Resource;

public class FieldAreaAdjoinList : ResourceFile
{
    public class Vector3
    {
        public MtVector3 Pos { get; set; }
        public uint QuestId { get; set; }
        public uint FlagId { get; set; }
    }

    public class AdjoinInfo
    {
        public List<Vector3> Positions { get; set; }
        public short DestinationStageNo { get; set; }
        public short NextStageNo { get; set; }
        public byte Priority { get; set; }

        public AdjoinInfo()
        {
            Positions = new List<Vector3>();
        }
    }

    public List<AdjoinInfo> AdjoinInfos { get; }

    public FieldAreaAdjoinList()
    {
        AdjoinInfos = new List<AdjoinInfo>();
    }

    protected override void ReadResource(IBuffer buffer)
    {
        uint version = ReadUInt32(buffer);
        AdjoinInfos.Clear();
        uint unk = ReadUInt16(buffer);
        uint count = ReadUInt32(buffer);
        for (int i = 0; i < count; i++)
        {
            AdjoinInfo adjoinInfo = new AdjoinInfo();
            adjoinInfo.DestinationStageNo = ReadInt16(buffer);
            adjoinInfo.NextStageNo = ReadInt16(buffer);
            adjoinInfo.Positions = ReadMtArray(buffer, ReadVector3);
            adjoinInfo.Priority = ReadByte(buffer);
            AdjoinInfos.Add(adjoinInfo);
        }
    }

    private Vector3 ReadVector3(IBuffer buffer)
    {
        Vector3 vec = new Vector3();
        vec.Pos = ReadMtVector3(buffer);
        vec.QuestId = ReadUInt32(buffer);
        vec.FlagId = ReadUInt32(buffer);
        return vec;
    }
}
