﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     //
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SampleSoap
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.SoapClient.com/xml/SoapResponder.wsdl", ConfigurationName="SampleSoap.SoapResponderPortType")]
    public interface SoapResponderPortType
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.SoapClient.com/SoapObject", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        System.Threading.Tasks.Task<SampleSoap.Method1Response> Method1Async(SampleSoap.Method1Request request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Method1", WrapperNamespace="http://www.SoapClient.com/xml/SoapResponder.xsd", IsWrapped=true)]
    public partial class Method1Request
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public string bstrParam1;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=1)]
        public string bstrParam2;
        
        public Method1Request()
        {
        }
        
        public Method1Request(string bstrParam1, string bstrParam2)
        {
            this.bstrParam1 = bstrParam1;
            this.bstrParam2 = bstrParam2;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Method1Response", WrapperNamespace="http://www.SoapClient.com/xml/SoapResponder.xsd", IsWrapped=true)]
    public partial class Method1Response
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public string bstrReturn;
        
        public Method1Response()
        {
        }
        
        public Method1Response(string bstrReturn)
        {
            this.bstrReturn = bstrReturn;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    public interface SoapResponderPortTypeChannel : SampleSoap.SoapResponderPortType, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    public partial class SoapResponderPortTypeClient : System.ServiceModel.ClientBase<SampleSoap.SoapResponderPortType>, SampleSoap.SoapResponderPortType
    {
        
    /// <summary>
    /// Implement this partial method to configure the service endpoint.
    /// </summary>
    /// <param name="serviceEndpoint">The endpoint to configure</param>
    /// <param name="clientCredentials">The client credentials</param>
    static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public SoapResponderPortTypeClient() : 
                base(SoapResponderPortTypeClient.GetDefaultBinding(), SoapResponderPortTypeClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.SoapResponderPortType.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public SoapResponderPortTypeClient(EndpointConfiguration endpointConfiguration) : 
                base(SoapResponderPortTypeClient.GetBindingForEndpoint(endpointConfiguration), SoapResponderPortTypeClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public SoapResponderPortTypeClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(SoapResponderPortTypeClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public SoapResponderPortTypeClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(SoapResponderPortTypeClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public SoapResponderPortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<SampleSoap.Method1Response> SampleSoap.SoapResponderPortType.Method1Async(SampleSoap.Method1Request request)
        {
            return base.Channel.Method1Async(request);
        }
        
        public System.Threading.Tasks.Task<SampleSoap.Method1Response> Method1Async(string bstrParam1, string bstrParam2)
        {
            SampleSoap.Method1Request inValue = new SampleSoap.Method1Request();
            inValue.bstrParam1 = bstrParam1;
            inValue.bstrParam2 = bstrParam2;
            return ((SampleSoap.SoapResponderPortType)(this)).Method1Async(inValue);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.SoapResponderPortType))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.SoapResponderPortType))
            {
                return new System.ServiceModel.EndpointAddress("http://www.soapclient.com/xml/soapresponder.wsdl");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return SoapResponderPortTypeClient.GetBindingForEndpoint(EndpointConfiguration.SoapResponderPortType);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return SoapResponderPortTypeClient.GetEndpointAddress(EndpointConfiguration.SoapResponderPortType);
        }
        
        public enum EndpointConfiguration
        {
            
            SoapResponderPortType,
        }
    }
}