using Arrowgene.Ddon.Bot.Action;

namespace Arrowgene.Ddon.Bot;

public class BotActionParser
{
    public BotActionParser()
    {
    }

    public BotAction? Parse(string input)
    {
        string[] action = input.Split("|");
        switch (action[0])
        {
            case "login": return new Login(action[1]);
        }

        return null;
    }
}
