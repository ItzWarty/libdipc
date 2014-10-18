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
      private readonly IdentityComponent identity;
      private readonly PeeringBehaviorComponent peeringBehavior;
      private readonly ReceivingBehaviorComponent receivingBehavior;
      private readonly SendingBehaviorComponent sendingBehavior;
      private readonly RoutingBehaviorComponent routingBehavior;
      private readonly Dictionary<Type, Component> componentsByInterface = new Dictionary<Type, Component>();

      public LocalNodeImpl(IdentityComponent identity, PeeringBehaviorComponent peeringBehavior, ReceivingBehaviorComponent receivingBehavior, SendingBehaviorComponent sendingBehavior, RoutingBehaviorComponent routingBehavior)
      {
         this.identity = identity;
         this.peeringBehavior = peeringBehavior;
         this.receivingBehavior = receivingBehavior;
         this.sendingBehavior = sendingBehavior;
         this.routingBehavior = routingBehavior;

         componentsByInterface.Add(typeof(IdentityComponent), identity);
         componentsByInterface.Add(typeof(PeeringBehaviorComponent), peeringBehavior);
         componentsByInterface.Add(typeof(ReceivingBehaviorComponent), receivingBehavior);
         componentsByInterface.Add(typeof(SendingBehaviorComponent), sendingBehavior);
         componentsByInterface.Add(typeof(RoutingBehaviorComponent), routingBehavior);
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
      public T GetComponent<T>() where T : Component { return (T)componentsByInterface.GetValueOrDefault(typeof(T)); }
   }
}
