using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    public class BazaarExhibition
    {
        public uint CharacterId { get; set; }
        public CDataBazaarCharacterInfo Info { get; set; }
        
        public BazaarExhibition()
        {
            Info = new CDataBazaarCharacterInfo();
        }
    }

    public enum BazaarExhibitionState : byte
    {
        Idle = 0,
        Unk1 = 1,
        OnSale = 2,
        Sold = 3
    }
}