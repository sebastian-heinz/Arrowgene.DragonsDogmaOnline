using System.Collections.Generic;

namespace Ddo.Server.Common.Instance
{
    /// <summary>
    /// Provides Unique Ids for instancing.
    /// </summary>
    public class InstanceGenerator
    {
        private readonly object _lock;
        private uint _currentId;

        private readonly Dictionary<uint, IInstance> _instances;

        public InstanceGenerator()
        {
            _lock = new object();
            _currentId = 0;
            _instances = new Dictionary<uint, IInstance>();
        }

        public void AssignInstance(IInstance instance)
        {
            uint id;
            lock (_lock)
            {
                id = _currentId;
                _currentId++;
            }

            _instances.Add(id, instance);
            instance.InstanceId = id;
        }

        public T CreateInstance<T>() where T : IInstance, new()
        {
            uint id;
            lock (_lock)
            {
                id = _currentId;
                _currentId++;
            }

            T instance = new T();
            _instances.Add(id, instance);
            instance.InstanceId = id;
            return instance;
        }

        public IInstance GetInstance(uint id)
        {
            if (!_instances.ContainsKey(id))
            {
                return null;
            }

            return _instances[id];
        }
    }
}
