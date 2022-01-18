using System;
using Arrowgene.Logging;
using Xunit.Abstractions;

namespace Arrowgene.Ddon.Test
{
    public class TestBase : IDisposable
    {
        private ITestOutputHelper _output;

        public TestBase(ITestOutputHelper output)
        {
            _output = output;
#if (DEBUG)
            LogProvider.Start();
            LogProvider.OnLogWrite += (sender, args) => { _output.WriteLine(args.Log.ToString()); };
#endif
        }

        public void Output(string text)
        {
            _output.WriteLine(text);
        }

        public void Dispose()
        {
#if (DEBUG)
            LogProvider.Stop();
#endif
        }
    }
}
