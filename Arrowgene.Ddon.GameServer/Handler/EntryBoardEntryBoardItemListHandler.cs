using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Diagnostics.Metrics;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EntryBoardEntryBoardItemListHandler : GameRequestPacketHandler<C2SEntryBoardEntryBoardItemListReq, S2CEntryBoardEntryBoardItemListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EntryBoardEntryBoardItemListHandler));

        public EntryBoardEntryBoardItemListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CEntryBoardEntryBoardItemListRes Handle(GameClient client, C2SEntryBoardEntryBoardItemListReq request)
        {
            var result = new S2CEntryBoardEntryBoardItemListRes()
            {
                BoardId = request.BoardId,
            };

            // TODO: Implement friend, clan, party and group search
            // TODO: Implement more complex item rank searches
            // TODO: Implement complex job searches

            var groups = Server.BoardManager.GetGroupsForBoardId(request.BoardId);
            foreach (var group in groups)
            {
                if (group.ContentStatus != ContentStatus.Recruiting)
                {
                    // Skip groups that are currently playing
                    continue;
                }

                if (request.SearchParameter.IsNoPassword && group.Password != "")
                {
                    continue;
                }

                if (request.SearchParameter.RequiredItemRankMin > 0 &&
                    (group.EntryItem.Param.RequiredItemRank < request.SearchParameter.RequiredItemRankMin ||
                     group.EntryItem.Param.RequiredItemRank > request.SearchParameter.RequiredItemRankMax))
                {
                    continue;
                }

                if (request.SearchParameter.RankMin > 0 &&
                    (group.EntryItem.Param.TopEntryJobLevel < request.SearchParameter.RankMin ||
                    group.EntryItem.Param.BottomEntryJobLevel> request.SearchParameter.RankMax))
                {
                    continue;
                }

                if (request.SearchParameter.FirstName != "" || request.SearchParameter.LastName != "")
                {
                    bool foundFirst = false;
                    bool foundSecond = false;
                    foreach (var info in group.EntryItem.EntryMemberList)
                    {
                        foundFirst = false;
                        foundSecond = false;

                        if (!info.EntryFlag)
                        {
                            // Slot was not filled
                            continue;
                        }

                        var characterName = info.CharacterListElement.CommunityCharacterBaseInfo.CharacterName;
                        if (request.SearchParameter.FirstName != "" && characterName.FirstName == request.SearchParameter.FirstName)
                        {
                            foundFirst = true;
                        }

                        if (request.SearchParameter.FirstName != "" && characterName.FirstName == request.SearchParameter.FirstName)
                        {
                            foundSecond = true;
                        }

                        if (foundFirst || foundSecond)
                        {
                            break;
                        }
                    }

                    if (!foundFirst && !foundSecond)
                    {
                        continue;
                    }
                }

                result.EntryItemList.Add(group.EntryItem);

                if (result.EntryItemList.Count >= request.Num)
                {
                    break;
                }
            }

            return result;
        }
    }
}
