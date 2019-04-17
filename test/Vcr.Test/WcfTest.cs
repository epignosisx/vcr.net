using System;
using System.Collections.Generic;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using SampleSoap;
using Xunit;

namespace Vcr.Test
{
    public class WcfTest
    {
        private readonly VCR _vcr;

        public WcfTest()
        {
            _vcr = new VCR();
            _vcr.Storage = new MemoryStorage();
        }

        [Fact]
        public async Task RecordsThenPlaybacks()
        {
            using (_vcr.UseCassette("wcf", RecordMode.Once))
            {
                var client = new SoapResponderPortTypeClient(new BasicHttpBinding(), new EndpointAddress("http://www.soapclient.com/xml/soapresponder.wsdl"));
                client.Endpoint.EndpointBehaviors.Add(new VcrBehavior(_vcr));
                var response = await client.Method1Async("1", "2");
            }

            using (_vcr.UseCassette("wcf", RecordMode.None))
            {
                var client = new SoapResponderPortTypeClient(new BasicHttpBinding(), new EndpointAddress("http://www.soapclient.com/xml/soapresponder.wsdl"));
                client.Endpoint.EndpointBehaviors.Add(new VcrBehavior(_vcr));
                var response = await client.Method1Async("1", "2");
            }

            Assert.True(true);
        }
    }


    public class VcrBehavior : IEndpointBehavior
    {
        private readonly VCR _vcr;

        public VcrBehavior(VCR vcr)
        {
            _vcr = vcr ?? throw new ArgumentNullException(nameof(vcr));
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            bindingParameters.Add(new Func<HttpClientHandler, HttpMessageHandler>(GetHttpMessageHandler));
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        public HttpMessageHandler GetHttpMessageHandler(HttpClientHandler httpClientHandler)
        {
            var handler = _vcr.GetVcrHandler();
            handler.InnerHandler = httpClientHandler;
            return handler;
        }
    }
}
