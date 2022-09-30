using System.Threading;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyJoinHandler : GameStructurePacketHandler<C2SPartyPartyJoinReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyPartyJoinHandler));

        public PartyPartyJoinHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPartyPartyJoinReq> packet)
        {
            PartyGroup party = Server.PartyManager.GetParty(packet.Structure.PartyId);
            if (party == null)
            {
                Logger.Error(client, "Could not join party, does not exist");
                // todo return error
                return;
            }

            if (!party.Join(client))
            {
                Logger.Error(client, "Could not join party, its full");
                // todo return error
                return;
            }

            client.PendingInvitedParty = null;

            S2CPartyPartyJoinRes response = new S2CPartyPartyJoinRes();
            response.PartyId = party.Id;
            client.Send(response);

            // Send updated member list to all party members
            S2CPartyPartyJoinNtc partyJoinNtc = new S2CPartyPartyJoinNtc();
            partyJoinNtc.HostCharacterId = party.Host.Character.Id;
            partyJoinNtc.LeaderCharacterId = party.Leader.Character.Id;
            
            // Send party player context NTCs to the new member
            foreach (Character character in party.Characters)
            {
                CDataPartyMember partyMember = new CDataPartyMember();
                partyMember.CharacterListElement.ServerId = character.Server.Id;
                partyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterId = character.Id;
                partyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterName.FirstName =
                    character.FirstName;
                partyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterName.LastName = character.LastName;
                partyMember.CharacterListElement.CurrentJobBaseInfo.Job = character.Job;
                partyMember.CharacterListElement.CurrentJobBaseInfo.Level = (byte)character.ActiveCharacterJobData.Lv;
                partyMember.CharacterListElement.OnlineStatus = character.OnlineStatus;
                partyMember.CharacterListElement.unk2 = 1;
                partyMember.MemberType = party.GetMemberType(character);
                partyMember.MemberIndex = party.GetSlotIndex(character);
             //   partyMember.PawnId = character.Id; // TODO pawn.ID from DB 
                partyMember.IsLeader = character.Id == party.Leader.Character.Id;
            //    partyMember.IsPawn = partyMember.MemberType == 2;
             //   partyMember.IsPlayEntry = false;
                partyMember.JoinState = JoinState.On;
         //       partyMember.AnyValueList = new byte[] { 0x0, 0xDA, 0x5D, 0x4E, 0x0, 0x1, 0x0, 0x2 }; // Taken from pcaps
           //    partyMember.SessionStatus = 0;

                partyJoinNtc.PartyMembers.Add(partyMember);
            }

            party.SendToAll(partyJoinNtc);
            

            // Send party player context NTCs to the new member
            foreach (Character character in party.Characters)
            {
                
                int memberType = party.GetMemberType(character);
                if (memberType == 1)
                {
                    CDataPartyPlayerContext partyPlayerContext = new CDataPartyPlayerContext();
                    partyPlayerContext.Base = new CDataContextBase(character);
                    partyPlayerContext.PlayerInfo = new CDataContextPlayerInfo(character);
                    partyPlayerContext.ResistInfo = new CDataContextResist(character);
                    partyPlayerContext.EditInfo = character.EditInfo;
                    
                    S2CContextGetPartyPlayerContextNtc partyPlayerContextNtc = new S2CContextGetPartyPlayerContextNtc();
                    partyPlayerContextNtc.CharacterId = character.Id;
                    partyPlayerContextNtc.Context = partyPlayerContext;
                    partyPlayerContextNtc.Context.Base.MemberIndex = party.GetSlotIndex(character);
                    party.SendToAll(partyPlayerContextNtc);
                }

                if (memberType == 2)
                {
                    Pawn pawn = party.GetPawn((uint)party.GetSlotIndex(character));
                    
                    CDataPartyContextPawn partyContextPawn = new CDataPartyContextPawn();
                    partyContextPawn.Base = new CDataContextBase(character);
                    partyContextPawn.Base.PawnId = character.Id;
                    partyContextPawn.Base.CharacterId = character.Id;
                    partyContextPawn.Base.PawnType = pawn.PawnType;
                    partyContextPawn.Base.HmType = pawn.HmType;
                    partyContextPawn.PlayerInfo = new CDataContextPlayerInfo(character);
                    partyContextPawn.PawnReactionList = pawn.PawnReactionList;
                    partyContextPawn.Unk0 = new byte[64];
                    partyContextPawn.SpSkillList = pawn.SpSkillList;
                    partyContextPawn.ResistInfo = new CDataContextResist(character);
                    partyContextPawn.EditInfo = character.EditInfo;
                    
                    S2CContextGetPartyMypawnContextNtc partyPlayerContextNtc = new S2CContextGetPartyMypawnContextNtc();
                    partyPlayerContextNtc.PawnId = character.Id;
                    partyPlayerContextNtc.Context = partyContextPawn;
                    partyPlayerContextNtc.Context.Base.MemberIndex = party.GetSlotIndex(character);
                    party.SendToAll(partyPlayerContextNtc);
                }
                
                // TODO only new member or all ?
            }
        }
    }
}
