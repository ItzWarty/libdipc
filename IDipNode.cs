using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Ipc
{
   public interface IDipNode
   {
      DipRole Role { get; }
      Guid Guid { get; }
      IDipIdentifier Identifier { get; }
      IReadOnlyCollection<IDipNode> Peers { get; }
      IDipNode Parent { get; }

      Task<IPeeringResult> PeerParentAsync(IDipNode parent);
      Task<IPeeringResult> PeerChildAsync(IDipNode child);
      void Send<T>(IDipNode recipient, IMessage<T> message);
      void Send<T>(IEnvelopeV1<T> envelope);
      void SendV1<T>(IEnvelopeV1<T> envelope);
      void Receive<T>(IEnvelopeV1<T> envelope);
      void ReceiveV1<T>(IEnvelopeV1<T> envelope);
      IMessage DequeueMessage();
      bool TryDequeueMessage(out IMessage message);
   }
}
