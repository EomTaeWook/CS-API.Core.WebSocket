using System;
using System.Collections.Generic;
using System.Text;
using API.Core.WebSocket.Hubs.Descriptor;

namespace API.Core.WebSocket.Hubs
{
    public class DefaultHubActivator : IHubActivator
    {
        private readonly IDependencyResolver _resolver;

        public DefaultHubActivator(IDependencyResolver resolver)
        {
            _resolver = resolver;
        }

        public IHub Create(HubDescriptor descriptor)
        {
            if (descriptor == null)
            {
                throw new ArgumentNullException("descriptor");
            }

            if (descriptor.HubType == null)
            {
                return null;
            }

            object hub = _resolver.GetService(descriptor.HubType) ?? Activator.CreateInstance(descriptor.HubType);
            return hub as IHub;
        }
    }
}
