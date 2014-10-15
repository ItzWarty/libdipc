using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dargon.Ipc.Messaging;
using ItzWarty.Collections;

namespace Dargon.Ipc.OldRouting
{
   public abstract class DipNodeBase : IDipNode
   {
      private readonly DipRole m_role;
      private readonly Guid m_guid;
      private readonly string m_name;

      private IDipNode m_parentNode;
      private IDipNode m_peeringParent;
      private readonly Semaphore m_parentNodeSemaphore = new Semaphore(1, 1);
      private readonly ConcurrentSet<IDipNode> m_peeringChildren = new ConcurrentSet<IDipNode>();
      private readonly ConcurrentSet<IDipNode> m_children = new ConcurrentSet<IDipNode>();
      private readonly ConcurrentDictionary<Guid, IDipNode> m_peersByGuid = new ConcurrentDictionary<Guid, IDipNode>();
      private readonly BlockingCollection<IMessage> m_messageQueue = new BlockingCollection<IMessage>(new ConcurrentQueue<IMessage>());
      
      protected DipNodeBase(DipRole role, Guid guid, string name)
      {
         m_role = role;
         m_guid = guid;
         m_name = name;
      }

      public DipRole Role { get { return m_role; } }
      public Guid Guid { get { return m_guid; } }
      public IReadOnlyCollection<IDipNode> Peers { get { return m_peersByGuid.Values.ToList(); } } // TODO: EW
      public IDipNode Parent { get { return m_parentNode; } }

      public async Task<IPeeringResult> PeerParentAsync(IDipNode parent)
      {
         if (parent == null)
            throw new ArgumentNullException("parent");

         if (m_peeringParent == parent)
         {
            m_peeringParent = null;
            return PeeringSuccess(parent);
         }
         try
         {
            this.m_parentNodeSemaphore.WaitOne();
            m_peeringParent = parent;

            if (m_parentNode == parent)
               return PeeringSuccess(parent);

            if (m_children.Contains(parent))
               throw new InvalidOperationException("Cannot create circular child/parent dependency");

            var result = PeerParent(parent);
            if (result.PeeringState == PeeringState.Connected)
            {
               m_parentNode = parent;
               m_peersByGuid.TryAdd(parent.Guid, parent);
            }
            return result;
         }
         finally
         {
            this.m_parentNodeSemaphore.Release();
         }
      }
      
      protected abstract IPeeringResult PeerParent(IDipNode parent);

      public async Task<IPeeringResult> PeerChildAsync(IDipNode child)
      {
         if (m_parentNode == child)
            throw new InvalidOperationException("Cannot create circular child/parent dependency");

         if (m_peeringChildren.Contains(child) || m_children.Contains(child))
         {
            m_peeringChildren.TryRemove(child);
            return PeeringSuccess(child);
         }
         else
         {
            m_peeringChildren.TryAdd(child);
            var result = this.PeerChild(child);
            m_peeringChildren.TryRemove(child);
            if (result.PeeringState == PeeringState.Connected)
            {
               m_children.TryAdd(child);
               m_peersByGuid.TryAdd(child.Guid, child);
            }
            return result;
         }
      }

      protected abstract IPeeringResult PeerChild(IDipNode child);

      protected bool HasPeer(IDipNode node) { return this.HasPeer(node.Guid); }
      protected bool HasPeer(Guid guid) { return m_peersByGuid.ContainsKey(guid); }

      protected IPeeringResult PeeringSuccess(IDipNode node, Exception e = null)
      {
         return new PeeringResult(PeeringState.Connected, node, e);
      }

      protected IPeeringResult PeeringFailure(IDipNode node, Exception e = null)
      {
         return new PeeringResult(PeeringState.Disconnected, node, e);
      }

