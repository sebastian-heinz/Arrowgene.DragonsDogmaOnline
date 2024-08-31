using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterCharacterPenaltyReviveHandler : GameRequestPacketHandler<C2SCharacterCharacterPenaltyReviveReq, S2CCharacterCharacterPenaltyReviveRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterCharacterPenaltyReviveHandler));

        // Same time as it was in the original server
        private static readonly TimeSpan WeaknessTimeSpan = TimeSpan.FromMinutes(20);

        public CharacterCharacterPenaltyReviveHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCharacterCharacterPenaltyReviveRes Handle(GameClient client, C2SCharacterCharacterPenaltyReviveReq packet)
        {
            S2CCharacterCharacterPenaltyReviveRes res = new S2CCharacterCharacterPenaltyReviveRes();

            if (client.GameMode != GameMode.BitterblackMaze)
            {
                // Weakness
                client.Send(new S2CCharacterStartDeathPenaltyNtc()
                {
                    RemainTime = (uint)WeaknessTimeSpan.Seconds
                });

                // Restore after time passes
                Task.Delay(WeaknessTimeSpan).ContinueWith(_ => client.Send(new S2CCharacterFinishDeathPenaltyNtc()));
            }
            else if (client.GameMode == GameMode.BitterblackMaze)
            {
                // The player will lose all items in their bag when homepoints after a death (equipped items stay)
                List<CDataItemUpdateResult> updateItemList = new List<CDataItemUpdateResult>();
                Server.Database.ExecuteInTransaction(connection =>
                {
                    updateItemList = Server.ItemManager.RemoveAllItemsFromInventory(client.Character, client.Character.Storage, ItemManager.ItemBagStorageTypes, connection);
                });

                // Flush Storage
                S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc()
                {
                    UpdateType = ItemNoticeType.SwitchingStorage,
                    UpdateItemList = updateItemList
                };
                client.Send(updateCharacterItemNtc);
            }

            return res;
        }
    }
}
