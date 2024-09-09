using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Chat.Command.Commands
{
    public class RepopCommand : ChatCommand
    {
        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "repop";
        public override string HelpText => "usage: `/repop StageId LayerNo GroupId SubGroupId PositionIndex (WaitSeconds)` - The enemy in the CSV should have RepopCount > 0`";

        private DdonGameServer Server;

        public RepopCommand(DdonGameServer server) {
            Server = server;
        }

        public override void Execute(string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
        {
            if (command.Length < 5)
            {
                responses.Add(ChatResponse.CommandError(client, "no arguments provided"));
                return;
            }

            try
            {
                uint stageNo = uint.Parse(command[0]);
                byte layerNo = byte.Parse(command[1]);
                uint groupId = uint.Parse(command[2]);
                byte subGroupId = byte.Parse(command[3]);
                byte positionIndex = byte.Parse(command[4]);
                uint waitSeconds = 0;

                if (command.Length > 5)
                {
                    waitSeconds = uint.Parse(command[5]);
                }


                CDataStageLayoutId enemyGroup = new CDataStageLayoutId() //client.Character.EnemyGroupsEntered.First();
                {
                    StageId = stageNo,
                    LayerNo = layerNo,
                    GroupId = groupId
                };
                List<InstancedEnemy> enemySpawns = client.Party.InstanceEnemyManager.GetAssets(enemyGroup)
                    .Where(x => x.Subgroup == subGroupId)
                    .ToList();

                if (enemySpawns.Count <= positionIndex)
                {
                    responses.Add(ChatResponse.CommandError(client, "no enemies to repopulate in this enemy group and position index"));
                    return;
                }

                S2CInstanceEnemyRepopNtc ntc = new S2CInstanceEnemyRepopNtc();
                ntc.LayoutId = enemyGroup;
                ntc.EnemyData.PositionIndex = positionIndex;
                ntc.EnemyData.EnemyInfo = enemySpawns[positionIndex].asCDataStageLayoutEnemyPresetEnemyInfoClient();
                ntc.WaitSecond = waitSeconds;
                client.Party.SendToAll(ntc);

                responses.Add(ChatResponse.ServerMessage(client, "Command Executed"));
            }
            catch(Exception)
            {
                responses.Add(ChatResponse.CommandError(client, "invalid arguments"));
            }
        }
    }
}
