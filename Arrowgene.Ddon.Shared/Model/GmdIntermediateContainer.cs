using System;

namespace Arrowgene.Ddon.Shared.Model;

public class GmdIntermediateContainer : IEquatable<GmdIntermediateContainer>
{
    public uint Index { get; set; }
    public string Key { get; set; }
    public string Msg { get; set; }
    public uint a2 { get; set; }
    public uint a3 { get; set; }
    public uint a4 { get; set; }
    public uint a5 { get; set; }
    public string Path { get; set; }
    public string Name { get; set; }
    public uint KeyReadIndex { get; set; }
    public uint MsgReadIndex { get; set; }
    public string Str { get; set; }

    public bool Equals(GmdIntermediateContainer other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Index == other.Index
               && string.Equals(Key, other.Key, StringComparison.OrdinalIgnoreCase)
              // && string.Equals(Msg, other.Msg, StringComparison.OrdinalIgnoreCase)
               && a2 == other.a2
               && a3 == other.a3
               && a4 == other.a4
            //   && a5 == other.a5
               && string.Equals(Path, other.Path, StringComparison.OrdinalIgnoreCase)
               && string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase)
               && KeyReadIndex == other.KeyReadIndex
               && MsgReadIndex == other.MsgReadIndex
               && string.Equals(Str, other.Str, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != typeof(GmdIntermediateContainer)) return false;
        return Equals((GmdIntermediateContainer)obj);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Index);
        hashCode.Add(Key, StringComparer.OrdinalIgnoreCase);
        //  hashCode.Add(Msg, StringComparer.OrdinalIgnoreCase);
        hashCode.Add(a2);
        hashCode.Add(a3);
        hashCode.Add(a4);
     //   hashCode.Add(a5);
        hashCode.Add(Path, StringComparer.OrdinalIgnoreCase);
        hashCode.Add(Name, StringComparer.OrdinalIgnoreCase);
        hashCode.Add(KeyReadIndex);
        hashCode.Add(MsgReadIndex);
        hashCode.Add(Str, StringComparer.OrdinalIgnoreCase);
        return hashCode.ToHashCode();
    }

    public static bool operator ==(GmdIntermediateContainer left, GmdIntermediateContainer right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(GmdIntermediateContainer left, GmdIntermediateContainer right)
    {
        return !Equals(left, right);
    }
}
