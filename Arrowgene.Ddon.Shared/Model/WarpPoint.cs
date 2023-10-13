public class WarpPoint
{
    public uint WarpPointId { get; set; }
    public uint Price { get; set; }

    public uint CalculateFinalPrice(bool isFavorite) 
    {
        return isFavorite ? 0 : this.Price;
    }
}