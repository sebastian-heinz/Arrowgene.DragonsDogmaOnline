namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public class QuestProcessState
    {
        public ushort ProcessNo { get; set; }
        public ushort SequenceNo { get; set; }
        public ushort BlockNo { get; set; }

        public override string ToString()
        {
            return $"{ProcessNo}.{SequenceNo}.{BlockNo}";
        }
    }
}
