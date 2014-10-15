using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dargon.Ipc.Components;
using Dargon.PortableObjects;

namespace Dargon.Ipc
{
   public class LocalNodeImpl : INode
   {
      private readonly IIdentityComponent identity;
      private readonly IPeeringBehaviorComponent peeringBehavior;
      private readonly IReceivingBehaviorComponent receivingBehavior;
      private readonly ISendingBehaviorComponent sendingBehavior;
      private readonly IRoutingBehaviorComponent routingBehavior;
      private readonly Dictionary<Type, object> componentsByInterface = new Dictionary<Type, object>();

      public LocalNodeImpl(IIdentityComponent identity, IPeeringBehaviorComponent peeringBehavior, IReceivingBehaviorComponent receivingBehavior, ISendingBehaviorComponent sendingBehavior, IRoutingBehaviorComponent routingBehavior)
      {
         this.identity = identity;
         this.peeringBehavior = peeringBehavior;
         this.receivingBehavior = receivingBehavior;
         this.sendingBehavior = sendingBehavior;
         this.routingBehavior = routingBehavior;
      }

      public Task<IPeeringResult> ConnectToParent(INode node) { return peeringBehavior.PeerParentAsync(node); }
      public void Send<TPayload>(INode node, TPayload payload) where TPayload : IPortableObject { sendingBehavior.Send(node, payload); }
   }
}
