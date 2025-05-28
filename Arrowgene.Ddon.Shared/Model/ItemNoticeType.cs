namespace Arrowgene.Ddon.Shared.Model
{
    public enum ItemNoticeType : ushort
    {
        Default = 0x0,
        Gather = 0x1,
        Drop = 0x2,
        UseBag = 0x3,
        ConsumeBag = 0x4,
        ConsumeStorage = 0x5,
        StoreStorage_item = 0x6,
        LoadStorage_item = 0x7,
        StoreStorage_items = 0x8,
        LoadStorage_items = 0x9,
        ShopGoods_buy = 0xa,
        ShopItemSell = 0xb,
        ReceiveAreaSupply = 0xc,
        CreatePawn = 0xd,
        DeletePawn = 0xe,
        UseGatheringItem = 0xf,
        Quest = 0x10,
        StorePostItemGacha = 0x11,
        StorePostItemMail = 0x12,
        BazaarExhibit = 0x13,
        BazaarCancel = 0x14,
        BazaarProceeds = 0x15,
        ExStorageItems = 0x16,
        ConsumeExStorage = 0x17,
        StartCraft = 0x18,
        GetCraftProduct = 0x19,
        StartEquipGradeUp = 0x1a,
        StartAttachElement = 0x1b,
        StartDetachElement = 0x1c,
        StartEquipColorChang = 0x1d,
        LoadPostItems = 0x1e,
        StampBonus = 0x1f,
        QuestDelivery = 0x20,
        UseJobItem = 0x21,
        GpItem = 0x22,
        CraftCreate = 0x23,
        ChangeEquip = 0x24,
        ChangePawnEquip = 0x25,
        ChangeStorageEquip = 0x26,
        ChangeStoragePawnEquip = 0x27,
        ChangeJob = 0x28,
        ChangePawnJob = 0x29,
        OrbDevoteConsume = 0x2a,
        GetDispelItem = 0x2b,
        SwitchingStorage = 0x2c,
        CraftRecipeRelease = 0x2d,
        FurnitureLayout = 0x2e,
        ResetCraftpoint = 0x2f,
        BaggageItems = 0x30,
        TemporaryItems = 0x31,
        ReleaseTreeElement = 0x32,
        PawnExpeditionDrop = 0x33,
        StorePostItemBoxGacha = 0x34,
        // 0x35 // Item obtained
        // 0x36 // Item obtained
        // 0x37
        SoulOrdealReward = 0x38,
        // 0x39 // Item obtained
        // 0x3a // Item consumed
        ReceivedItemFromCrafting = 0x3b,
        // 0x3c
        // 0x3d
        // 0x3e
        // 0x3f
        // 0x40
        // 0x41 // Item obtained
        // 0x42 // Item used and obtained
        // 0x43
        // 0x44 // Item consumed
        ItemSafetySetting = 0x45,
        EmblemStartAttach = 0x46,
        EmblemStartDetach = 0x47,
        // 0x48
        // 0x49 // Item obtained
        EmblemStatUpdate = 0x4a,
        GatherEquipItem = 0x4b,
        // 0x4c Item Obtained
        // 0x4d Item Obtained
        // 0x4e Item Obtained
        // 0x4f Item Obtained
        Debug = 0x64,
        DebugAdd = 0x64,
        DebugSub = 0x65,
        DebugEquipAdd = 0x66,

        // ItemsBought = 0x10a,
        // UseJobItem2 = 0x121,
    }
}
