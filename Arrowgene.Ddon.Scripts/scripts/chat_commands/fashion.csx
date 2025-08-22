#load "libs.csx"

public class ChatCommand : IChatCommand
{
    private static readonly ILogger Logger = LogProvider.Logger(typeof(ChatCommand));

    public override AccountStateType AccountState => AccountStateType.User;
    public override string CommandName => "fashion";
    public override string HelpText => "usage: `/fashion help/check/reset/add/save/load/apply [*]` - Commands for altering dress equipment.";

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
                    responses.Add(ChatResponse.ServerChat(client, $"/fashion help: Print this."));
                    responses.Add(ChatResponse.ServerChat(client, $"/fashion check [pawnName]: Show your current template."));
                    responses.Add(ChatResponse.ServerChat(client, $"/fashion reset [pawnName]: Empty the current template."));
                    responses.Add(ChatResponse.ServerChat(client, $"/fashion add [pawnName]: Add your equipped dress items to the template."));
                    responses.Add(ChatResponse.ServerChat(client, $"/fashion apply [pawnName]: Apply the current template."));
                    responses.Add(ChatResponse.ServerChat(client, $"/fashion save templateName [pawnName]: Save a template for later use."));
                    responses.Add(ChatResponse.ServerChat(client, $"/fashion load templateName [pawnName]: Load a template, overwriting the current template."));
                    break;
                }
            case "check":
                {
                    if (GetTargetCharacter(client, 1, command, responses, out var targetCharacter))
                    {
                        if (!CheckItems(client, targetCharacter, out var names))
                        {
                            PrintTemplate(client, targetCharacter, responses, LobbyChatMsgType.ManagementAlertC, names);
                            responses.Add(ChatResponse.CommandError(client, $"Template invalid; item missing."));
                            return;
                        }
                        if (!CheckEnsembleRules(client, targetCharacter))
                        {
                            PrintTemplate(client, targetCharacter, responses, LobbyChatMsgType.ManagementAlertC, names);
                            responses.Add(ChatResponse.CommandError(client, $"Template invalid; you cannot mix regular and ensemble gear."));
                            return;
                        }
                        if (!CheckEmptySpace(client, targetCharacter))
                        {
                            PrintTemplate(client, targetCharacter, responses, LobbyChatMsgType.ManagementAlertC, names);
                            responses.Add(ChatResponse.CommandError(client, $"Template invalid; unequip any vanity items that are not in the template."));
                            return;
                        }

                        PrintTemplate(client, targetCharacter, responses, LobbyChatMsgType.ManagementGuideC, names);
                    }
                    break;
                }
            case "add":
                {
                    if (GetTargetCharacter(client, 1, command, responses, out var targetCharacter))
                    {
                        AddFashionData(targetCharacter);
                        PrintTemplate(client, targetCharacter, responses, LobbyChatMsgType.ManagementGuideC, GetNames(client, targetCharacter));
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
                            PrintTemplate(client, targetCharacter, responses, LobbyChatMsgType.ManagementGuideC, GetNames(client, targetCharacter));
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
                            responses.Add(ChatResponse.CommandError(client, $"Template could not be applied; item missing."));
                            foreach (var name in names)
                            {
                                responses.Add(ChatResponse.CommandError(client, $"{name}"));
                            }
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

    private void PrintTemplate(GameClient client, CharacterCommon targetCharacter, List<ChatResponse> responses, LobbyChatMsgType chatType, IEnumerable<string> names)
    {
        responses.Add(ChatResponse.ServerChat(client, $"Template for {targetCharacter.CDataCharacterName}:"));
        foreach (var name in names)
        {
            responses.Add(new ChatResponse(client, $"{name}", chatType));
        }

    }

    private void PrintTemplate(GameClient client, CharacterCommon targetCharacter, List<ChatResponse> responses)
    {
        responses.Add(ChatResponse.ServerChat(client, $"Template for {targetCharacter.CDataCharacterName}:"));
        foreach (var name in GetNames(client, targetCharacter))
        {
            responses.Add(ChatResponse.ServerChat(client, $"{name}"));
        }
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
