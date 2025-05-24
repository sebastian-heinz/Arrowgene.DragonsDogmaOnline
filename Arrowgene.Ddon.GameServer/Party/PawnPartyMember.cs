using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.GameServer.Party;

public class PawnPartyMember : PartyMember
{
    public Pawn Pawn { get; set; }

    public override CDataPartyMember GetCDataPartyMember()
    {
        CDataPartyMember obj = new CDataPartyMember();
        GameStructure.CDataPartyMember(obj, this);
        return obj;
    }

    public S2CContextGetPartyMyPawnContextNtc GetPartyContext()
    {
        CDataPartyContextPawn partyContextPawn = new CDataPartyContextPawn();
        GameStructure.CDataContextBase(partyContextPawn.Base, Pawn);
        partyContextPawn.Base.PawnId = PawnId;
        partyContextPawn.Base.CharacterId = Pawn.CharacterId;
        partyContextPawn.Base.PawnType = Pawn.PawnType;
        partyContextPawn.Base.HmType = Pawn.HmType;
        GameStructure.CDataContextPlayerInfo(partyContextPawn.PlayerInfo, Pawn);
        partyContextPawn.PawnReactionList = Pawn.PawnReactionList;
        partyContextPawn.TrainingStatus = Pawn.TrainingStatus.GetValueOrDefault(Pawn.Job, new byte[64]);
        partyContextPawn.SpSkillList = Pawn.SpSkills.GetValueOrDefault(Pawn.Job, new List<CDataSpSkill>());
        GameStructure.CDataContextResist(partyContextPawn.ResistInfo, Pawn);
        partyContextPawn.EditInfo = Pawn.EditInfo;

        S2CContextGetPartyMyPawnContextNtc partyPlayerContextNtc = new()
        {
            PawnId = PawnId,
            Context = partyContextPawn
        };
        partyPlayerContextNtc.Context.Base.MemberIndex = MemberIndex;
        return partyPlayerContextNtc;
    }

    public S2CContextGetPartyMyPawnContextNtc GetS2CContextGetParty_ContextNtcEx()
    {
        CDataPartyContextPawn partyContextPawn = new CDataPartyContextPawn();
        GameStructure.CDataContextBase(partyContextPawn.Base, Pawn);
        partyContextPawn.Base.PawnId = PawnId;
        partyContextPawn.Base.CharacterId = Pawn.CharacterId;
        partyContextPawn.Base.PawnType = Pawn.PawnType;
        partyContextPawn.Base.HmType = Pawn.HmType;
        GameStructure.CDataContextPlayerInfo(partyContextPawn.PlayerInfo, Pawn);
        partyContextPawn.PawnReactionList = Pawn.PawnReactionList;
        partyContextPawn.TrainingStatus = Pawn.TrainingStatus.GetValueOrDefault(Pawn.Job, new byte[64]);
        partyContextPawn.SpSkillList = Pawn.SpSkills.GetValueOrDefault(Pawn.Job, new List<CDataSpSkill>());
        GameStructure.CDataContextResist(partyContextPawn.ResistInfo, Pawn);
        partyContextPawn.EditInfo = Pawn.EditInfo;

        S2CContextGetPartyMyPawnContextNtc partyPlayerContextNtc = new S2CContextGetPartyMyPawnContextNtc();
        partyPlayerContextNtc.PawnId = PawnId;
        partyPlayerContextNtc.Context = partyContextPawn;
        partyPlayerContextNtc.Context.Base.MemberIndex = MemberIndex;
        return partyPlayerContextNtc;
    }

    public S2CContextGetPartyRentedPawnContextNtc GetS2CContextGetPartyRentedPawn_ContextNtc()
    {
        CDataPartyContextPawn partyContextPawn = new CDataPartyContextPawn();
        GameStructure.CDataContextBase(partyContextPawn.Base, Pawn);
        partyContextPawn.Base.PawnId = PawnId;
        partyContextPawn.Base.CharacterId = Pawn.CharacterId;
        partyContextPawn.Base.PawnType = Pawn.PawnType;
        partyContextPawn.Base.HmType = Pawn.HmType;
        GameStructure.CDataContextPlayerInfo(partyContextPawn.PlayerInfo, Pawn);
        partyContextPawn.PawnReactionList = Pawn.PawnReactionList;
        partyContextPawn.TrainingStatus = Pawn.TrainingStatus.GetValueOrDefault(Pawn.Job, new byte[64]);
        partyContextPawn.SpSkillList = Pawn.SpSkills.GetValueOrDefault(Pawn.Job, new List<CDataSpSkill>());
        GameStructure.CDataContextResist(partyContextPawn.ResistInfo, Pawn);
        partyContextPawn.EditInfo = Pawn.EditInfo;

        var contextNtc = new S2CContextGetPartyRentedPawnContextNtc();
        contextNtc.PawnId = PawnId;
        contextNtc.Context = partyContextPawn;
        contextNtc.Context.Base.MemberIndex = MemberIndex;
        return contextNtc;
    }
}
