namespace Arrowgene.Ddon.Shared.Model
{
    public enum CraftType : byte
    {
        CraftTypeCreate = 1,
        CraftTypeUpgrade = 2, // Enhancement
        CraftTypeElement = 3,
        CraftTypeColor = 4,
        CraftTypeQuality = 5
        // TODO there might be more season 3-specific craft types via NPCs other than Sonia?
    }
}
