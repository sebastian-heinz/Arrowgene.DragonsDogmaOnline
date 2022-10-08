using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer;

public static class GameStructure
{
    public static void CDataCharacterListElement(
        CDataCharacterListElement cDataCharacterListElement,
        Character character)
    {
        CDataCommunityCharacterBaseInfo(cDataCharacterListElement.CommunityCharacterBaseInfo, character);
        cDataCharacterListElement.ServerId = 0;
        cDataCharacterListElement.OnlineStatus = 0;
        CDataJobBaseInfo(cDataCharacterListElement.CurrentJobBaseInfo, character.Job,
            (byte)character.ActiveCharacterJobData.Lv);
        CDataJobBaseInfo(cDataCharacterListElement.EntryJobBaseInfo, character.Job,
            (byte)character.ActiveCharacterJobData.Lv);
        cDataCharacterListElement.MatchingProfile = "";
        cDataCharacterListElement.unk2 = 0;
    }

    public static void CDataCommunityCharacterBaseInfo(
        CDataCommunityCharacterBaseInfo cDataCommunityCharacterBaseInfo,
        Character character)
    {
        cDataCommunityCharacterBaseInfo.CharacterId = character.Id;
        CDataCharacterName(cDataCommunityCharacterBaseInfo.CharacterName, character);
        cDataCommunityCharacterBaseInfo.ClanName = "";
    }

    public static void CDataJobBaseInfo(CDataJobBaseInfo cDataJobBaseInfo,
        JobId jobId,
        byte jobLevel)
    {
        cDataJobBaseInfo.Job = jobId;
        cDataJobBaseInfo.Level = jobLevel;
    }

    public static void CDataCharacterName(CDataCharacterName cDataCharacterName,
        Character character)
    {
        cDataCharacterName.FirstName = character.FirstName;
        cDataCharacterName.LastName = character.LastName;
    }

    public static void CDataPartyMember(CDataPartyMember cDataPartyMember,
        PartyMember partyMember)
    {
        CDataCharacterListElement(cDataPartyMember.CharacterListElement, partyMember.Character);
        cDataPartyMember.MemberType = partyMember.MemberType;
        cDataPartyMember.MemberIndex = partyMember.MemberIndex;
        cDataPartyMember.PawnId = partyMember.PawnId;
        cDataPartyMember.IsLeader = partyMember.IsLeader;
        cDataPartyMember.IsPawn = partyMember.IsPawn;
        cDataPartyMember.IsPlayEntry = partyMember.IsPlayEntry;
        cDataPartyMember.JoinState = partyMember.JoinState;
        cDataPartyMember.AnyValueList = partyMember.AnyValueList;
        cDataPartyMember.SessionStatus = partyMember.SessionStatus;
    }
}
