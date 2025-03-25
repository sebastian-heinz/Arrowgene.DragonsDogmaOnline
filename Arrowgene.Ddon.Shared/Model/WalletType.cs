namespace Arrowgene.Ddon.Shared.Model
{
    /**
     * TODO: Values past 16 no longer show up in the season 3 client shop UI.
     * TODO: Although there are more than 16 common_unit_name keys, we can't test their consumption currently.
     * The currency symbols are defined in common_message.gmd via common_unit_* keys.
     * The currency symbols for generic tickets (枚) and counters (個) are defined in gacha_res.gmd.
     */
    public enum WalletType : byte
    {
        None = 0,
        Gold = 1, // G
        RiftPoints = 2, // R
        BloodOrbs = 3, // BO
        SilverTickets = 4, // 枚
        GoldenGemstones = 5, // 個
        RentalPoints = 6, // RP
        ResetJobPoints = 7, // JP_RESET => No icon, not testable, unsure
        ResetCraftSkills = 8, // CP_RESET => No icon, not testable, unsure
        HighOrbs = 9, // HO
        DominionPoints = 10, // DP
        AdventurePassPoints = 11, // BP
        CustomMadeServiceTickets = 12, // 枚
        BitterblackMazeResetTicket = 13,
        GoldenDragonMark = 14, // 個
        SilverDragonMark = 15, // 個
        RedDragonMark = 16 // 個
        // Play Points
        // Clan Points
    }
}
