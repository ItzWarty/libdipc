using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Ipc.Networking.Handlers;
using Dargon.Transport;
using ItzWarty;

namespace Dargon.Ipc.Networking
{
   public interface IDiscoveryEndpoint
   {

   }

   public class DiscoveryEndpointServer : IDiscoveryEndpoint
   {
      private readonly IDtpNode node;

      public DiscoveryEndpointServer(INetworkConfiguration configuration, IDtpNodeFactory nodeFactory) {
         this.node = nodeFactory.CreateNode(NodeRole.ServerOrClient, configuration.Port, new DipcServerInstructionSet(this).Wrap<IInstructionSet>());
      }

      public void HandleServiceAdvertisement(Guid nodeGuid, Guid[] serviceGuids) 
      { 
         throw new NotImplementedException();
      }
   }
}
