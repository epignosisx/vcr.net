using SharpYaml.Serialization;
using System.Collections.Generic;
using System.Net.Http;

namespace Vcr
{
    public class HttpRequest
    {
        [YamlMember(1)]
        public string Uri { get; set; }

        [YamlMember(2)]
        public string Method { get; set; }

        [YamlMember(3)]
        public Dictionary<string, List<string>> Headers { get; set; } = new Dictionary<string, List<string>>();

        [YamlMember(4)]
        public HttpBody Body { get; set; }
        
        public static HttpRequest Create(HttpRequestMessage httpRequestMessage)
        {
            var request = new HttpRequest();
            request.Method = httpRequestMessage.Method.ToString();
            request.Uri = httpRequestMessage.RequestUri.AbsoluteUri;

            foreach (var header in httpRequestMessage.Headers)
            {
                if (!request.Headers.TryGetValue(header.Key, out List<string> values))
                {
                    values = new List<string>();
                    request.Headers.Add(header.Key, values);
                }
                values.AddRange(header.Value);
            }

            if (httpRequestMessage.Content != null)
            {
                request.Body = new HttpBody();

                //TODO: verify if this fallback logic should be the other way around.
                request.Body.Encoding = httpRequestMessage.Content.Headers.ContentType.CharSet ?? string.Join(", ", httpRequestMessage.Content.Headers.ContentEncoding);
                
                //TODO: serialize to base64 if content type is not string based.
                request.Body.String = httpRequestMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult(); // ok, since we called 'LoadIntoBufferAsync'.

                foreach (var header in httpRequestMessage.Content.Headers)
                {
                    if (!request.Headers.TryGetValue(header.Key, out List<string> values))
                    {
                        values = new List<string>();
                        request.Headers.Add(header.Key, values);
                    }
                    values.AddRange(header.Value);
                }
            }

            return request;
        }

        public HttpRequestMessage ToHttpRequestMessage()
        {
            var httpRequestMessage = new HttpRequestMessage(new HttpMethod(Method), Uri);
            if (Body != null)
                httpRequestMessage.Content = new StringContent(Body.String);

            foreach(var header in Headers)
            {
                if (!httpRequestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value) && httpRequestMessage.Content != null)
                    httpRequestMessage.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return httpRequestMessage;
        }
    }
}
