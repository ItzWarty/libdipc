using Dargon.Ipc.Routing;

namespace Dargon.Ipc.Components
{
   public class RoutingBehaviorComponentImpl : RoutingBehaviorComponent
   {
      private readonly INetworkLayer networkLayer;
      private ILocalNodeInternal node;

      public RoutingBehaviorComponentImpl(INetworkLayer networkLayer) {
         this.networkLayer = networkLayer;
      }

      public void Attach(ILocalNodeInternal node) { this.node = node; }

      public void Route(INode sender, IEnvelope envelope) { networkLayer.Transport(node, envelope); }
   }
}