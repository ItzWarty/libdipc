using System;
using System.Collections.Generic;
using System.Threading;
using Dargon.Ipc.Messaging;

namespace Dargon.Ipc.OldRouting
{
   public interface IDipNode
   {
      DipRole Role { get; }
      Guid Guid { get; }
      IReadOnlyCollection<IDipNode> Peers { get; }
      IDipNode Parent { get; }

      IMessage DequeueMessage(CancellationToken? cancellationToken = null);
      bool TryDequeueMessage(out IMessage message);
   }
}
