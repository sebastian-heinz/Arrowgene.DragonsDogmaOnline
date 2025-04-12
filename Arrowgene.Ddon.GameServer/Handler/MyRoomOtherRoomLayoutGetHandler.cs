using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MyRoomOtherRoomLayoutGetHandler : GameRequestPacketHandler<C2SMyRoomOtherRoomGetLayoutReq, S2CMyRoomOtherRoomLayoutGetRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MyRoomOtherRoomLayoutGetHandler));

        public MyRoomOtherRoomLayoutGetHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CMyRoomOtherRoomLayoutGetRes Handle(GameClient client, C2SMyRoomOtherRoomGetLayoutReq request)
        {
            var targetCharacter = Server.ClientLookup.GetClientByCharacterId(request.CharacterId)?.Character
                ?? throw new ResponseErrorException(Shared.Model.ErrorCode.ERROR_CODE_CHARACTER_DATA_INVALID_CHARACTER_ID);

            S2CMyRoomOtherRoomLayoutGetRes res = new()
            {
                CharacterId = request.CharacterId
            };

            var customizations = Server.Database.SelectMyRoomCustomization(targetCharacter.CharacterId);
            foreach (var requiredCustom in MyRoomFurnitureListGetHandler.RequiredFurniturePlacements)
            {
                if (!customizations.ContainsValue(requiredCustom.Value))
                {
                    customizations[requiredCustom.Key] = requiredCustom.Value;
                }
            }

            res.FurnitureList = customizations
                .Where(x => x.Value < MyRoomMyRoomBgmUpdateHandler.MYROOM_BGM_LAYOUTNO) // Filter out the pseudo-customizations we use for other handlers.
                .Select(x => new CDataFurnitureLayout()
                {
                    ItemID = x.Key,
                    LayoutID = x.Value
                })
                .ToList();
            res.BgmAcquirementNo = (uint)customizations.FirstOrDefault(x => x.Value == MyRoomMyRoomBgmUpdateHandler.MYROOM_BGM_LAYOUTNO).Key;
            res.ActivePlanetariumNo = (uint)customizations.FirstOrDefault(x => x.Value == MyRoomUpdatePlanetariumHandler.MYROOM_PLANETARIUM_LAYOUTNO).Key;
            res.PawnId = targetCharacter.PartnerPawnId;

            if (targetCharacter.PartnerPawnId > 0)
            {
                Pawn partnerPawn = targetCharacter.Pawns.Find(x => x.PawnId == targetCharacter.PartnerPawnId)
                    ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_MY_ROOM_NO_PARTNER,
                    $"Attempting to enter other room of character {targetCharacter.CharacterId}; couldn't find partner pawn {targetCharacter.PartnerPawnId}");
                GameStructure.CDataNoraPawnInfo(res.PawnInfo, partnerPawn, Server);
            }
            res.UnlockTerrace = targetCharacter.ContentsReleased.Contains(ContentsRelease.YourRoomsTerrace);

            return res;
        }
    }
}
