using Arrowgene.Buffers;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Client.Resource;

public class Texture : ResourceFile
{
    
    private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(Texture));
    
    protected override MagicIdWidth IdWidth => MagicIdWidth.UInt;

    protected override void ReadResource(IBuffer buffer)
    {
        uint a = ReadUInt32(buffer);
        uint b = ReadUInt32(buffer);
        uint c = ReadUInt32(buffer);
        
        uint t = MagicId >> 0x18;
        uint t1 = t & 0xF;

        uint a1 = a >> 0x6;
        uint a2 = a1 & 0x1FFF;
        uint mOrgWidth = a2 << (byte) t1;

        uint aa1 = a >> 0x13;
        uint mOrgHeight = aa1 << (byte) t1;
        
        uint aa2 = a >> 0x10;
        uint aa3 = aa2 & 0x1FFF;
        uint mOrgDepth = aa3 << (byte) t1;
        
        if ((MagicId & 0xF0000000) == 0x60000000 )
        {
            Logger.Error("TODO");
            // MtDataReader::read(&v250, &this->mSHFactor, 108u);
            byte[] data = ReadBytes(buffer, 0x6C);
        }
        
        byte b1 = (byte) (b & 0xFF);
        uint a11 = a & 0x3F;
        uint ab11 = b1 * a11;
        uint ab111 = ab11 << 0x2;

        uint mipLevelCount = CalcMipLevelCount(mOrgWidth);





        int i = 1;
    }

    private uint CalcMipLevelCount(uint size)
    {
        uint v1 = 0xFFFFFFFF;
        do
            ++v1;
        while (1 << (int) v1 <= size);
        return v1;
    }

    public byte[] ExportDds()
    {
        return new byte[0];
    }
}
