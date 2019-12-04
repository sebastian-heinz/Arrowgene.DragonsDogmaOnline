namespace Ddo.Cli.Argument
{
    public class SwitchProperty<T> : ISwitchProperty
    {
        public static TryParseHandler NoOp = (string value, out T result) =>
        {
            result = default;
            return true;
        };

        private TryParseHandler _parser;
        private AssignHandler _assigner;

        public SwitchProperty(string key, string valueDescription, string description, TryParseHandler parser,
            AssignHandler assigner)
        {
            Key = key;
            ValueDescription = valueDescription;
            Description = description;
            _parser = parser;
            _assigner = assigner;
        }

        public delegate bool TryParseHandler(string value, out T result);

        public delegate void AssignHandler(T result);

        public string Key { get; }
        public string Description { get; }
        public string ValueDescription { get; }

        public bool Assign(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            if (!_parser(value, out T result))
            {
                return false;
            }

            _assigner(result);
            return true;
        }
    }
}
