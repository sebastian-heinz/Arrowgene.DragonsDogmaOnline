using System.Collections.Generic;

namespace Ddo.Cli.Argument
{
    public interface ISwitchConsumer
    {
        List<ISwitchProperty> Switches { get; }
    }
}
