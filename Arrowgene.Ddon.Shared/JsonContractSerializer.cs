using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Xml;

namespace Arrowgene.Ddon.Shared
{
    public static class JsonContractSerializer
    {
        public static readonly DataContractJsonSerializerSettings Settings =
            new DataContractJsonSerializerSettings
            {
                UseSimpleDictionaryFormat = true
            };

        public static T Deserialize<T>(string json)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            T obj = default;
            try
            {
                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                {
                    DataContractJsonSerializer serializer =
                        new DataContractJsonSerializer(typeof(T), Settings);
                    obj = (T) serializer.ReadObject(stream);
                    stream.Close();
                    Thread.CurrentThread.CurrentCulture = currentCulture;
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.ToString());
                throw;
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = currentCulture;
            }

            return obj;
        }

        public static string Serialize<T>(T obj)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            string json = null;
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (XmlDictionaryWriter writer =
                        JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8, true, true, "  "))
                    {
                        DataContractJsonSerializer serializer =
                            new DataContractJsonSerializer(typeof(T), Settings);
                        serializer.WriteObject(writer, obj);
                        writer.Flush();
                    }

                    byte[] jsonBytes = stream.ToArray();
                    json = Encoding.UTF8.GetString(jsonBytes, 0, jsonBytes.Length);
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.ToString());
                throw;
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = currentCulture;
            }

            return json;
        }
    }
}
