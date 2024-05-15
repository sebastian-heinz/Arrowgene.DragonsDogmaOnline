using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Instance
{
    public class OmManager
    {

        public static void SetOmData(Dictionary<uint, Dictionary<ulong, uint>> InstanceOmData, uint stageId, ulong key, uint value)
        {
            lock (InstanceOmData)
            {
                if (!InstanceOmData.ContainsKey(stageId))
                {
                    InstanceOmData[stageId] = new Dictionary<ulong, uint>();
                }

                InstanceOmData[stageId][key] = value;
            }
        }

        public static uint GetOmData(Dictionary<uint, Dictionary<ulong, uint>> InstanceOmData, uint stageId, ulong key)
        {
            uint result = 0;
            lock (InstanceOmData)
            {
                if (!InstanceOmData.ContainsKey(stageId) || !InstanceOmData[stageId].ContainsKey(key))
                {
                    result = 0;
                }
                else
                {
                    result = InstanceOmData[stageId][key];
                }
            }

            return result;
        }

        public static Dictionary<ulong, uint> GetAllOmData(Dictionary<uint, Dictionary<ulong, uint>> InstanceOmData, uint stageId)
        {
            Dictionary<ulong, uint> result = new Dictionary<ulong, uint>();
            lock (InstanceOmData)
            {
                if (InstanceOmData.ContainsKey(stageId))
                {
                    // Make a shallow clone to return (dict is only integral types)
                    result = new Dictionary<ulong, uint>(InstanceOmData[stageId]);
                }
            }
            return result;
        }

        public static uint ExchangeOmData(Dictionary<uint, Dictionary<ulong, uint>> InstanceOmData, uint stageId, ulong key, uint value)
        {
            uint oldValue = 0;
            lock (InstanceOmData)
            {
                if (InstanceOmData.ContainsKey(stageId) && InstanceOmData[stageId].ContainsKey(key))
                {
                    oldValue = InstanceOmData[stageId][key];
                }

                if (!InstanceOmData.ContainsKey(stageId))
                {
                    InstanceOmData[stageId] = new Dictionary<ulong, uint>();
                }

                InstanceOmData[stageId][key] = value;
            }
            return oldValue;
        }

        public static void ClearOmData(Dictionary<uint, Dictionary<ulong, uint>> InstanceOmData, uint stageId, ulong key)
        {
            lock (InstanceOmData)
            {
                if (InstanceOmData.ContainsKey(stageId) && InstanceOmData[stageId].ContainsKey(key))
                {
                    InstanceOmData[stageId].Remove(key);
                }
            }
        }

        public static void ClearAllOmData(Dictionary<uint, Dictionary<ulong, uint>> InstanceOmData, uint stageId)
        {
            lock (InstanceOmData)
            {
                if (InstanceOmData.ContainsKey(stageId))
                {
                    InstanceOmData.Remove(stageId);
                }
            }
        }

        public static void ResetAllOmData(Dictionary<uint, Dictionary<ulong, uint>> InstanceOmData)
        {
            lock (InstanceOmData)
            {
                InstanceOmData.Clear();
            }
        }
    }
}
