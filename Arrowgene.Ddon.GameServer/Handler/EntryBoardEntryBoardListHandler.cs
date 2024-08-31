using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EntryBoardEntryBoardListHandler : GameRequestPacketHandler<C2SEntryBoardEntryBoardListReq, S2CEntryBoardEntryBoardListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EntryBoardEntryBoardListHandler));


        public EntryBoardEntryBoardListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CEntryBoardEntryBoardListRes Handle(GameClient client, C2SEntryBoardEntryBoardListReq request)
        {
            // var result = new S2CEntryBoardEntryBoardListRes.Serializer().Read(GameFull.Dump_709.AsBuffer());

            var result = new S2CEntryBoardEntryBoardListRes();
            foreach (var contentId in request.Unk0List.Select(x => x.Value).ToList())
            {
                if (Server.ExmManager.HasContentId(contentId))
                {
                    var data = Server.ExmManager.GetEntryItemDataForContent(contentId);
                    var contentParams = new CDataEntryBoardListParam()
                    {
                        EntryId = contentId,
                        SortieMin = 1,
                        NoPartyMembers = (ushort) data.EntryMemberList.Count,
                        TimeOut = 3600, // TODO: Figure this out from some config?
                    };
                    result.EntryList.Add(contentParams);
                }
            }

            return result;
        }
    }
}
