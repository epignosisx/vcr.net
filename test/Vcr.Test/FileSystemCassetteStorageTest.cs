using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xunit;

namespace Vcr.Test
{
    public class FileSystemCassetteStorageTest : IDisposable
    {
        private readonly DirectoryInfo _dir;

        public FileSystemCassetteStorageTest()
        {
            _dir = new DirectoryInfo($"./{Guid.NewGuid()}");
        }

        public void Dispose()
        {
            try
            {
                _dir.Refresh();
                _dir.Delete();
            }
            catch
            {
            }
        }

        [Fact]
        public void CreatesDirectoryIfNew()
        {
            //arrange + act
            var sut = new FileSystemCassetteStorage(_dir);
            _dir.Refresh();

            //assert
            Assert.True(_dir.Exists);
        }


        [Fact]
        public void RoundtripCassette()
        {
            //arrange
            var sut = new FileSystemCassetteStorage(_dir);
            var list = new List<HttpInteraction> {
                new HttpInteraction {
                    Request = CreateHttpRequestMessage(),
                    Response = CreateHttpResponseMessage()
                }
            };

            //act
            sut.Save("a cassette", list);

            //assert
            Assert.Single(_dir.GetFiles());

            //act
            var newList = sut.Load("a cassette");
            Assert.Single(newList);
            Assert.False(newList[0].Played);
            Assert.NotNull(newList[0].Response);
            Assert.NotNull(newList[0].Response);
        }

        private static HttpRequestMessage CreateHttpRequestMessage()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "http://example.com");
            request.Headers.Accept.ParseAdd("application/json");
            request.Headers.Date = DateTime.Now;
            request.Headers.Connection.Add("Keep-Alive");
            request.Content = new StringContent("{\"name\": \"jimmy\"}", Encoding.UTF8, "application/json");
            return request;
        }

        private static HttpResponseMessage CreateHttpResponseMessage()
        {
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Version = new Version(1, 1);
            response.Headers.Date = DateTime.Now;
            response.Headers.CacheControl = new CacheControlHeaderValue();
            response.Headers.CacheControl.Public = true;
            response.Headers.CacheControl.MaxAge = TimeSpan.FromMinutes(5);
            response.Content = new StringContent("{\"status\": \"accepted\"}", Encoding.UTF8, "application/json");
            return response;
        }
    }
}
