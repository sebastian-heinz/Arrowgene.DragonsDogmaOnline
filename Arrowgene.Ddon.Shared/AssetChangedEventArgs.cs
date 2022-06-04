using System;

namespace Arrowgene.Ddon.Shared;

public class AssetChangedEventArgs : EventArgs
{
    public AssetChangedEventArgs(string key, object asset)
    {
        Key = key;
        Asset = asset;
    }

    public string Key { get; }
    public object Asset { get; }
}
