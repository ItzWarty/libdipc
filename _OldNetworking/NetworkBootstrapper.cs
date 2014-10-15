using Dargon.Transport;
using ItzWarty;

namespace Dargon.Ipc.OldNetworking
{
   public class NetworkBootstrapper : INetworkBootstrapper
   {
      public INetworkContext Bootstrap(INetworkBootstrapConfiguration config, IDipNodeFactory dipFactory = null, IDtpNodeFactory dtpFactory = null)
      {
         dipFactory = dipFactory ?? new DefaultDipNodeFactory();
         dtpFactory = dtpFactory ?? new DefaultDtpNodeFactory();

         var node = dtpFactory.CreateNode(config.IsHostRepresentative, config.Namespace, new DipInstructionSet().Wrap());
         var localRouter = dipFactory.CreateLocalRouter(config.RouterConfiguration);
         var hostNetworkNode = dipFactory.CreateLocalhostNetwork(config.HostName, config.HostGuid);
         var globalNetworkNode = dipFactory.CreateGlobalNetwork(config.Namespace);
         globalNetworkNode.PeerChildAsync(hostNetworkNode).Wait();
         hostNetworkNode.PeerChildAsync(localRouter).Wait();

         return new NetworkContext(this, node, globalNetworkNode, hostNetworkNode, localRouter);
      }
   }
}
