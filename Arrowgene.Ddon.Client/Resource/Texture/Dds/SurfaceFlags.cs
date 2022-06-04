namespace Arrowgene.Ddon.Client.Resource.Texture.Dds;

public enum SurfaceFlags : uint
{
    TEXTURE = 0x00001000, // DDSCAPSTEXTURE
    MIPMAP = 0x00400008, // DDSCAPSCOMPLEX | DDSCAPSMIPMAP
    CUBEMAP = 0x00000008, // DDSCAPSCOMPLEX
}
