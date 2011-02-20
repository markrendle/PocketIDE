using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using Microsoft.ServiceModel.Description;
using Microsoft.ServiceModel.Http;

namespace PocketIDE.Web
{
    public class CodeConfiguration : HttpHostConfiguration, IProcessorProvider, IInstanceFactory
    {
        private readonly CompositionContainer _container;

        public CodeConfiguration(CompositionContainer container)
        {
            _container = container;
        }

        public void RegisterRequestProcessorsForOperation(HttpOperationDescription operation,
                                                          IList<Processor> processors, MediaTypeProcessorMode mode)
        {
            processors.Add(new JsonProcessor(operation, mode));
        }

        public void RegisterResponseProcessorsForOperation(HttpOperationDescription operation,
                                                           IList<Processor> processors, MediaTypeProcessorMode mode)
        {
            processors.Add(new JsonProcessor(operation, mode));
        }

        // Get the instance from MEF
        public object GetInstance(Type serviceType, InstanceContext instanceContext, Message message)
        {
            string contract = AttributedModelServices.GetContractName(serviceType);
            string identity = AttributedModelServices.GetTypeIdentity(serviceType);

            // force non-shared so that every service doesn't need to have a [PartCreationPolicy] attribute.
            var definition = new ContractBasedImportDefinition(contract, identity, null, ImportCardinality.ExactlyOne,
                                                               false, false, CreationPolicy.NonShared);
            return _container.GetExports(definition).First().Value;
        }

        public void ReleaseInstance(InstanceContext instanceContext, object service)
        {
            // no op
        }
    }
}