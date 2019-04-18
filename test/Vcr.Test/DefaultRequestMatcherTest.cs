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
        [InlineData("utf-8", "body", "utf-16", "body", false)]
        [InlineData("utf-8", "body", "utf-8", "body1", false)]
        [InlineData("utf-8", "body", "utf-8", "body", true)]
        public void CompareBody(string recordedEncoding, string recordedBody, string encoding, string body, bool found)
        {
            //arrange
            var matcher = new DefaultRequestMatcher();
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
    }
}