      public void Send<T>(IDipNode recipient, IMessage<T> message)
      {
         if (this.m_peersByGuid.ContainsKey(recipient.Guid))
         {
            var envelope = EnvelopeFactory.NewDirectEnvelopeFromMessage(this, recipient, message);
            Send(envelope);
         }
         else
         {
            RerouteEnvelope(EnvelopeFactory.NewUnroutedEnvelopeToRecipient(this, recipient, message));
         }
      }

      public void Send<T>(IEnvelopeV1<T> envelope) { SendV1(envelope); }
      public virtual void SendV1<T>(IEnvelopeV1<T> envelope)
      {
         throw new NotImplementedException();
//         if (!envelope.HasNextHop())
//         {
//            if (this.Guid == envelope.Recipient)
//               this.ReceiveV1(envelope);
//            else
//            {
//               throw new InvalidOperationException("malformed envelope");
//            }
//         }
//
//         IDipNode nextHop;
//         if (!m_peersByGuid.TryGetValue(envelope.GetNextHopGuid(), out nextHop))
//            throw new InvalidOperationException("attempted to send envelope to non-connected peer");
//
//         var nextEnvelope = envelope.GetEnvelopeForNextHop();
//         nextHop.Receive(nextEnvelope);   
      }

      public void Receive<T>(IEnvelopeV1<T> envelope) { ReceiveV1(envelope); }
      public abstract void ReceiveV1<T>(IEnvelopeV1<T> envelope);

      protected virtual void EnqueueMessage(IMessage message) { m_messageQueue.Add(message); }
      public virtual IMessage DequeueMessage(CancellationToken? cancellationToken = null) { return cancellationToken == null ? m_messageQueue.Take() : m_messageQueue.Take(cancellationToken.Value); }
      public virtual bool TryDequeueMessage(out IMessage message) { return m_messageQueue.TryTake(out message); }

      protected void RouteEnvelope<T>(IEnvelopeV1<T> envelope)
      {
         throw new NotImplementedException();
//         IDipNode nextHop;
//         if (envelope.Recipient == this.Guid)
//         {
//            m_messageQueue.Add(envelope.Message);
//         }
//         else if (!envelope.HasNextHop() || !m_peersByGuid.TryGetValue(envelope.GetNextHopGuid(), out nextHop))
//         {
//            RerouteEnvelope(envelope);
//         }
//         else
//         {
//            nextHop.Receive(envelope.GetEnvelopeForNextHop());
//         }
      }

      private void RerouteEnvelope<T>(IEnvelopeV1<T> envelope)
      {
         throw new NotImplementedException();
//         var thisCrumbs = this.Identifier.Breadcrumbs;
//         var envelopeCrumbs = envelope.Recipient.Breadcrumbs;
//         
//         // Find common ancestor between breadcrumbms
//         int thisIndexResult = -1;
//         int recipientIndexResult = -1;
//         for (var thisIndex = thisCrumbs.Length - 1; thisIndex >= 0; thisIndex--)
//         {
//            for (var recipientIndex = 0; recipientIndex < envelopeCrumbs.Length; recipientIndex++)
//            {
//               if (thisCrumbs[thisIndex] == envelopeCrumbs[recipientIndex])
//               {
//                  thisIndexResult = thisIndex;
//                  recipientIndexResult = recipientIndex;
//                  goto loop_exit;
//               }
//            }
//         }
//
//      loop_exit:
//         if (thisIndexResult == -1 || recipientIndexResult == -1)
//            return; // drop packet
//
//         var newRoute = new Guid[(thisCrumbs.Length - thisIndexResult - 1) + (envelopeCrumbs.Length - recipientIndexResult - 1) + 1];
//         var newRouteIndex = 0;
//         for (var i = thisCrumbs.Length - 1; i >= thisIndexResult; i--)
//            newRoute[newRouteIndex++] = thisCrumbs[i];
//         for (var i = recipientIndexResult + 1; i < envelopeCrumbs.Length; i++)
//            newRoute[newRouteIndex++] = envelopeCrumbs[i];
//         RouteEnvelope(envelope.GetReroutedEnvelope(newRoute));
      }
   }
}
