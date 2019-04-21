using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Vcr.Test
{
    public class DefaultRequestMatcherTest
    {
        [Theory]
        [InlineData("https://example.com", "https://example.com", true)]
        [InlineData("https://example.com/abc", "https://example.com", false)]
        public void CompareUri(string recordedUri, string uri, bool found)
        {
            //arrange
            var matcher = new DefaultRequestMatcher();
            var recordedRequest = new HttpRequest { Uri = recordedUri };
            var request = new HttpRequest { Uri = uri };

            //act
            var result = matcher.FindMatch(new[] { new HttpInteraction { Request = recordedRequest } }, request);

            //assert
            Assert.Equal(found, result != null);
        }

        [Theory]
        [InlineData("GET", "GET", true)]
        [InlineData("GET", "get", false)]
        [InlineData("GET", "POST", false)]
        public void CompareHttpMethod(string recordedMethod, string method, bool found)
        {
            //arrange
            var matcher = new DefaultRequestMatcher();
            var recordedRequest = new HttpRequest { Uri = "https://example.com", Method = recordedMethod };
            var request = new HttpRequest { Uri = "https://example.com", Method = method };

            //act
            var result = matcher.FindMatch(new[] { new HttpInteraction { Request = recordedRequest } }, request);

            //assert
            Assert.Equal(found, result != null);
        }

        [Theory]
        //Compare = true
        [InlineData("utf-8", "body", "utf-16", "body", true, false)]
        [InlineData("utf-8", "body", "utf-8", "body1", true, false)]
        [InlineData("utf-8", "body", "utf-8", "body", true, true)]
        //Compare = false
        [InlineData("utf-8", "body", "utf-16", "body", false, true)]
        [InlineData("utf-8", "body", "utf-8", "body1", false, true)]
        [InlineData("utf-8", "body", "utf-8", "body", false, true)]
        public void CompareBody(string recordedEncoding, string recordedBody, string encoding, string body, bool compare, bool found)
        {
            //arrange
            var matcher = new DefaultRequestMatcher() { CompareBody = compare };
            var recordedRequest = new HttpRequest {
                Uri = "https://example.com",
                Method = "POST",
                Body = new HttpBody {
                    Encoding = recordedEncoding,
                    String = recordedBody
                }
            };
            var request = new HttpRequest {
                Uri = "https://example.com",
                Method = "POST",
                Body = new HttpBody {
                    Encoding = encoding,
                    String = body
                }
            };

            //act
            var result = matcher.FindMatch(new[] { new HttpInteraction { Request = recordedRequest } }, request);

            //assert
            Assert.Equal(found, result != null);
        }

        [Theory]
        [MemberData(nameof(HeaderData))]
        public void CompareHeaders(Dictionary<string, List<string>> recordedHeaders, Dictionary<string, List<string>> headers, bool compare, bool found)
        {
            //arrange
            var matcher = new DefaultRequestMatcher() { CompareHeaders = compare };
            var recordedRequest = new HttpRequest
            {
                Uri = "https://example.com",
                Method = "GET",
                Headers = recordedHeaders
            };
            var request = new HttpRequest
            {
                Uri = "https://example.com",
                Method = "GET",
                Headers = headers
            };

            //act
            var result = matcher.FindMatch(new[] { new HttpInteraction { Request = recordedRequest } }, request);

            //assert
            Assert.Equal(found, result != null);
        }

        public static IEnumerable<object[]> HeaderData()
        {
            return new List<object[]> {
                new object[]{
                    new Dictionary<string, List<string>> { ["SomeHeader"] = new List<string> { "SomeValue" } },
                    new Dictionary<string, List<string>> { ["SomeHeader"] = new List<string> { "SomeValue" } },
                    true,
                    true
                },
                new object[]{
                    new Dictionary<string, List<string>> { ["SomeHeader"] = new List<string> { "SomeValue", "OtherValue" } },
                    new Dictionary<string, List<string>> { ["SomeHeader"] = new List<string> { "SomeValue", "OtherValue" } },
                    true,
                    true
                },
                new object[]{
                    new Dictionary<string, List<string>> { ["SomeHeader"] = new List<string> { "SomeValue" } },
                    new Dictionary<string, List<string>> { ["SomeHeader"] = new List<string> { "OtherValue" } },
                    true,
                    false
                },
                new object[]{
                    new Dictionary<string, List<string>> { ["SomeHeader"] = new List<string> { "SomeValue" } },
                    new Dictionary<string, List<string>> { ["SomeHeader"] = new List<string> {  } },
                    true,
                    false
                },
                new object[]{
                    new Dictionary<string, List<string>> { ["SomeHeader"] = new List<string> { "SomeValue" } },
                    new Dictionary<string, List<string>> { ["SomeHeader"] = new List<string> { "SomeValue", "OtherValue" } },
                    true,
                    false
                },
                new object[]{
                    new Dictionary<string, List<string>> { ["SomeHeader"] = new List<string> { "SomeValue", "OtherValue" } },
                    new Dictionary<string, List<string>> { ["SomeHeader"] = new List<string> { "SomeValue" } },
                    true,
                    false
                },
                new object[]{
                    new Dictionary<string, List<string>> { ["SomeHeader"] = new List<string> { "SomeValue", "OtherValue" } },
                    new Dictionary<string, List<string>> { ["SomeHeader"] = new List<string> { "OtherValue", "SomeValue" } },
                    true,
                    false
                }
            };
        }
    }
}
