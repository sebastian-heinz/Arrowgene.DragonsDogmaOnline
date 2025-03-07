public class PartyUtils
{
    /**
     * @brief Calculates the difference in levels between the lowest and highest members of the party.
     *
     * @param[in] party The party the client is currently a member of.
     *
     * @return Returns the difference in levels between the lowest and highest members of the party.
     */
    public static uint LevelRange(PartyGroup party)
    {
        var firstMember = party.Clients.First();
        uint maxLevel = firstMember.Character.ActiveCharacterJobData.Lv;
        uint minLevel = firstMember.Character.ActiveCharacterJobData.Lv;

        foreach (var member in party.Members)
        {
            CharacterCommon characterCommon = null;
            if (member is PlayerPartyMember)
            {
                var client = ((PlayerPartyMember)member).Client;
                characterCommon = client.Character;
            }
            else if (member is PawnPartyMember)
            {
                characterCommon = ((PawnPartyMember)member).Pawn;
            }

            maxLevel = Math.Max(maxLevel, characterCommon.ActiveCharacterJobData.Lv);
            minLevel = Math.Min(minLevel, characterCommon.ActiveCharacterJobData.Lv);
        }

        return maxLevel - minLevel;
    }

    /**
     * @brief Determines the level of the highest member in the party.
     *
     * @param[in] party The party the client is currently a member of.
     *
     * @return Returns the level of the highest member in the party.
     */
    public static uint MemberMaxLevel(PartyGroup party)
    {
        uint maxLevel = party.Clients.First().Character.ActiveCharacterJobData.Lv;
        foreach (var member in party.Members)
        {
            CharacterCommon characterCommon = null;
            if (member is PlayerPartyMember)
            {
                var client = ((PlayerPartyMember)member).Client;
                characterCommon = client.Character;
            }
            else if (member is PawnPartyMember)
            {
                characterCommon = ((PawnPartyMember)member).Pawn;
            }

            maxLevel = Math.Max(maxLevel, characterCommon.ActiveCharacterJobData.Lv);
        }

        return maxLevel;
    }

    /**
     * @brief Determines if all members in the party are owned by the party leader.
     *
     * @param[in] party The party the client is currently a member of.
     *
     * @return Returns true if all members are owned by the party leader, else false.
     */
    public static bool AllMembersOwnedByPartyLeader(PartyGroup party)
    {
        uint characterId = 0;
        foreach (var member in party.Members)
        {
            uint id = 0;
            if (member is PlayerPartyMember)
            {
                var client = ((PlayerPartyMember)member).Client;
                id = client.Character.CharacterId;
            }
            else if (member is PawnPartyMember)
            {
                var pawn = ((PawnPartyMember)member).Pawn;
                if (pawn.IsRented)
                {
                    return false;
                }

                id = pawn.CharacterId;
            }

            if (characterId == 0)
            {
                characterId = id;
            }

            if (characterId != id)
            {
                return false;
            }
        }

        return true;
    }

    /**
     * @brief Finds the GameClient object for the owner of the pawn in the party.
     *
     * @param[in] pawn The pawn to find the owners client for.
     * @param[in] party The party the pawn is a member of.
     *
     * @return Returns the owners client if it exists, otherwise null.
     */
    public static GameClient GetPawnOwner(Pawn pawn, PartyGroup party)
    {
        foreach (var client in party.Clients)
        {
            if (pawn.CharacterId == client.Character.CharacterId)
            {
                return client;
            }
        }
        return null;
    }
}