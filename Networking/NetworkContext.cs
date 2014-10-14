using Dargon.Ipc.Routing;
using Dargon.Transport;

namespace Dargon.Ipc.Networking
{
   public class NetworkContext : INetworkContext
   {
      public INetworkBootstrapper Bootstrapper { get; private set; }
      public IDtpNode Node { get; set; }
      public IDipNode GlobalNetwork { get; private set; }
      public IDipNode HostNetwork { get; private set; }
      public IDipNode LocalRouter { get; private set; }

      public NetworkContext(INetworkBootstrapper bootstrapper, IDtpNode node, IDipNode globalNetwork, IDipNode hostNetwork, IDipNode localRouter)
      {
         Bootstrapper = bootstrapper;
         Node = node;
         GlobalNetwork = globalNetwork;
         HostNetwork = hostNetwork;
         LocalRouter = localRouter;
      }
   }
}