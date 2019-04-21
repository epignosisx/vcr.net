using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Vcr.Test
{
    public class CassetteTest
    {
        private readonly VCR _vcr;
        private const string s_testUrl = "http://portquiz.net";
        private const string s_secondaryTestUrl = "http://portquiz.net:8080";

        public CassetteTest()
        {
            _vcr = new VCR(new MemoryStorage());
        }

        [Fact]
        public async Task RecordsOnce_RecordsThenPlaybacks()
        {
            using (_vcr.UseCassette("something", RecordMode.Once))
            {
                var vcrHandler = _vcr.GetVcrHandler();
                var httpClient = CreateHttpClient(vcrHandler);
                var response = await httpClient.GetAsync(s_testUrl);
            }

            using (_vcr.UseCassette("something", RecordMode.None))
            {
                var vcrHandler = _vcr.GetVcrHandler();
                var httpClient = CreateHttpClient(vcrHandler);
                var response = await httpClient.GetAsync(s_testUrl);
            }

            Assert.True(true);
        }

        [Fact]
        public async Task RecordsOnce_ThrowsOnUnmatchedRequestDuringPlayback()
        {
            using (_vcr.UseCassette("something", RecordMode.Once))
            {
                var vcrHandler = _vcr.GetVcrHandler();
                var httpClient = CreateHttpClient(vcrHandler);
                var response = await httpClient.GetAsync(s_testUrl);
            }

            using (_vcr.UseCassette("something", RecordMode.None))
            {
                var vcrHandler = _vcr.GetVcrHandler();
                var httpClient = CreateHttpClient(vcrHandler);
                await Assert.ThrowsAsync<Exception>(() => httpClient.GetAsync(s_secondaryTestUrl));
            }
        }

        [Fact]
        public async Task RecordsOnce_RecordsSameRequestMultipleTimes()
        {
            //arrange
            using (_vcr.UseCassette("something", RecordMode.Once))
            {
                var vcrHandler = _vcr.GetVcrHandler();
                var httpClient = CreateHttpClient(vcrHandler);

                //act
                var response = await httpClient.GetAsync(s_testUrl);
                response = await httpClient.GetAsync(s_testUrl);

                //arrange
                Assert.Equal(2, _vcr.Cassette.HttpInteractions.Count);
            }
        }

        [Fact]
        public async Task RecordsAll_RecordsEveryTime()
        {
            using (_vcr.UseCassette("something", RecordMode.All))
            {
                var vcrHandler = _vcr.GetVcrHandler();
                var httpClient = CreateHttpClient(vcrHandler);
                await httpClient.GetAsync(s_testUrl);
                await httpClient.GetAsync(s_testUrl);

                Assert.Equal(2, _vcr.Cassette.HttpInteractions.Count);
            }
        }

        [Fact]
        public async Task RecordsNone_ThrowsWhenPlaybackRequestedButNothingIsRecorded()
        {
            using (_vcr.UseCassette("something", RecordMode.None))
            {
                var vcrHandler = _vcr.GetVcrHandler();
                var httpClient = CreateHttpClient(vcrHandler);

                await Assert.ThrowsAsync<Exception>(() => httpClient.GetAsync(s_testUrl));
            }
        }

        [Fact]
        public async Task RecordsNewEpisodes_RecordsOnlyNew()
        {
            // Record one episode
            using (_vcr.UseCassette("something", RecordMode.Once))
            {
                var vcrHandler = _vcr.GetVcrHandler();
                var httpClient = CreateHttpClient(vcrHandler);
                await httpClient.GetAsync(s_testUrl);
            }

            // Replay
            using (_vcr.UseCassette("something", RecordMode.NewEpisodes))
            {
                var vcrHandler = _vcr.GetVcrHandler();
                var httpClient = CreateHttpClient(vcrHandler);
                await httpClient.GetAsync(s_testUrl);
                Assert.Single(_vcr.Cassette.HttpInteractions);
            }

            // Record new episode
            using (_vcr.UseCassette("something", RecordMode.NewEpisodes))
            {
                var vcrHandler = _vcr.GetVcrHandler();
                var httpClient = CreateHttpClient(vcrHandler);
                await httpClient.GetAsync(s_secondaryTestUrl);
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
