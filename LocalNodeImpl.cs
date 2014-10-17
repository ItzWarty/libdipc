using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dargon.Ipc.Components;
using Dargon.PortableObjects;
using ItzWarty;

namespace Dargon.Ipc
{
   public class LocalNodeImpl : ILocalNode, ILocalNodeInternal
   {
      private readonly IIdentityComponent identity;
      private readonly IPeeringBehaviorComponent peeringBehavior;
      private readonly IReceivingBehaviorComponent receivingBehavior;
      private readonly ISendingBehaviorComponent sendingBehavior;
      private readonly IRoutingBehaviorComponent routingBehavior;
      private readonly Dictionary<Type, IComponent> componentsByInterface = new Dictionary<Type, IComponent>();

      public LocalNodeImpl(IIdentityComponent identity, IPeeringBehaviorComponent peeringBehavior, IReceivingBehaviorComponent receivingBehavior, ISendingBehaviorComponent sendingBehavior, IRoutingBehaviorComponent routingBehavior)
      {
         this.identity = identity;
         this.peeringBehavior = peeringBehavior;
         this.receivingBehavior = receivingBehavior;
         this.sendingBehavior = sendingBehavior;
         this.routingBehavior = routingBehavior;

         componentsByInterface.Add(typeof(IIdentityComponent), identity);
         componentsByInterface.Add(typeof(IPeeringBehaviorComponent), peeringBehavior);
         componentsByInterface.Add(typeof(IReceivingBehaviorComponent), receivingBehavior);
         componentsByInterface.Add(typeof(ISendingBehaviorComponent), sendingBehavior);
         componentsByInterface.Add(typeof(IRoutingBehaviorComponent), routingBehavior);
      }

      public void Initialize()
      {
         foreach (var component in componentsByInterface.Values) {
            component.Attach(this);
         }
      }

      public Guid Guid { get { return identity.Guid; } }
      public Task<IPeeringResult> SetParent(INode node) { return peeringBehavior.PeerParentAsync(node); }
      public void Send<TPayload>(INode node, TPayload payload) where TPayload : IPortableObject { sendingBehavior.Send(node, payload); }
      public void Receive(INode sender, IEnvelope envelope) { receivingBehavior.Receive(sender, envelope); }
      public T GetComponent<T>() where T : IComponent { return (T)componentsByInterface.GetValueOrDefault(typeof(T)); }
   }
}
