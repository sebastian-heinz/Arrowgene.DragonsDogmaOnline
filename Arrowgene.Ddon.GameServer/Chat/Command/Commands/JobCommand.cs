using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer.Handler;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Chat.Command.Commands
{
    public class JobCommand : ChatCommand
    {
        public override AccountStateType AccountState => AccountStateType.User;

        public override string Key => "job";
        public override string HelpText => "usage: `/job [job]`";

        private JobChangeJobHandler _jobChangeHandler;

        public JobCommand(DdonGameServer server)
        {
            _jobChangeHandler = new JobChangeJobHandler(server);
        }

        public override void Execute(string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
        {
            if (command.Length <= 0)
            {
                // check expected length before accessing
                responses.Add(ChatResponse.CommandError(client, "no arguments provided"));
                return;
            }

            JobId? job = null;

            // Try by id
            if(Byte.TryParse(command[0], out byte parsedJobId))
            {
                job = (JobId) parsedJobId;
            }
            else
            {
                string commandJobName = string.Concat(command).ToUpper();
                foreach(string jobName in Enum.GetNames(typeof(JobId)))
                {
                    if(jobName.ToUpper() == commandJobName)
                    {
                        job = (JobId) Enum.Parse(typeof(JobId), jobName);
                        break;
                    }
                }
            }

            if(job == null)
            {
                responses.Add(ChatResponse.CommandError(client, "invalid job, try one of the following (job id or name):"));
                responses.Add(ChatResponse.CommandError(client, "1-Fighter"));
                responses.Add(ChatResponse.CommandError(client, "2-Seeker"));
                responses.Add(ChatResponse.CommandError(client, "3-Hunter"));
                responses.Add(ChatResponse.CommandError(client, "4-Priest"));
                responses.Add(ChatResponse.CommandError(client, "5-Shield Sage"));
                responses.Add(ChatResponse.CommandError(client, "6-Sorcerer"));
                responses.Add(ChatResponse.CommandError(client, "7-Warrior"));
                responses.Add(ChatResponse.CommandError(client, "8-Element Archer"));
                responses.Add(ChatResponse.CommandError(client, "9-Alchemist"));
                responses.Add(ChatResponse.CommandError(client, "10-Spirit Lancer"));
                responses.Add(ChatResponse.CommandError(client, "11-High Scepter"));
            }
            else if(!client.Character.CharacterJobDataList.Exists(x => x.Job == (JobId) job))
            {
                responses.Add(ChatResponse.CommandError(client, $"This character has no data for the job {Enum.GetName(typeof(JobId), job)}"));
            }
            else
            {
                _jobChangeHandler.Handle(client,
                    new StructurePacket<C2SJobChangeJobReq>(
                        new C2SJobChangeJobReq()
                        {
                            JobId = (JobId)job
                        }));
            }
        }
    }
}
