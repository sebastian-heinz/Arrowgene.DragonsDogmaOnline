#load "libs.csx"

using System.Collections.Generic;

public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.Admin;
    public override string CommandName => "godmode";
    public override string HelpText => "usage: `/godmode [true/false]` - Toggle high stats.";

    private uint BoostedStat = 10000;

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        bool toggle = true;
        if (command.Length > 0 && bool.TryParse(command[0], out bool parsedBool))
        {
            toggle = parsedBool;
        }

        if (toggle)
        {
            client.Send(new S2CCharacterGainCharacterParamNtc()
            {
                CharacterId = client.Character.CharacterId,
                HpMaxGain = BoostedStat,
                StaminaMaxGain = BoostedStat,
                AttackGain = BoostedStat,
                DefenseGain = BoostedStat,
                MagicAttackGain = BoostedStat,
                MagicDefenseGain = BoostedStat
            });
            responses.Add(ChatResponse.ServerMessage(client, "Godmode enabled."));
        }
        else
        {
            client.Send(new S2CCharacterGainCharacterParamNtc()
            {
                CharacterId = client.Character.CharacterId,
                HpMaxGain = client.Character.ExtendedParams.HpMax,
                StaminaMaxGain = client.Character.ExtendedParams.StaminaMax,
                AttackGain = client.Character.ExtendedParams.Attack,
                DefenseGain = client.Character.ExtendedParams.Defence,
                MagicAttackGain = client.Character.ExtendedParams.MagicAttack,
                MagicDefenseGain = client.Character.ExtendedParams.MagicDefence
            });
            responses.Add(ChatResponse.ServerMessage(client, "Godmode disabled."));
        }
    }
}

return new ChatCommand();
