using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Ipc
{
   public class LocalTerminal : IDipNode
   {
      private readonly ITerminalConfiguration m_configuration;
      private readonly Dictionary<Guid, IDipNode> m_peersFromGuids = new Dictionary<Guid,IDipNode>();
      private readonly Guid m_guid;
      private readonly ConcurrentQueue<IDipMessage> m_messageQueue = new ConcurrentQueue<IDipMessage>(); 

      public LocalTerminal(ITerminalConfiguration config)
      {
         m_configuration = config;
         m_guid = Guid.NewGuid();
      }

      public async Task<IPeeringResult> Peer(IDipNode node)
      {
         if (node == this)
            return new PeeringResult(PeeringState.Disconnected, null, new Exception("node attempted connection to self"));

         if (m_peersFromGuids.ContainsKey(node.Guid))
            return new PeeringResult(PeeringState.Connected, null, new Exception("already connected to node"));

         var result = await node.Peer(this);
         if (result.PeeringState == PeeringState.Connected)
         {
            m_peersFromGuids.Add(node.Guid, node);
         }
         return new PeeringResult(result.PeeringState, node, result.Exception);
      }

      public void SendMessage<T>(IDipNode recipient, IDipMessage<T> message) where T : ISerializable
      {
         if (!m_peersFromGuids.ContainsKey(recipient.Guid))
            throw new InvalidOperationException("attempted to send message to non-connected peer");

         recipient.ReceiveMessage(this, message);
      }

      public void ReceiveMessage<T>(IDipNode sender, IDipMessage<T> message) where T : ISerializable
      {
         m_messageQueue.Enqueue(message);
      }

      public Guid Guid { get { return m_guid; } }
      public string Identifier { get { return m_configuration.NodeIdentifier; }}
      public IReadOnlyCollection<IDipNode> Peers { get { return m_peersFromGuids.Values.ToList(); } }
   }
}
