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
   public class LocalTerminal : DipNodeBase
   {
      public LocalTerminal(ILocalTerminalConfiguration config)
         : base(DipRole.LocalTerminal, config.Guid != Guid.Empty ? config.Guid : Guid.NewGuid(), config.NodeIdentifier)
      {
      }

      protected override IPeeringResult PeerParent(IDipNode parent)
      {
         if (parent.Role.HasFlag(DipRole.Remote))
            PeeringFailure(parent, new InvalidOperationException("Local terminals cannot peer with remote nodes"));

         var result = parent.PeerChildAsync(this).Result;
         return new PeeringResult(result.PeeringState, parent, result.Exception);
      }

      protected override IPeeringResult PeerChild(IDipNode child)
      {
         return PeeringFailure(child, new InvalidOperationException("Terminals cannot have child nodes!"));
      }

      public override void ReceiveV1<T>(IEnvelopeV1<T> envelope)
      {
         if (envelope.RecipientId.Guid != this.Guid)
         {
#if DEBUG
            throw new InvalidOperationException("Local Terminals shouldn't relay messages!");
#else
            RouteEnvelope(envelope);
#endif
         }
         else
         {
            EnqueueMessage(envelope.Message);  
         }
      }
   }
}
