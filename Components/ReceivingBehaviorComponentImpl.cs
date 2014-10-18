using System;

namespace Dargon.Ipc.Components
{
   public class ReceivingBehaviorComponentImpl : ReceivingBehaviorComponent
   {
      private ILocalNodeInternal node;

      public void Attach(ILocalNodeInternal node) { this.node = node; }

      public void Receive(INode sender, IEnvelope envelope) 
      {
         if (node == null) {
            throw new InvalidOperationException("Attach wasn't called!");
         }

         if (envelope.Recipient == node.Guid) {
            // todo
         } else {
            node.GetComponent<RoutingBehaviorComponent>().Route(sender, envelope);
         }
      }
   }
}