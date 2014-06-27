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
      Guid Guid { get; }
      string Identifier { get; }
      IReadOnlyCollection<IDipNode> Peers { get; }

      Task<IPeeringResult> Peer(IDipNode node);
      void SendMessage<T>(IDipNode recipient, IDipMessage<T> message)
         where T : ISerializable;
      void ReceiveMessage<T>(IDipNode sender, IDipMessage<T> message)
         where T : ISerializable;
   }
}
