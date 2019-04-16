using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Vcr.Test
{
    public class CassetteTest
    {
        private readonly VCR _vcr;

        public CassetteTest()
        {
            _vcr = new VCR();
            _vcr.Storage = new MemoryStorage();
        }

        [Fact]
        public async Task RecordsThenPlaybacks()
        {
            using (_vcr.UseCassette("something", RecordMode.Once))
            {
                var vcrHandler = _vcr.GetVcrHandler();
                var httpClient = CreateHttpClient(vcrHandler);
                var response = await httpClient.GetAsync("http://portquiz.net");
            }

            using (_vcr.UseCassette("something", RecordMode.None))
            {
                var vcrHandler = _vcr.GetVcrHandler();
                var httpClient = CreateHttpClient(vcrHandler);
                var response = await httpClient.GetAsync("http://portquiz.net");
            }

            Assert.True(true);
        }

        [Fact]
        public async Task RecordsEveryTime()
        {
            using (_vcr.UseCassette("something", RecordMode.All))
            {
                var vcrHandler = _vcr.GetVcrHandler();
                var httpClient = CreateHttpClient(vcrHandler);
                await httpClient.GetAsync("http://portquiz.net");
                await httpClient.GetAsync("http://portquiz.net");

                Assert.Equal(2, _vcr.Cassette.HttpInteractions.Count);
            }
        }

        [Fact]
        public async Task ThrowsWhenPlaybackRequestedButNothingIsRecorded()
        {
            using (_vcr.UseCassette("something", RecordMode.None))
            {
                var vcrHandler = _vcr.GetVcrHandler();
                var httpClient = CreateHttpClient(vcrHandler);

                await Assert.ThrowsAsync<Exception>(() => httpClient.GetAsync("http://portquiz.net"));
            }
        }

        [Fact]
        public async Task RecordsNewEpisodesOnly()
        {
            // Record one episode
            using (_vcr.UseCassette("something", RecordMode.Once))
            {
                var vcrHandler = _vcr.GetVcrHandler();
                var httpClient = CreateHttpClient(vcrHandler);
                await httpClient.GetAsync("http://portquiz.net");
            }

            // Replay
            using (_vcr.UseCassette("something", RecordMode.NewEpisodes))
            {
                var vcrHandler = _vcr.GetVcrHandler();
                var httpClient = CreateHttpClient(vcrHandler);
                await httpClient.GetAsync("http://portquiz.net");
                Assert.Single(_vcr.Cassette.HttpInteractions);
            }

            // Record new episode
            using (_vcr.UseCassette("something", RecordMode.NewEpisodes))
            {
                var vcrHandler = _vcr.GetVcrHandler();
                var httpClient = CreateHttpClient(vcrHandler);
                await httpClient.GetAsync("http://portquiz.net:8080");
                Assert.Equal(2, _vcr.Cassette.HttpInteractions.Count);
            }
        }

        private static HttpClient CreateHttpClient(DelegatingHandler vcrHandler)
        {
            vcrHandler.InnerHandler = new HttpClientHandler
            {
                UseCookies = false
            };

            return new HttpClient(vcrHandler);
        }
    }
}
