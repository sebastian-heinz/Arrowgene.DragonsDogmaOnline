#load "libs.csx"

public class ChatCommand : IChatCommand
{
    private static readonly ILogger Logger = LogProvider.Logger(typeof(ChatCommand));

    public override AccountStateType AccountState => AccountStateType.User;
    public override string CommandName => "fashion";
    public override string HelpText => "usage: `/fashion help/check/reset/add/save/load/apply/dumpids/fromids [*]` - Commands for altering dress equipment.";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        if (!LibDdon.GetSetting<bool>("GameServerSettings", "EnableVisualEquip"))
        {
            responses.Add(ChatResponse.CommandError(client, "This command is not enabled."));
            return;
        }

        if (!client.Character.HasContentReleased(ContentsRelease.DressEquipment))
        {
            responses.Add(ChatResponse.CommandError(client, "You do not have this feature unlocked yet."));
            return;
        }

        if (!StageManager.IsSafeArea(client.Character.Stage))
        {
            responses.Add(ChatResponse.CommandError(client, "You must be in a safe area to use this command."));
            return;
        }

        string chosenCommand = "help";
        if (command.Length >= 1)
        {
            chosenCommand = command[0];
        }

        switch (chosenCommand.ToLowerInvariant())
        {
            case "help":
                {
                    client.Send(new S2CConnectionInformationNtc([
                        "/fashion help: Print this.",
                        "/fashion check [pawnName]: Show your current template.",
                        "/fashion reset [pawnName]: Empty the current template.",
                        "/fashion add [pawnName]: Add your equipped dress items to the template.",
                        "/fashion apply [pawnName]: Apply the current template.",
                        "/fashion save templateName [pawnName]: Save a template for later use (this session only).",
                        "/fashion load templateName [pawnName]: Load a template, overwriting the current template (this session only).",
                        "/fashion dumpids [pawnName]: Print vanity ItemIds (7 slots).",
                        "/fashion fromids id,id,... [pawnName]: Build template from 7 vanity ItemIds you own."
                    ]));
                    break;
                }
            case "check":
                {
                    if (GetTargetCharacter(client, 1, command, responses, out var targetCharacter))
                    {
                        if (!CheckItems(client, targetCharacter, out var names))
                        {
                            PrintTemplate(client, targetCharacter, names, "Template invalid; item missing.");
                            responses.Add(ChatResponse.CommandError(client, $"Template invalid; item missing."));
                            return;
                        }
                        if (!CheckEnsembleRules(client, targetCharacter))
                        {
                            PrintTemplate(client, targetCharacter, names, "Template invalid; you cannot mix regular and ensemble gear.");
                            responses.Add(ChatResponse.CommandError(client, $"Template invalid; you cannot mix regular and ensemble gear."));
                            return;
                        }
                        if (!CheckEmptySpace(client, targetCharacter))
                        {
                            PrintTemplate(client, targetCharacter, names, "Template invalid; unequip any vanity items that are not in the template.");
                            responses.Add(ChatResponse.CommandError(client, $"Template invalid; unequip any vanity items that are not in the template."));
                            return;
                        }

                        PrintTemplate(client, targetCharacter, names);
                    }
                    break;
                }
            case "add":
                {
                    if (GetTargetCharacter(client, 1, command, responses, out var targetCharacter))
                    {
                        AddFashionData(targetCharacter);
                        PrintTemplate(client, targetCharacter, GetNames(client, targetCharacter));
                    }
                    break;
                }
            case "reset":
                {
                    if (GetTargetCharacter(client, 1, command, responses, out var targetCharacter))
                    {
                        ResetFashionData(targetCharacter);
                        responses.Add(ChatResponse.ServerChat(client, $"Template for {targetCharacter.CDataCharacterName} reset."));
                    }
                    break;
                }
            case "save":
                {
                    if (GetTargetCharacter(client, 2, command, responses, out var targetCharacter))
                    {
                        if (command.Length == 1)
                        {
                            responses.Add(ChatResponse.CommandError(client, "No template name provided."));
                            return;
                        }

                        string password = command[1];

                        SaveFashion(client, targetCharacter, password);
                        responses.Add(ChatResponse.ServerChat(client, $"Template for {targetCharacter.CDataCharacterName} saved with pass:"));
                        responses.Add(ChatResponse.ServerChat(client, $"    '{password}'"));
                    }
                    break;
                }
            case "load":
                {
                    if (GetTargetCharacter(client, 2, command, responses, out var targetCharacter))
                    {
                        if (command.Length == 1)
                        {
                            responses.Add(ChatResponse.CommandError(client, "No template name provided."));
                            return;
                        }

                        string password = command[1];

                        if (LoadFashion(client, targetCharacter, password))
                        {
                            PrintTemplate(client, targetCharacter, GetNames(client, targetCharacter));
                        }
                        else
                        {
                            responses.Add(ChatResponse.CommandError(client, $"No template was found by that name."));
                        }
                    }
                    break;
                }
            case "apply":
                {
                    if (GetTargetCharacter(client, 1, command, responses, out var targetCharacter))
                    {
                        if (client.Party.GetPartyMemberByCharacter(targetCharacter) is null)
                        {
                            responses.Add(ChatResponse.CommandError(client, $"Template could not be applied; {targetCharacter.CDataCharacterName} is not in your party."));
                            return;
                        }

                        if (!CheckItems(client, targetCharacter, out var names))
                        {
                            PrintTemplate(client, targetCharacter, names, "Template could not be applied; item missing.");
                            responses.Add(ChatResponse.CommandError(client, $"Template could not be applied; item missing."));
                            return;
                        }

                        if (!CheckEnsembleRules(client, targetCharacter))
                        {
                            responses.Add(ChatResponse.CommandError(client, $"Template could not be applied; you cannot mix regular and ensemble gear."));
                            return;
                        }

                        if (!CheckEmptySpace(client, targetCharacter))
                        {
                            responses.Add(ChatResponse.CommandError(client, $"Template could not be applied; unequip any vanity items that are not in the template."));
                            return;
                        }

                        try
                        {
                            HandleSwap(server, client, targetCharacter).Send();
                            responses.Add(ChatResponse.ServerChat(client, $"Template for {targetCharacter.CDataCharacterName} applied."));
                        }
                        catch (Exception ex)
                        {
                            responses.Add(ChatResponse.CommandError(client, $"Something went wrong. You may not have enough inventory space to handle the swapping."));
                        }
                    }
                    break;
                }
            case "dumpids":
                {
                    if (GetTargetCharacter(client, 1, command, responses, out var targetCharacter))
                    {
                        var itemIds = UidsToItemIds(client, targetCharacter, GetFashionData(targetCharacter));
                        var compact = string.Join(",", ToCompactFashionIds(itemIds));
                        // Compact list: helm,body,wearBody,arm,leg,wearLeg,accessory (no weapons/jewelry/lantern).
                        PrintTemplate(
                            client,
                            targetCharacter,
                            GetNames(client, targetCharacter),
                            $"FASHION_IDS:{compact}",
                            "Copy the FASHION_IDS line for /fashion fromids.");
                    }
                    break;
                }
            case "fromids":
                {
                    if (command.Length < 2)
                    {
                        responses.Add(ChatResponse.CommandError(client, "usage: /fashion fromids id,id,... [pawnName]"));
                        return;
                    }

                    // Optional pawn name is the last arg when more than one arg follows fromids.
                    int pawnArgIndex = command.Length >= 3 ? 2 : int.MaxValue;
                    if (!GetTargetCharacter(client, pawnArgIndex, command, responses, out var targetCharacter))
                    {
                        return;
                    }

                    if (!TryParseItemIdList(command[1], out var itemIds, out var parseError))
                    {
                        responses.Add(ChatResponse.CommandError(client, parseError));
                        return;
                    }

                    var missingNames = new List<string>();
                    var wrongSlotNames = new List<string>();
                    var resolved = ResolveItemIdsToUids(client, targetCharacter, itemIds, missingNames, wrongSlotNames);
                    FashionTable.AddOrUpdate(targetCharacter, resolved);

                    int boundCount = resolved.Count(x => x is not null);
                    var extra = new List<string>();
                    if (boundCount == 0)
                    {
                        extra.Add("No ItemIds could be matched to items you own in the correct slots. Working template is empty.");
                    }
                    else
                    {
                        extra.Add($"Bound {boundCount} slot(s) to items you own.");
                    }
                    foreach (var wrong in wrongSlotNames)
                    {
                        extra.Add($"Wrong slot: {wrong}");
                    }
                    foreach (var missing in missingNames)
                    {
                        extra.Add($"Missing owned item: {missing}");
                    }
                    if (missingNames.Count > 0 || wrongSlotNames.Count > 0)
                    {
                        extra.Add("Apply will fail or stay incomplete until each slot has a valid owned piece.");
                    }
                    PrintTemplate(client, targetCharacter, GetNames(client, targetCharacter), [.. extra]);
                    break;
                }
            default:
                {
                    responses.Add(ChatResponse.CommandError(client, $"Unknown fashion subcommand."));
                    break;
                }
        }
    }

    private ConditionalWeakTable<CharacterCommon, List<string>> FashionTable { get; } = [];
    private ConditionalWeakTable<GameClient, Dictionary<string, List<string>>> LockedFashions { get; } = [];
    private HashSet<StorageType> StorageTypes { get; } = [StorageType.StorageBoxNormal, StorageType.StorageBoxExpansion, StorageType.ItemBagEquipment];
    private static byte TOTAL_EQUIP_SLOTS => EquipmentTemplate.TOTAL_EQUIP_SLOTS;
    // dumpids/fromids use only these vanity indexes (ArmorHelm..Accessory).
    // Excluded: weapons (0-1), jewelry (9-13), lantern (14).
    private static readonly int[] FashionSlotIndexes = [2, 3, 4, 5, 6, 7, 8];
    private static int FashionSlotCount => FashionSlotIndexes.Length;
    private const int MaxFromIdsPayloadLength = 128;

    private static bool IsFashionSlotIndex(int index)
    {
        return index >= 2 && index <= 8;
    }

    private static List<uint> ToCompactFashionIds(List<uint> fullSlotIds)
    {
        var compact = new List<uint>(FashionSlotCount);
        foreach (int index in FashionSlotIndexes)
        {
            compact.Add(index < fullSlotIds.Count ? fullSlotIds[index] : 0);
        }
        return compact;
    }

    private static List<uint> FromCompactFashionIds(List<uint> compactIds)
    {
        var full = Enumerable.Repeat(0u, TOTAL_EQUIP_SLOTS).ToList();
        for (int i = 0; i < FashionSlotCount && i < compactIds.Count; i++)
        {
            full[FashionSlotIndexes[i]] = compactIds[i];
        }
        return full;
    }

    private static void ClearNonFashionSlots(List<uint> itemIds)
    {
        for (int i = 0; i < itemIds.Count; i++)
        {
            if (!IsFashionSlotIndex(i))
            {
                itemIds[i] = 0;
            }
        }
    }

    private void PrintTemplate(GameClient client, CharacterCommon targetCharacter, IEnumerable<string> names, params string[] extraLines)
    {
        List<string> lines = [$"Template for {targetCharacter.CDataCharacterName}:"];
        var nameList = names?.ToList() ?? [];
        if (nameList.Count == 0)
        {
            lines.Add("(empty — no items in the working template)");
        }
        else
        {
            lines.AddRange(nameList);
        }
        if (extraLines is not null && extraLines.Length > 0)
        {
            lines.AddRange(extraLines.Where(x => !string.IsNullOrWhiteSpace(x)));
        }
        client.Send(new S2CConnectionInformationNtc(lines));
    }

    private bool GetTargetCharacter(GameClient client, int index, string[] command, List<ChatResponse> responses, out CharacterCommon targetCharacter)
    {
        if (index >= command.Length)
        {
            targetCharacter = client.Character;
            return true;
        }
        else
        {
            targetCharacter = client.Character.Pawns
                .Select((pawn, index) => new { pawn = pawn, pawnNumber = (byte)(index + 1) })
                .Where(tuple => tuple.pawn.Name == command[index])
                .FirstOrDefault()
                ?.pawn;

            if (targetCharacter is null)
            {
                responses.Add(ChatResponse.CommandError(client, "No pawn was found by that name."));
                return false;
            }

            return true;
        }
    }

    private List<string> GetFashionData(CharacterCommon character)
    {
        return FashionTable.GetValue(character, x => [.. Enumerable.Repeat<string>(null, TOTAL_EQUIP_SLOTS)]);
    }

    private void ResetFashionData(CharacterCommon character)
    {
        FashionTable.AddOrUpdate(character, [.. Enumerable.Repeat<string>(null, TOTAL_EQUIP_SLOTS)]);
    }

    private void SetFashionData(CharacterCommon character)
    {
        FashionTable.AddOrUpdate(character, FetchFashionFromCharacter(character));
    }

    private void AddFashionData(CharacterCommon character)
    {
        var currentFashion = GetFashionData(character);
        var newFashion = FetchFashionFromCharacter(character);

        for (int i = 0; i < TOTAL_EQUIP_SLOTS; i++)
        {
            if (newFashion[i] is not null)
            {
                currentFashion[i] = newFashion[i];
            }
        }
    }

    private static bool TryParseItemIdList(string raw, out List<uint> itemIds, out string error)
    {
        itemIds = null;
        error = null;

        if (string.IsNullOrWhiteSpace(raw))
        {
            error = "No ItemId list provided.";
            return false;
        }

        if (raw.Length > MaxFromIdsPayloadLength)
        {
            error = $"ItemId list is too long (max {MaxFromIdsPayloadLength} characters).";
            return false;
        }

        var parts = raw.Split(',', StringSplitOptions.TrimEntries);
        // Accept compact (7 vanity slots) or legacy full (15 slots with unused zeros).
        if (parts.Length != FashionSlotCount && parts.Length != TOTAL_EQUIP_SLOTS)
        {
            error = $"Expected {FashionSlotCount} vanity ItemIds (or legacy {TOTAL_EQUIP_SLOTS}), got {parts.Length}.";
            return false;
        }

        var parsed = new List<uint>(parts.Length);
        for (int i = 0; i < parts.Length; i++)
        {
            string part = parts[i];
            if (string.IsNullOrEmpty(part))
            {
                error = $"Invalid ItemId at slot {i} (empty).";
                return false;
            }

            // Reject signs, hex, and overflow (> uint.MaxValue).
            if (!uint.TryParse(part, System.Globalization.NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture, out var itemId))
            {
                error = $"Invalid ItemId '{part}' at slot {i}. Use plain non-negative integers only.";
                return false;
            }

            parsed.Add(itemId);
        }

        itemIds = parts.Length == FashionSlotCount
            ? FromCompactFashionIds(parsed)
            : parsed;
        ClearNonFashionSlots(itemIds);
        return true;
    }

    private List<uint> UidsToItemIds(GameClient client, CharacterCommon character, List<string> uids)
    {
        var result = new List<uint>(TOTAL_EQUIP_SLOTS);
        var equipped = character.Equipment.GetItems(EquipType.Visual);

        for (int i = 0; i < TOTAL_EQUIP_SLOTS; i++)
        {
            if (!IsFashionSlotIndex(i))
            {
                result.Add(0);
                continue;
            }

            string uid = uids[i];
            if (uid is null)
            {
                result.Add(0);
                continue;
            }

            var equippedItem = equipped.FirstOrDefault(item => item?.UId == uid);
            if (equippedItem is not null)
            {
                result.Add(equippedItem.ItemId);
                continue;
            }

            var found = client.Character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, uid);
            if (found is not null)
            {
                result.Add(found.Item2.Item2.ItemId);
                continue;
            }

            Logger.Error($"Fashion dumpids could not resolve UID '{uid}' to an ItemId.");
            result.Add(0);
        }

        ClearNonFashionSlots(result);
        return result;
    }

    private static EquipSlot ExpectedEquipSlotForTemplateIndex(int templateIndex)
    {
        // Template indexes are 0-based; EquipSlot values are 1-based and align 1:1.
        return (EquipSlot)(templateIndex + 1);
    }

    private static bool ItemFitsFashionSlot(uint itemId, int templateIndex, out string itemLabel)
    {
        itemLabel = $"ItemId {itemId}";
        if (!LibDdon.Assets.ClientItemInfos.ContainsKey(itemId))
        {
            return false;
        }

        var info = LibDdon.Assets.ClientItemInfos[itemId];
        itemLabel = info.Name ?? itemLabel;
        EquipSlot expected = ExpectedEquipSlotForTemplateIndex(templateIndex);
        return info.EquipSlot == expected;
    }

    private List<string> ResolveItemIdsToUids(
        GameClient client,
        CharacterCommon character,
        List<uint> itemIds,
        List<string> missingNames,
        List<string> wrongSlotNames)
    {
        var result = new List<string>(TOTAL_EQUIP_SLOTS);
        var usedUids = new HashSet<string>();

        for (int i = 0; i < TOTAL_EQUIP_SLOTS; i++)
        {
            uint itemId = itemIds[i];
            if (!IsFashionSlotIndex(i) || itemId == 0)
            {
                result.Add(null);
                continue;
            }

            if (!ItemFitsFashionSlot(itemId, i, out var itemLabel))
            {
                result.Add(null);
                EquipSlot expected = ExpectedEquipSlotForTemplateIndex(i);
                if (!LibDdon.Assets.ClientItemInfos.ContainsKey(itemId))
                {
                    missingNames.Add(itemLabel);
                }
                else
                {
                    var actual = LibDdon.Assets.ClientItemInfos[itemId].EquipSlot;
                    wrongSlotNames.Add($"{itemLabel} → slot {expected}, item is {actual?.ToString() ?? "unknown"}");
                }
                continue;
            }

            string uid = FindOwnedUidForItemId(client, character, itemId, usedUids);
            if (uid is null)
            {
                result.Add(null);
                missingNames.Add(itemLabel);
            }
            else
            {
                result.Add(uid);
            }
        }

        return result;
    }

    private string FindOwnedUidForItemId(GameClient client, CharacterCommon character, uint itemId, HashSet<string> usedUids)
    {
        foreach (var item in character.Equipment.GetItems(EquipType.Visual).Where(x => x is not null))
        {
            if (item.ItemId == itemId && usedUids.Add(item.UId))
            {
                return item.UId;
            }
        }

        foreach (var match in client.Character.Storage.FindItemsByIdInStorage(StorageTypes, (ItemId)itemId))
        {
            // Use positional fields so this compiles against both named and unnamed tuple shapes.
            string uid = match.Item2.Item2.UId;
            if (usedUids.Add(uid))
            {
                return uid;
            }
        }

        foreach (var match in client.Character.Storage.FindItemsByIdInStorage(ItemManager.EquipmentStorages, (ItemId)itemId))
        {
            string uid = match.Item2.Item2.UId;
            if (usedUids.Add(uid))
            {
                return uid;
            }
        }

        return null;
    }

    private void SaveFashion(GameClient client, CharacterCommon character, string password)
    {
        LockedFashions.GetValue(client, x => [])[password] = GetFashionData(character);
    }

    private bool LoadFashion(GameClient client, CharacterCommon character, string password)
    {
        if (TryGetLockedFashion(client, password, out var fashions))
        {
            FashionTable.AddOrUpdate(character, fashions);
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool TryGetLockedFashion(GameClient client, string password, out List<string> fashions)
    {
        if (!LockedFashions.TryGetValue(client, out var dict))
        {
            fashions = null;
            return false;
        }
        else
        {
            var status = dict.TryGetValue(password, out var foundFashions);
            fashions = foundFashions;
            return status;
        }
    }

    private List<string> GetNames(GameClient client, CharacterCommon character)
    {
        List<string> strings = [];
        var equippedItems = character.Equipment.GetItems(EquipType.Visual).Where(x => x is not null).Select(x => x.UId);
        foreach (var item in GetFashionData(character))
        {
            if (item is null)
            {
                continue;
            }

            var foundItem = client.Character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, item);
            bool isEquipped = equippedItems.Contains(item);
            if (foundItem is null)
            {
                strings.Add("* !UNKNOWN ITEM!");
            }
            else if (StorageTypes.Contains(foundItem.Item1))
            {
                var itemData = LibDdon.Assets.ClientItemInfos[foundItem.Item2.Item2.ItemId];
                strings.Add($"* {itemData.Name} -OK-");
            }
            else if (isEquipped)
            {
                var itemData = LibDdon.Assets.ClientItemInfos[foundItem.Item2.Item2.ItemId];
                strings.Add($"* {itemData.Name} -EQUIPPED-");
            }
            else
            {
                var itemData = LibDdon.Assets.ClientItemInfos[foundItem.Item2.Item2.ItemId];
                strings.Add($"* {itemData.Name} !NOT AVAILABLE!");
            }
        }

        return strings;
    }

    private List<CDataCharacterEquipInfo> AsCDataCharacterEquipInfo(CharacterCommon character)
    {
        var equippedItems = character.Equipment.GetItems(EquipType.Visual).Where(x => x is not null).Select(x => x.UId);
        return [.. GetFashionData(character)
                .Select((x, index) => new { Item = x, Slot = (byte)(index + 1) })
                .Where(tuple => tuple.Item is not null
                    && !equippedItems.Contains(tuple.Item))
                .Select(tuple => new CDataCharacterEquipInfo()
                {
                    EquipItemUId = tuple!.Item,
                    EquipType = EquipType.Visual,
                    EquipCategory = tuple!.Slot
                })];
    }

    private bool CheckItems(GameClient client, CharacterCommon character, out List<string> names)
    {
        names = GetNames(client, character);
        var equippedItems = character.Equipment.GetItems(EquipType.Visual).Where(x => x is not null).Select(x => x.UId);
        foreach (var item in GetFashionData(character).Where(x => x is not null))
        {
            bool isInInventory = client.Character.Storage.FindItemByUIdInStorage(StorageTypes, item) is not null;
            bool isEquipped = equippedItems.Contains(item);
            if (!isInInventory && !isEquipped)
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckEnsembleRules(GameClient client, CharacterCommon character)
    {
        var items = GetFashionData(character).Where(x => x is not null);
        bool containsEnsemble = false;
        bool containsOther = false;
        foreach (var item in items)
        {
            if (containsEnsemble && containsOther)
            {
                continue;
            }

            var itemInInventory = client.Character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, item);
            if (itemInInventory is null)
            {
                // An item is missing, so no need to compute further.
                return false;
            }

            var itemInfo = LibDdon.Assets.ClientItemInfos[itemInInventory.Item2.Item2];
            if (itemInfo.SubCategory == ItemSubCategory.EquipEnsemble)
            {
                containsEnsemble = true;
            }
            else if (EquipManager.EnsembleSlots.Contains(itemInfo.EquipSlot ?? 0))
            {
                containsOther = true;
            }
        }

        return !(containsEnsemble && containsOther);
    }

    private bool CheckEmptySpace(GameClient client, CharacterCommon character)
    {
        IEnumerable<string> items = GetFashionData(character).Where(x => x is not null);

        // Calculate what slots are going to be occupied after we apply the template.
        var equippedItems = character.Equipment.GetItems(EquipType.Visual);
        bool hasEnsembleEquipped = equippedItems.Any(x => x is not null && LibDdon.Assets.ClientItemInfos[x].SubCategory == ItemSubCategory.EquipEnsemble);
        foreach (var item in items)
        {
            var itemInInventory = client.Character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, item);
            if (itemInInventory is null)
            {
                // An item is missing, so no need to compute further.
                return false;
            }
            else
            {
                var itemInfo = LibDdon.Assets.ClientItemInfos[itemInInventory.Item2.Item2];
                var currentSlotItem = equippedItems[(byte)itemInfo.EquipSlot - 1];

                //Logger.Info($"{itemInInventory}")

                if (currentSlotItem?.UId == item)
                {
                    // This item is already equipped and doesn't have to be moved.
                    continue;
                }
                else if (currentSlotItem is not null)
                {
                    // Check for the primary slot being already occupied.
                    return false;
                }
                else if (itemInfo.SubCategory == ItemSubCategory.EquipEnsemble
                    && EquipManager.EnsembleSlots.Any(x => equippedItems[(byte)x - 1] is not null))
                {
                    // Check for an incoming ensemble displacing an equipped item in any slot
                    return false;
                }
                else if (hasEnsembleEquipped && EquipManager.EnsembleSlots.Contains(itemInfo.EquipSlot ?? 0))
                {
                    // Check for the incoming item displacing an equipped ensemble cross-ways.
                    return false;
                }
            }
        }

        return true;
    }

    private PacketQueue HandleSwap(DdonGameServer server, GameClient client, CharacterCommon character)
    {
        PacketQueue queue = new();

        server.Database.ExecuteInTransaction(connection =>
        {
            queue.AddRange(server.EquipManager.HandleChangeEquipList(
                server, client,
                character,
                AsCDataCharacterEquipInfo(character),
                ItemNoticeType.ChangeEquip,
                [.. StorageTypes],
                connection));
        });

        if (character is Character arisen)
        {
            client.Enqueue(new S2CEquipChangeCharacterEquipNtc()
            {
                CharacterId = arisen.CharacterId,
                EquipItemList = character.Equipment.AsCDataEquipItemInfo(EquipType.Performance),
                VisualEquipItemList = character.Equipment.AsCDataEquipItemInfo(EquipType.Visual)
            }, queue);

            client.Enqueue(arisen.S2CContextGetLobbyPlayerContextNtc, queue);
        }
        else if (character is Pawn pawn)
        {
            client.Enqueue(new S2CEquipChangePawnEquipNtc()
            {
                CharacterId = pawn.CharacterId,
                PawnId = pawn.PawnId,
                EquipItemList = pawn.Equipment.AsCDataEquipItemInfo(EquipType.Performance),
                VisualEquipItemList = pawn.Equipment.AsCDataEquipItemInfo(EquipType.Visual),
            }, queue);

            var member = client.Party.GetPartyMemberByCharacter(pawn);
            if (member is not null && member is PawnPartyMember pawnMember)
            {
                client.Party.EnqueueToAll(pawnMember.GetPartyContext(), queue);
            }
        }

        return queue;
    }

    private List<string> FetchFashionFromCharacter(CharacterCommon character)
    {
        return new List<string>(character.EquipmentTemplate
            .GetEquipment(character.Job, EquipType.Visual)
            .Select(x => x?.UId))
        {
            // Trim out weapon and subweapon for animation reasons.
            [0] = null,
            [1] = null
        };
    }
}

return new ChatCommand();
