using System.Text;

namespace Arrowgene.Ddon.Shared.Model;

public class GmdIntermediateContainer
{
    public uint Index { get; set; }
    public string Key { get; set; }
    public string MsgOrg { get; set; }
    public string MsgEn { get; set; }
    public uint a2 { get; set; }
    public uint a3 { get; set; }
    public uint a4 { get; set; }
    public uint a5 { get; set; }
    public string GmdPath { get; set; }
    public string ArcName { get; set; }
    public string ArcPath { get; set; }
    public uint KeyReadIndex { get; set; }
    public uint MsgReadIndex { get; set; }
    public string Str { get; set; }

    public string GetUniqueQualifierLanguageAgnostic()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(Index);
        sb.Append(Key);
        //sb.Append(MsgOrg);
        sb.Append(a2);
        sb.Append(a3);
        sb.Append(a4);
        //sb.Append(a5);
        sb.Append(GmdPath);
        sb.Append(ArcPath);
        sb.Append(ArcName);
        sb.Append(KeyReadIndex);
        sb.Append(MsgReadIndex);
        sb.Append(Str);
        return sb.ToString();
    }
}
