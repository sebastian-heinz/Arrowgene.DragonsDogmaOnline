using System.Collections.Generic;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Chat.Command.Commands
{
    /// <summary>
    /// Example Command Handler
    /// </summary>
    public class EnemyCommand : ChatCommand
    {
        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "enemy";
        public override string HelpText => "usage: `/enemy id`";

        public override void Execute(string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
        {
            if (command.Length <= 0)
            {
                responses.Add(ChatResponse.CommandError(client, "no arguments provided"));
                return;
            }

            //    S2CInstanceGetEnemySetListRes response = new S2CInstanceGetEnemySetListRes();
            //    response.LayoutId  = client.Stage;
            //    response.SubGroupId = 1;
            //    response.RandomSeed = 0xFFFFFFFF;
            //      CDataLayoutEnemyData layoutEnemyData = new CDataLayoutEnemyData();
            //      layoutEnemyData.EnemyInfo.EnemyId = 0x010400;
            //      layoutEnemyData.EnemyInfo.NamedEnemyParamsId = 0x8FA;
            //      layoutEnemyData.EnemyInfo.Scale = 100;
            //      layoutEnemyData.EnemyInfo.Lv = 50;
            //      layoutEnemyData.EnemyInfo.EnemyTargetTypesId = 1;
            //      response.EnemyList.Add(layoutEnemyData);


            S2CInstanceEnemyRepopNtc ntc = new S2CInstanceEnemyRepopNtc();
            ntc.EnemyData.PositionIndex = 21;
            ntc.EnemyData.EnemyInfo.Lv = 33;
            ntc.EnemyData.EnemyInfo.EnemyId = 0x010315;
            ntc.EnemyData.EnemyInfo.NamedEnemyParamsId = 0x8FA;
            ntc.EnemyData.EnemyInfo.Scale = 100;
            ntc.EnemyData.EnemyInfo.EnemyTargetTypesId = 1;

            //  layoutEnemyData.EnemyInfo.EnemyId = 0x010314;
            //  layoutEnemyData.EnemyInfo.NamedEnemyParamsId = 0x8FA;
            //  layoutEnemyData.EnemyInfo.Scale = 100;
            //  layoutEnemyData.EnemyInfo.Lv = 94;
            //  layoutEnemyData.EnemyInfo.EnemyTargetTypesId = 1;
            //  response.EnemyList.Add(layoutEnemyData);

            //    ntc.LayoutId = client.Stage;
            //    client.Send(ntc);


            responses.Add(ChatResponse.ServerMessage(client, "Command Executed"));
        }
    }
}
