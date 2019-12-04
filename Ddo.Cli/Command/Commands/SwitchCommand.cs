using System;
using System.Collections.Generic;
using System.Text;
using Ddo.Cli.Argument;

namespace Ddo.Cli.Command.Commands
{
    public class SwitchCommand : ConsoleCommand
    {
        private readonly List<ISwitchConsumer> _parameterConsumers;

        public SwitchCommand(List<ISwitchConsumer> parameterConsumers)
        {
            _parameterConsumers = parameterConsumers;
        }

        public override CommandResultType Handle(ConsoleParameter parameter)
        {
            foreach (string key in parameter.SwitchMap.Keys)
            {
                ISwitchProperty property = FindSwitch(key);
                if (property == null)
                {
                    Logger.Error($"Switch '{key}' not found");
                    continue;
                }

                string value = parameter.SwitchMap[key];
                if (!property.Assign(value))
                {
                    Logger.Error($"Switch '{key}' failed, value: '{value}' is invalid");
                }
                else
                {
                    Logger.Info($"Applied {key}={value}");
                }
            }
            
            foreach (string booleanSwitch in parameter.Switches)
            {
                ISwitchProperty property = FindSwitch(booleanSwitch);
                if (property == null)
                {
                    Logger.Error($"Switch '{booleanSwitch}' not found");
                    continue;
                }
                if (!property.Assign(bool.TrueString))
                {
                    Logger.Error($"Switch '{booleanSwitch}' failed, value: '{bool.TrueString}' is invalid");
                }
                else
                {
                    Logger.Info($"Applied {booleanSwitch}={bool.TrueString}");
                }
            }

            return CommandResultType.Completed;
        }

        private ISwitchProperty FindSwitch(string key)
        {
            foreach (ISwitchConsumer consumer in _parameterConsumers)
            {
                foreach (ISwitchProperty property in consumer.Switches)
                {
                    if (property.Key == key)
                    {
                        return property;
                    }
                }
            }

            return null;
        }

        private string ShowSwitches()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Available Switches:");
            sb.Append(Environment.NewLine);
            foreach (ISwitchConsumer consumer in _parameterConsumers)
            {
                foreach (ISwitchProperty property in consumer.Switches)
                {
                    sb.Append(Environment.NewLine);
                    sb.Append(property.Key);
                    sb.Append(Environment.NewLine);
                    sb.Append("> Ex.: ");
                    sb.Append(property.ValueDescription);
                    sb.Append(Environment.NewLine);
                    sb.Append("> ");
                    sb.Append(property.Description);
                    sb.Append(Environment.NewLine);
                }
            }

            return sb.ToString();
        }

        public override string Key => "switch";
        public override string Description => $"Changes configuration switches{Environment.NewLine}{ShowSwitches()}";
    }
}
