using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.GameServer.Utils;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EntryBoardEntryBoardItemCreateHandler : GameRequestPacketHandler<C2SEntryBoardEntryBoardItemCreateReq, S2CEntryBoardEntryBoardItemCreateRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EntryBoardEntryBoardItemCreateHandler));

        public EntryBoardEntryBoardItemCreateHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CEntryBoardEntryBoardItemCreateRes Handle(GameClient client, C2SEntryBoardEntryBoardItemCreateReq request)
        {
            // var pcap = new S2CEntryBoardEntryBoardItemCreateRes.Serializer().Read(GameFull.Dump_710.AsBuffer());
            var result = new S2CEntryBoardEntryBoardItemCreateRes()
            {
                BoardId = request.BoardId,
            };

            result.EntryItem.Param = request.CreateParam;
            result.EntryItem.PartyLeaderCharacterId = client.Character.CharacterId;
            result.EntryItem.TimeOut = 3600;
            result.EntryItem.Id = Random.Shared.NextU32();
            result.EntryItem.Param.MinEntryNum = 1;

            var member = new CDataEntryMemberData()
            {
                EntryFlag = false,
                Id = 1,
            };
            GameStructure.CDataCharacterListElement(member.CharacterListElement, client.Character);
            result.EntryItem.EntryMemberList.Add(member);

            for (int i = 2; i < request.CreateParam.MaxEntryNum + 1; i++)
            {
                result.EntryItem.EntryMemberList.Add(new CDataEntryMemberData()
                {
                    EntryFlag = false,
                    Id = (ushort) i
                });
            }

            // Everything went well, so store the state in the ExmManager
            Server.ExmManager.CreateGroupForContent(request.BoardId, result.EntryItem);
            Server.ExmManager.AddCharacterToContentGroup(request.BoardId, client.Character);

            S2CEntryBoardEntryBoardItemChangeMemberNtc ntc = new S2CEntryBoardEntryBoardItemChangeMemberNtc()
            {
                EntryFlag = true,
                MemberData = member,
            };
            client.Send(ntc);

            Server.CharacterManager.UpdateOnlineStatus(client, client.Character, OnlineStatus.EntryBoard);

            return result;
        }
     }
}
