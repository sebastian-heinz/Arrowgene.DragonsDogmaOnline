using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arrowgene.Ddon.Shared.Entity;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.TypeInspectors;

namespace Arrowgene.Ddon.Shared.Network
{
    public class StructurePacket<TStruct> : StructurePacket, IStructurePacket<TStruct> where TStruct : class, IPacketStructure, new()
    {
        private TStruct _structure;

        public StructurePacket(IPacket packet) : base(packet)
        {
        }

        public StructurePacket(TStruct structure) : base(structure.Id, null)
        {
            SetStructure(structure);
        }
        
        public TStruct Structure
        {
            get => GetStructure();
            set => SetStructure(value);
        }

        private void SetStructure(TStruct structure)
        {
            Data = EntitySerializer.Get<TStruct>().Write(structure);
            _structure = structure;
        }

        private TStruct GetStructure()
        {
            if (_structure == null)
            {
                _structure = EntitySerializer.Get<TStruct>().Read(AsBuffer());
            }

            return _structure;
        }
    }

    public abstract class StructurePacket : Packet, IStructurePacket
    {
        public static ISerializer YamlSerializer { get; } = new SerializerBuilder()
            .WithTypeInspector(inspector => new MyTypeInspector(inspector))
            .WithTypeConverter(new ByteArrayConverter())
            .Build();

        public static ISerializer JsonSerializer { get; } = new SerializerBuilder()
            .WithTypeInspector(inspector => new MyTypeInspector(inspector))
            .WithTypeConverter(new ByteArrayConverter())
            .JsonCompatible()
            .Build();

        protected StructurePacket(IPacket packet) : base(packet.Id, packet.Data, packet.Source, packet.Count)
        {
        }

        protected StructurePacket(PacketId id, byte[] data) : base(id, data)
        {
        }
        
        public string PrintStructure()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(PrintHeader());
            stringBuilder.Append(Environment.NewLine);
            YamlSerializer.Serialize(new IndentedTextWriter(new StringWriter(stringBuilder)), this);
            return stringBuilder.ToString();
        }

        private class MyTypeInspector : TypeInspectorSkeleton
        {
            private readonly ITypeInspector _innerTypeDescriptor;

            public MyTypeInspector(ITypeInspector innerTypeDescriptor)
            {
                _innerTypeDescriptor = innerTypeDescriptor;
            }

            public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object container)
            {
                IEnumerable<IPropertyDescriptor> props = _innerTypeDescriptor.GetProperties(type, container);
                List<IPropertyDescriptor> filtered = new List<IPropertyDescriptor>();
                IPropertyDescriptor structure = null;
                bool hasData = false;
                bool hasCount = false;
                foreach (IPropertyDescriptor prop in props)
                {
                    if (prop.Name == "Structure")
                    {
                        structure = prop;
                    }

                    if (prop.Name == "Data")
                    {
                        hasData = true;
                    }

                    if (prop.Name == "Count")
                    {
                        hasCount = true;
                    }

                    if (prop.Type == typeof(PacketId) && prop.Name == "Id")
                    {
                        // exclude PacketIds from serialisation
                        continue;
                    }

                    filtered.Add(prop);
                }

                if (structure != null && hasData && hasCount)
                {
                    // assuming this is the top level of StructurePacket.Structure,
                    // in this case only serialize the `.Structure` property.
                    filtered.Clear();
                    filtered.Add(structure);
                }

                return filtered;
            }
        }
    }

    public class ByteArrayConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return type == typeof(byte[]);
        }

        public object ReadYaml(IParser parser, Type type)
        {
            var scalar = (YamlDotNet.Core.Events.Scalar)parser.Current;
            var bytes = Util.FromHexString(scalar.Value);
            parser.MoveNext();
            return bytes;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            var bytes = (byte[])value;
            emitter.Emit(new YamlDotNet.Core.Events.Scalar(
                null,
                "tag:yaml.org,2002:binary",
                Util.ToHexString(bytes),
                ScalarStyle.Plain,
                false,
                false
            ));
        }
    }
}
