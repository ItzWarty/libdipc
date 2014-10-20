using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Ipc.Networking.Handlers;
using Dargon.Transport;
using ItzWarty;
using ItzWarty.Collections;

namespace Dargon.Ipc.Networking
{
   public class DiscoveryEndpointFactory
   {
      private readonly INetworkConfiguration networkConfiguration;
      private readonly IDtpNodeFactory nodeFactory;
      
      public DiscoveryEndpointFactory(INetworkConfiguration networkConfiguration, IDtpNodeFactory nodeFactory)
      {
         this.networkConfiguration = networkConfiguration;
         this.nodeFactory = nodeFactory;
      }

      public IDiscoveryEndpoint Create()
      {
         while (true) {
            try {
               return new DiscoveryEndpointServer(networkConfiguration, nodeFactory);
            }
            catch (Exception e)
            {
               throw new NotImplementedException();
            }
         }
         // node = nodeFactory.CreateNode(NodeRole.ServerOrClient, kPort, new DipcInstructionSet().Wrap());
      }
   }
}
