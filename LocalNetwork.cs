using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dargon.Ipc
{
   public class LocalNetwork : DipNodeBase, INetwork
   {
      private readonly ConcurrentDictionary<IDipNode, NetworkNode> m_networkNodeByDipNode = new ConcurrentDictionary<IDipNode, NetworkNode>();
      private readonly ConcurrentDictionary<Guid, NetworkNode> m_innerNodesByDipNode = new ConcurrentDictionary<Guid, NetworkNode>();

      public LocalNetwork(Guid guid, string identifier, INetwork parentNetwork = null)
         : base(DipRole.LocalNetwork, guid, identifier, parentNetwork)
      {
      }

      protected override IPeeringResult Peer(IDipNode node)
      {
         if (node.Role.HasFlag(DipRole.Remote))
            throw new NotImplementedException("Support for remote peering");

         return PeeringSuccess(node);
      }

      public override void ReceiveV1<T>(IEnvelopeV1<T> envelope)
      {
         if (envelope.RecipientGuid == this.Guid)
            throw new NotImplementedException("Envelope intended for local network node");
         else
         {
            RouteEnvelope(envelope);
         }
      }
      
      /*
      public LocalNetwork(Guid? guid = null)
      {
         Guid = gu id ?? Guid.NewGuid();
      }

      public void Peer(INetwork network)
      {
         m_peeredNetworksByGuid.AddOrUpdate(network.Guid, network, (key, oldValue) => oldValue);
      }

      public void RegisterNode(IDipNode node)
      {
         GetOrAddNetworkNode(node);
      }

      private INetworkNode GetOrAddNetworkNode(IDipNode node)
      {
         var newNode = new NetworkNode(node);
         var nodeInDictionary = m_networkNodeByDipNode.AddOrUpdate(node, newNode, (key, existingValue) => existingValue);
         if (newNode == nodeInDictionary)
         {
            m_innerNodesByDipNode.TryAdd(newNode.Guid, newNode);
         }
         return nodeInDictionary;
      }

      public void AddEdge(IDipNode peer1, IDipNode peer2)
      {
         bool peer1IsRemote = peer1.Role.HasFlag(DipRole.Remote);
         bool peer2IsRemote = peer2.Role.HasFlag(DipRole.Remote);
         if (peer1IsRemote && peer2IsRemote)
            throw new InvalidOperationException("Local network shouldn't be routing for remote nodes!");
         else if (peer1IsRemote || peer2IsRemote) // mutually exclusive 
            AddOuterEdge(peer1, peer2);
         else
            AddInnerEdge(peer1, peer2);
      }

      void INetwork.Route(IDipNode source, IDipNode destination)
      {
         throw new NotImplementedException();
      }

      private void AddOuterEdge(IDipNode innerNode, IDipNode outerNode)
      {
         bool innerNodeIsRemote = innerNode.Role.HasFlag(DipRole.Remote);
         bool outerNodeIsRemote = outerNode.Role.HasFlag(DipRole.Remote);
         if (innerNodeIsRemote && outerNodeIsRemote)
            throw new InvalidOperationException("Local network shouldn't be routing for remote nodes!");
         else if (innerNodeIsRemote)
         {
            var temp = innerNode;
            innerNode = outerNode;
            outerNode = temp;
         }

      }

      private void AddInnerEdge(IDipNode peer1, IDipNode peer2)
      {
         m_networkNodeByDipNode[peer1].Link(m_networkNodeByDipNode[peer2]);  
      }

      public NetworkRoute Route(IDipNode source, IDipNode destination)
      {
         m_networkNodeByDipNode[source].Route(destination);
      }*/

      private class NetworkNode : INetworkNode
      {
         private readonly IDipNode m_node;
         private readonly ConcurrentDictionary<Guid, NetworkNode> m_localPeersByGuid = new ConcurrentDictionary<Guid, NetworkNode>();
         
         public NetworkNode(IDipNode node)
         {
            m_node = node;
         }

         public Guid Guid { get { return m_node.Guid; } }

         public void Link(NetworkNode peer)
         {
            bool linkExists = false;
            m_localPeersByGuid.AddOrUpdate(peer.Guid, peer, (key, existingNode) => {
               linkExists = true; 
               return existingNode;
            });
            if (!linkExists)
            {
               peer.m_localPeersByGuid.TryAdd(this.Guid, this);
            }
         }

         public IRoutingResult Route(IDipNode destination)
         {
            var visitedNodes = new HashSet<NetworkNode>();
            var s = new Stack<NetworkNode>();
            s.Push(this);
            while (s.Any())
            {
               var next = s.Pop();
               if (!visitedNodes.Contains(next))
               {

               }
            }
         }
      }
   }

   public interface INetworkNode
   {
      Guid Guid { get; }
   }
}
