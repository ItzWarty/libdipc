using Dargon.Ipc.Networking;

namespace Dargon.Ipc.Components
{
   public class RoutingBehaviorComponentImpl : RoutingBehaviorComponent
   {
      private readonly ITransportLayer transportLayer;
      private ILocalNodeInternal node;

      public RoutingBehaviorComponentImpl(ITransportLayer transportLayer) {
         this.transportLayer = transportLayer;
      }

      public void Attach(ILocalNodeInternal node) { this.node = node; }

      public void Route(INode sender, IEnvelope envelope) { transportLayer.Transport(node, envelope); }
   }
}