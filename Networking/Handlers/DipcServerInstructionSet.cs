using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Transport;

namespace Dargon.Ipc.Networking.Handlers
{
   public class DipcServerInstructionSet : IInstructionSet
   {
      private readonly DiscoveryEndpointServer discoveryEndpoint;

      public DipcServerInstructionSet(DiscoveryEndpointServer discoveryEndpoint) { this.discoveryEndpoint = discoveryEndpoint; }

      public bool UseConstructionContext { get { return false; } }
      public object ConstructionContext { get { throw new InvalidOperationException(); } }

      public bool TryCreateRemotelyInitializedTransactionHandler(byte opcode, uint transactionId, out RemotelyInitializedTransactionHandler handler)
      {
         handler = null;
         switch ((DipcOpcode)opcode) {
            case DipcOpcode.AdvertiseServices:
               handler = new AdvertiseServicesRith(transactionId, discoveryEndpoint);
               break;
         }
         return handler != null;
      }
   }
}
