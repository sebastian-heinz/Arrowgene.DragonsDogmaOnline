public class ReleasedWarpPoint
{
    public uint WarpPointId { get; set; }
    public uint FavoriteSlotNo { get; set; }
    public bool IsFavorite { get => FavoriteSlotNo != 0; }
}