using Arrowgene.Ddon.Client.Resource.Texture.Dds;
using Arrowgene.Ddon.Client.Resource.Texture.Tex;

namespace Arrowgene.Ddon.Client.Resource.Texture;

public static class TexConvert
{
    public static DdsTexture ToDdsTexture(TexTexture texTexture)
    {
        DdsTexture ddsTexture = new DdsTexture();
        return ddsTexture;
    }    
    
    public static TexTexture ToTexTexture(DdsTexture ddsTexture)
    {
        TexTexture texTexture = new TexTexture();
        texTexture.Header.Version = TexTexture.DddaTexHeaderVersion;
        texTexture.Header.Shift = 0;
        texTexture.Header.Alpha = 2;
        texTexture.Header.UnknownA = 0;
        texTexture.Header.UnknownB = 0;
        texTexture.Header.Depth = 1;
        return texTexture;
    }
}
