using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using YamlDotNet.Serialization;

namespace Vcr
{
    public class HttpResponse
    {
        [YamlMember(Order = 1)]
        public HttpStatus Status { get; set; }

        [YamlMember(Order = 2)]
        public Dictionary<string, List<string>> Headers { get; set; } = new Dictionary<string, List<string>>();

        [YamlMember(Order = 3)]
        public HttpBody Body { get; set; }

        [YamlMember(Order = 4)]
        public string HttpVersion { get; set; }

        public static HttpResponse Create(HttpResponseMessage httpResponseMessage)
        {
            var response = new HttpResponse();
            response.Status = new HttpStatus
            {
                Code = (int)httpResponseMessage.StatusCode,
                Message = httpResponseMessage.ReasonPhrase,
            };

            response.HttpVersion = httpResponseMessage.Version?.ToString();

            foreach (var header in httpResponseMessage.Headers)
            {
                if (!response.Headers.TryGetValue(header.Key, out List<string> values))
                {
                    values = new List<string>();
                    response.Headers.Add(header.Key, values);
                }
                values.AddRange(header.Value);
            }

            if (httpResponseMessage.Content != null)
            {
                response.Body = new HttpBody();

                //TODO: verify if this fallback logic should be the other way around.
                response.Body.Encoding = httpResponseMessage.Content.Headers.ContentType?.CharSet ?? string.Join(", ", httpResponseMessage.Content.Headers.ContentEncoding);

                if (httpResponseMessage.Content.Headers.ContentType?.CharSet == "\"utf-8\"")
                    httpResponseMessage.Content.Headers.ContentType.CharSet = "utf-8";

                //TODO: serialize to base64 if content type is not string based.
                response.Body.String = httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult(); // ok, since we called 'LoadIntoBufferAsync'.

                foreach (var header in httpResponseMessage.Content.Headers)
                {
                    if (!response.Headers.TryGetValue(header.Key, out List<string> values))
                    {
                        values = new List<string>();
                        response.Headers.Add(header.Key, values);
                    }
                    values.AddRange(header.Value);
                }
            }

            return response;
        }

        public HttpResponseMessage ToHttpRequestMessage(HttpRequestMessage request)
        {
            var httpResponseMessage = new HttpResponseMessage((HttpStatusCode)Status.Code);
            httpResponseMessage.ReasonPhrase = Status.Message;
            httpResponseMessage.RequestMessage = request;

            if (Body != null)
            {
                httpResponseMessage.Content = new StringContent(Body.String);
                httpResponseMessage.Content.Headers.Remove("Content-Type");//StringContent adds text/plain
            }

            foreach (var header in Headers)
            {
                if (!httpResponseMessage.Headers.TryAddWithoutValidation(header.Key, header.Value) && httpResponseMessage.Content != null)
                    httpResponseMessage.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return httpResponseMessage;
        }
    }
}
