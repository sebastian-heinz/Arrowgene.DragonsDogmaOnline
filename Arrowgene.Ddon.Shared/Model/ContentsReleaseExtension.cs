using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    public static class ContentsReleaseExtension
    {
        public static CDataCharacterReleaseElement ToCDataCharacterReleaseElement(this ContentsRelease value)
        {
            return new CDataCharacterReleaseElement(value);
        }
    }
}
