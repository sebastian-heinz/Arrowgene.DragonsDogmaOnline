using Arrowgene.Ddon.Server;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Utils
{
    // If using dotnet 7+ we can use instead:
    // where T : IBinaryInteger<T> so the pool can be all numeric types
    public class UniqueIdPool
    {
        private uint Counter;
        private Stack<uint> FreeIdStack;

        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(UniqueIdPool));

        public UniqueIdPool(uint defaultValue = 1)
        {
            Counter = defaultValue;
            FreeIdStack = new Stack<uint>();
            FreeIdStack.Push(Counter);
        }

        public uint GenerateId()
        {
            lock (FreeIdStack)
            {
                if (FreeIdStack.Count == 0)
                {
                    Counter = Counter + 1;
                    FreeIdStack.Push(Counter);
                }
                return FreeIdStack.Pop();
            }
        }

        public void ReclaimId(uint id)
        {
            lock (FreeIdStack)
            {
                FreeIdStack.Push(id);
            }
        }
    }
}
