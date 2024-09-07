using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.IO;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EntryBoardEntryBoardItemReadyHandler : GameStructurePacketHandler<C2SEntryBoardEntryBoardItemReadyReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EntryBoardEntryBoardItemReadyHandler));

        public EntryBoardEntryBoardItemReadyHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SEntryBoardEntryBoardItemReadyReq> request)
        {
            var data = Server.ExmManager.GetEntryItemDataForCharacter(client.Character);
            var contentId = Server.ExmManager.GetContentIdForCharacter(client.Character);
            var members = Server.ExmManager.GetCharacterIdsForContent(contentId);

            var ntc = new S2CEntryBoardEntryBoardItemReserveNtc()
            {
                NowMember = (uint) members.Count,
                MaxMember = data.Param.MaxEntryNum,
            };
            client.Send(ntc);

            client.Send(new S2CEntryBoardEntryBoardItemReadyRes());

            PartyGroup party = Server.PartyManager.NewParty(contentId);

            S2CPartyPartyInviteAcceptNtc inviteAcceptNtc = new S2CPartyPartyInviteAcceptNtc();
            inviteAcceptNtc.ServerId = (ushort)Server.Id;
            inviteAcceptNtc.PartyId = party.Id;
            inviteAcceptNtc.StageId = client.Character.Stage.Id;
            inviteAcceptNtc.PositionId = 0;
            inviteAcceptNtc.MemberIndex = 0;
            client.Send(inviteAcceptNtc);
        }
    }
}
