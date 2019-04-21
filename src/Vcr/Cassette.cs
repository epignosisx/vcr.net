using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Vcr
{
    public class Cassette : IDisposable
    {
        private readonly ICasseteStorage _storage;
        private readonly string _name;
        private readonly RecordMode _recordMode;
        private readonly bool _isNew;
        private readonly IRequestMatcher _requestMatcher;
        private readonly List<HttpInteraction> _httpInteractions;

        internal Cassette(ICasseteStorage storage, string name, RecordMode recordMode, IRequestMatcher requestMatcher)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _requestMatcher = requestMatcher ?? throw new ArgumentNullException(nameof(requestMatcher));
            _recordMode = recordMode;

            var httpInteractions = storage.Load(name);
            if (httpInteractions == null)
            {
                _isNew = true;
                _httpInteractions = new List<HttpInteraction>();
            }
            else
            {
                _isNew = false;
                _httpInteractions = httpInteractions;
            }
        }

        internal List<HttpInteraction> HttpInteractions => _httpInteractions;

        public void Dispose()
        {
            Save();
        }

        private void Save()
        {
            _storage.Save(_name, _httpInteractions);
        }

        internal Task<HttpResponseMessage> HandleRequestAsync(HttpCallAsync call, HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_recordMode == RecordMode.None)
                return PlaybackAsync(request);

            if (_recordMode == RecordMode.Once)
                return RecordOnceAsync(call, request, cancellationToken);

            if (_recordMode == RecordMode.NewEpisodes)
                return RecordNewAsync(call, request, cancellationToken);

            if (_recordMode == RecordMode.All)
                return RecordAsync(call, request, cancellationToken);

            throw new InvalidOperationException("Cassette record mode unknown: " + _recordMode.ToString());
        }

        private Task<HttpResponseMessage> PlaybackAsync(HttpRequestMessage request)
        {
            var match = _requestMatcher.FindMatch(_httpInteractions, HttpRequest.Create(request));
            if (match == null)
                throw new Exception("No matching request found"); //TODO: throw custom exception with more details.

            return ProcessMatchAsync(match);
        }

        private async Task<HttpResponseMessage> RecordAsync(HttpCallAsync call, HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Content != null)
                await request.Content.LoadIntoBufferAsync();

            //HttpRequest must be created before passing to the rest of the HttpClient middleware
            //since additional handlers can modify the request, ie. add headers which we will not have
            //when replaying and trying to mach.
            var httpRequest = HttpRequest.Create(request);
            var response = await call(request, cancellationToken);

            if (response.Content != null)
                await response.Content.LoadIntoBufferAsync();

            _httpInteractions.Add(new HttpInteraction { Request = httpRequest, Response = HttpResponse.Create(response) });
            return response;
        }

        private Task<HttpResponseMessage> RecordNewAsync(HttpCallAsync call, HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var match = _requestMatcher.FindMatch(_httpInteractions, HttpRequest.Create(request));
            if (match == null)
                return RecordAsync(call, request, cancellationToken);

            return ProcessMatchAsync(match);
        }

        private Task<HttpResponseMessage> RecordOnceAsync(HttpCallAsync call, HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_isNew)
                return RecordAsync(call, request, cancellationToken);

            var match = _requestMatcher.FindMatch(_httpInteractions, HttpRequest.Create(request));
            if (match != null)
                return ProcessMatchAsync(match);

            throw new Exception("No matching request found"); //TODO: throw custom exception with more details.
        }

        private Task<HttpResponseMessage> ProcessMatchAsync(HttpInteraction match)
        {
            match.Played = true;
            var httpRequestMessage = match.Request.ToHttpRequestMessage();
            var httpResponseMessage = match.Response.ToHttpRequestMessage(httpRequestMessage);
            return Task.FromResult(httpResponseMessage);
        }

    }
}
