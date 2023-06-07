using System;
using System.Linq;
using System.Collections.Generic;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.GameServer.Chat.Command.Commands
{
    public class JobCommand : ChatCommand
    {
        public override AccountStateType AccountState => AccountStateType.User;

        public override string Key => "job";
        public override string HelpText => "usage: `/job [job]`";

        private DdonGameServer _server;

        public JobCommand(DdonGameServer server)
        {
            _server = server;
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
                client.Character.Job = (JobId) job;

                _server.Database.UpdateCharacterCommonBaseInfo(client.Character);

                S2CJobChangeJobNtc notice = new S2CJobChangeJobNtc();
                notice.CharacterId = client.Character.CharacterId;
                notice.CharacterJobData = client.Character.ActiveCharacterJobData;
                notice.EquipItemInfo = client.Character.Equipment.getEquipmentAsCDataEquipItemInfo(client.Character.Job, EquipType.Performance)
                    .Union(client.Character.Equipment.getEquipmentAsCDataEquipItemInfo(client.Character.Job, EquipType.Visual))
                    .ToList();
                notice.SetAcquirementParamList = client.Character.CustomSkills
                    .Where(x => x.Job == job)
                    .Select(x => x.AsCDataSetAcquirementParam())
                    .ToList();
                notice.SetAbilityParamList = client.Character.Abilities
                    .Where(x => x.EquippedToJob == job)
                    .Select(x => x.AsCDataSetAcquirementParam())
                    .ToList();
                notice.LearnNormalSkillParamList = client.Character.NormalSkills
                    .Select(x => new CDataLearnNormalSkillParam(x))
                    .ToList();
                notice.EquipJobItemList = client.Character.CharacterEquipJobItemListDictionary[client.Character.Job];

                foreach(GameClient otherClient in _server.ClientLookup.GetAll())
                {
                    otherClient.Send(notice);
                }
            }
        }
    }
}