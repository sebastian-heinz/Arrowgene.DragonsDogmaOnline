using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Ddo.Server.Common;

namespace Ddo.Server.Web
{
    public class WebRequest
    {
        public static WebRequestMethod ParseMethod(string method)
        {
            method = method.ToLowerInvariant();
            switch (method)
            {
                case "get": return WebRequestMethod.Get;
                case "put": return WebRequestMethod.Put;
                case "post": return WebRequestMethod.Post;
                case "delete": return WebRequestMethod.Delete;
            }

            return WebRequestMethod.Unknown;
        }

        public WebRequestMethod Method { get; set; }
        public string Host { get; set; }
        public int? Port { get; set; }
        public string Path { get; set; }
        public string Scheme { get; set; }
        public string ContentType { get; set; }
        public string QueryString { get; set; }
        public long? ContentLength { get; set; }
        [XmlIgnore] public Stream Body { get; set; }
        public WebCollection<string, string> Header { get; }
        public WebCollection<string, string> QueryParameter { get; }
        public WebCollection<string, string> Cookies { get; }

        public WebRequest()
        {
            Path = null;
            Body = new MemoryStream();
            Header = new WebCollection<string, string>(key => key.ToLowerInvariant());
            Cookies = new WebCollection<string, string>(key => key.ToLowerInvariant());
            QueryParameter = new WebCollection<string, string>(key => key.ToLowerInvariant());
        }

        /// <summary>
        /// Clears the body.
        /// </summary>
        public void ClearBody()
        {
            Body.Position = 0;
            Body.SetLength(0);
        }

        /// <summary>
        /// Writes give bytes from current position till length of bytes.
        /// </summary>
        public Task WriteAsync(byte[] bytes)
        {
            return WriteAsync(bytes, Body.Position);
        }

        /// <summary>
        /// Writes give bytes from given position till length of bytes.
        /// </summary>
        public Task WriteAsync(byte[] bytes, long position)
        {
            Body.Position = position;
            return Body.WriteAsync(bytes, 0, bytes.Length);
        }

        public Task<byte[]> ReadBytesAsync()
        {
            Body.Position = 0;
            return Util.ReadAsync(Body);
        }

        public Task<string> ReadStringAsync()
        {
            return ReadStringAsync(Encoding.UTF8);
        }

        public async Task<string> ReadStringAsync(Encoding encoding)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            Body.Position = 0;
            byte[] body = await Util.ReadAsync(Body);
            return encoding.GetString(body);
        }

        public override string ToString()
        {
            return $"{Method} {Scheme}://{Host}:{Port?.ToString()}{Path}{QueryString}";
        }
    }
}
