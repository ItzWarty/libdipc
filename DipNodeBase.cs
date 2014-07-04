using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ItzWarty.Collections;

namespace Dargon.Ipc
{
   public abstract class DipNodeBase : IDipNode
   {
      private readonly DipRole m_role;
      private readonly Guid m_guid;
      private readonly string m_identifier;
      private readonly INetwork m_parentNetwork;
      
      private readonly ConcurrentSet<IDipNode> m_peeringNodes = new ConcurrentSet<IDipNode>();
      private readonly ConcurrentSet<IDipNode> m_peers = new ConcurrentSet<IDipNode>();
      private readonly ConcurrentDictionary<Guid, IDipNode> m_peersByGuid = new ConcurrentDictionary<Guid, IDipNode>();
      private readonly BlockingCollection<IMessage> m_messageQueue = new BlockingCollection<IMessage>(new ConcurrentQueue<IMessage>());

      protected DipNodeBase(DipRole role, Guid guid, string identifier, INetwork parentNetwork)
      {
         m_role = role;
         m_guid = guid;
         m_identifier = identifier;
         m_parentNetwork = parentNetwork;
      }

      public DipRole Role { get { return m_role; } }
      public Guid Guid { get { return m_guid; } }
      public string Identifier { get { return m_identifier; } }
      public IReadOnlyCollection<IDipNode> Peers { get { return m_peers; } }
      public INetwork ParentNetwork { get { return m_parentNetwork; } }

      public async Task<IPeeringResult> PeerAsync(IDipNode node)
      {
         if (m_peeringNodes.Contains(node) || m_peers.Contains(node))
            return PeeringSuccess(node);
         else
         {
            m_peeringNodes.TryAdd(node);
            var result = this.Peer(node);
            m_peeringNodes.TryRemove(node);
            if (result.PeeringState == PeeringState.Connected)
            {
               m_peers.TryAdd(node);
               m_peersByGuid.TryAdd(node.Guid, node);
            }
            return result;
         }
      }

      protected abstract IPeeringResult Peer(IDipNode node);

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
         if (this.m_peers.Contains(recipient))
         {
            var envelope = EnvelopeFactory.NewDirectEnvelopeFromMessage(this, recipient, message);
            Send(envelope);
         }
         else
         {
            throw new NotImplementedException("Routing messages");
         }
      }

      public void Send<T>(IEnvelopeV1<T> envelope) { SendV1(envelope); }
      public virtual void SendV1<T>(IEnvelopeV1<T> envelope)
      {
         if (!envelope.HasNextHop())
         {
            if (this.Guid == envelope.HopsToDestination.First())
               this.ReceiveV1(envelope);
            else
            {
               throw new InvalidOperationException("malformed envelope");
            }
         }

         IDipNode nextHop;
         if (!m_peersByGuid.TryGetValue(envelope.GetNextHopGuid(), out nextHop))
            throw new InvalidOperationException("attempted to send envelope to non-connected peer");

         var nextEnvelope = envelope.GetEnvelopeForNextHop();
         nextHop.Receive(nextEnvelope);   
      }

      public void Receive<T>(IEnvelopeV1<T> envelope) { ReceiveV1(envelope); }
      public abstract void ReceiveV1<T>(IEnvelopeV1<T> envelope);

      protected virtual void EnqueueMessage(IMessage message) { m_messageQueue.Add(message); }
      public virtual IMessage DequeueMessage() { return m_messageQueue.Take(); }
      public virtual bool TryDequeueMessage(out IMessage message) { return m_messageQueue.TryTake(out message); }

      protected void RouteEnvelope<T>(IEnvelopeV1<T> envelope)
      {
         Guid hopGuid = envelope.HopsToDestination[0]; // should be router guid
         Debug.Assert(hopGuid == this.Guid);

         IDipNode nextHop;
         if (envelope.RecipientGuid == this.Guid)
         {
            m_messageQueue.Add(envelope.Message);
         }
         else if (!envelope.HasNextHop() || !m_peersByGuid.TryGetValue(envelope.GetNextHopGuid(), out nextHop))
         {
            RerouteEnvelope(envelope);
         }
         else
         {
            nextHop.Receive(envelope.GetEnvelopeForNextHop());
         }
      }

      private void RerouteEnvelope<T>(IEnvelopeV1<T> envelope)
      {
      }
   }
}
