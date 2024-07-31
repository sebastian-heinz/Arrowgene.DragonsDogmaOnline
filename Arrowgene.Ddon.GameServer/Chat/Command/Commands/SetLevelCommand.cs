using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Chat.Command.Commands
{
    public class SetLevelCommand : ChatCommand
    {
        public override AccountStateType AccountState => AccountStateType.User;

        public override string Key => "setlevel";
        public override string HelpText => "usage: `/setlevel [jobid] [level]` - Set job level.";

        private DdonGameServer _server;

        public SetLevelCommand(DdonGameServer server)
        {
            _server = server;
        }

        public override void Execute(string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
        {
            uint itemId = 0;
            if (command.Length == 0)
            {
                responses.Add(ChatResponse.CommandError(client, "No arguments provided."));
                return;
            }

            JobId? targetJob = null;

            // Try by id
            if (Byte.TryParse(command[0], out byte parsedJobId))
            {
                targetJob = (JobId)parsedJobId;
            }
            else
            {
                string commandJobName = command[0].ToUpper();
                foreach (string jobName in Enum.GetNames(typeof(JobId)))
                {
                    if (jobName.ToUpper() == commandJobName)
                    {
                        targetJob = (JobId)Enum.Parse(typeof(JobId), jobName);
                        break;
                    }
                }
            }

            if (targetJob == null)
            {
                responses.Add(ChatResponse.CommandError(client, "invalid job, try one of the following (job id or name):"));
                responses.Add(ChatResponse.CommandError(client, "1-Fighter"));
                responses.Add(ChatResponse.CommandError(client, "2-Seeker"));
                responses.Add(ChatResponse.CommandError(client, "3-Hunter"));
                responses.Add(ChatResponse.CommandError(client, "4-Priest"));
                responses.Add(ChatResponse.CommandError(client, "5-ShieldSage"));
                responses.Add(ChatResponse.CommandError(client, "6-Sorcerer"));
                responses.Add(ChatResponse.CommandError(client, "7-Warrior"));
                responses.Add(ChatResponse.CommandError(client, "8-ElementArcher"));
                responses.Add(ChatResponse.CommandError(client, "9-Alchemist"));
                responses.Add(ChatResponse.CommandError(client, "10-SpiritLancer"));
                responses.Add(ChatResponse.CommandError(client, "11-HighScepter"));
                return;
            }

            uint targetLevel = 1;
            if (command.Length >= 2)
            {
                if (UInt32.TryParse(command[1], out uint parsedLevel))
                {
                    targetLevel = parsedLevel;
                }
                else
                {
                    responses.Add(ChatResponse.CommandError(client, $"Invalid level \"{command[1]}\". It must be a number"));
                    return;
                }
            }


            CharacterCommon targetCharacter = client.Character;
            CDataCharacterJobData? targetJobData = targetCharacter.CharacterJobDataList.Where(x => x.Job == targetJob).SingleOrDefault();

            if (targetJobData is null) 
            {
                responses.Add(ChatResponse.CommandError(client, "Missing CDataCharacterJobData. Something went wrong."));
                return;
            }

            targetJobData.Lv = targetLevel;
            targetJobData.JobPoint = 50000;
            targetJobData.Exp = 0;

            CDataCharacterJobData newStats = ExpManager.CalculateBaseStats((JobId)targetJob, targetLevel);
            targetJobData.Atk = newStats.Atk;
            targetJobData.MAtk = newStats.MAtk;
            targetJobData.Def = newStats.Def;
            targetJobData.MDef = newStats.MDef;

            if (targetCharacter.ActiveCharacterJobData.Job == targetJob)
            {
                S2CJobCharacterJobLevelUpNtc lvlNtc = new S2CJobCharacterJobLevelUpNtc();
                lvlNtc.Job = (JobId)targetJob;
                lvlNtc.Level = targetLevel;
                lvlNtc.AddJobPoint = 0;
                lvlNtc.TotalJobPoint = 50000;
                GameStructure.CDataCharacterLevelParam(lvlNtc.CharacterLevelParam, (Character)targetCharacter);
                client.Send(lvlNtc);
            }

            _server.Database.UpdateCharacterJobData(targetCharacter.CommonId, targetJobData);

            responses.Add(ChatResponse.ServerMessage(client, $"Setting {targetJob.ToString()} to level {targetLevel}."));
        }
    }
}
