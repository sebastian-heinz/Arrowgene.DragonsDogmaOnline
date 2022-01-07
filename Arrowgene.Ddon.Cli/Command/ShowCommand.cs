using System;
using System.Text;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Cli.Command
{
    public class ShowCommand : ICommand
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(ShowCommand));

        public string Key => "show";
        public string Description => $"Shows Copyright. Ex.:{Environment.NewLine}show w{Environment.NewLine}show c";

        public CommandResultType Run(CommandParameter parameter)
        {
            if (parameter.Arguments.Contains("w"))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(Environment.NewLine);
                sb.Append("15. Disclaimer of Warranty.");
                sb.Append(Environment.NewLine);
                sb.Append("THERE IS NO WARRANTY FOR THE PROGRAM, TO THE EXTENT PERMITTED BY");
                sb.Append(Environment.NewLine);
                sb.Append("APPLICABLE LAW. EXCEPT WHEN OTHERWISE STATED IN WRITING THE COPYRIGHT");
                sb.Append(Environment.NewLine);
                sb.Append("HOLDERS AND/OR OTHER PARTIES PROVIDE THE PROGRAM \"AS IS\" WITHOUT WARRANTY");
                sb.Append(Environment.NewLine);
                sb.Append("OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING, BUT NOT LIMITED TO,");
                sb.Append(Environment.NewLine);
                sb.Append("THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR");
                sb.Append(Environment.NewLine);
                sb.Append("PURPOSE.  THE ENTIRE RISK AS TO THE QUALITY AND PERFORMANCE OF THE PROGRAM");
                sb.Append(Environment.NewLine);
                sb.Append("IS WITH YOU.  SHOULD THE PROGRAM PROVE DEFECTIVE, YOU ASSUME THE COST OF");
                sb.Append(Environment.NewLine);
                sb.Append("ALL NECESSARY SERVICING, REPAIR OR CORRECTION.");
                sb.Append(Environment.NewLine);
                Logger.Info(sb.ToString());
                return CommandResultType.Completed;
            }

            if (parameter.Arguments.Contains("c"))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(Environment.NewLine);
                sb.Append("2. Basic Permissions.");
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append("All rights granted under this License are granted for the term of");
                sb.Append(Environment.NewLine);
                sb.Append("copyright on the Program, and are irrevocable provided the stated");
                sb.Append(Environment.NewLine);
                sb.Append("conditions are met. This License explicitly affirms your unlimited");
                sb.Append(Environment.NewLine);
                sb.Append("permission to run the unmodified Program. The output from running a");
                sb.Append(Environment.NewLine);
                sb.Append("covered work is covered by this License only if the output, given its");
                sb.Append(Environment.NewLine);
                sb.Append("content, constitutes a covered work. This License acknowledges your");
                sb.Append(Environment.NewLine);
                sb.Append("rights of fair use or other equivalent, as provided by copyright law.");
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append("You may make, run and propagate covered works that you do not");
                sb.Append(Environment.NewLine);
                sb.Append("convey, without conditions so long as your license otherwise remains");
                sb.Append(Environment.NewLine);
                sb.Append("in force.  You may convey covered works to others for the sole purpose");
                sb.Append(Environment.NewLine);
                sb.Append("of having them make modifications exclusively for you, or provide you");
                sb.Append(Environment.NewLine);
                sb.Append("with facilities for running those works, provided that you comply with");
                sb.Append(Environment.NewLine);
                sb.Append("the terms of this License in conveying all material for which you do");
                sb.Append(Environment.NewLine);
                sb.Append("not control copyright. Those thus making or running the covered works");
                sb.Append(Environment.NewLine);
                sb.Append("for you must do so exclusively on your behalf, under your direction");
                sb.Append(Environment.NewLine);
                sb.Append("and control, on terms that prohibit them from making any copies of");
                sb.Append(Environment.NewLine);
                sb.Append("your copyrighted material outside their relationship with you.");
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append("Conveying under any other circumstances is permitted solely under");
                sb.Append(Environment.NewLine);
                sb.Append("the conditions stated below. Sublicensing is not allowed; section 10");
                sb.Append(Environment.NewLine);
                sb.Append("makes it unnecessary.");
                sb.Append(Environment.NewLine);
                Logger.Info(sb.ToString());
                return CommandResultType.Completed;
            }

            return CommandResultType.Continue;
        }
        public void Shutdown()
        {
        }
    }
}
