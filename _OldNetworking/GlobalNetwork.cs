using System;
using Dargon.Ipc.Messaging;

namespace Dargon.Ipc.OldNetworking
{
   public class GlobalNetwork : DipNodeBase
   {
      public GlobalNetwork(string name, Guid guid) : base(DipRole.GlobalNetwork, guid, name)
      {
      }

      protected override IPeeringResult PeerParent(IDipNode parent)
      {
         return PeeringFailure(parent, new InvalidOperationException("Global node cannot have parent node"));
      }

      protected override IPeeringResult PeerChild(IDipNode child)
      {
         if (child.Role == DipRole.HostNetwork || child.Role == DipRole.RemoteNetwork)
         {
            var result = child.PeerParentAsync(this).Result;
            return new PeeringResult(result.PeeringState, child);
         }
         else
            return PeeringFailure(child, new InvalidOperationException("Global node cannot have parent node"));
      }

      public override void ReceiveV1<T>(IEnvelopeV1<T> envelope)
      {
         throw new NotImplementedException();
//         if (envelope.Recipient.Guid == this.Guid)
//            throw new NotImplementedException("Message routing to Local Network");
//
//         RouteEnvelope(envelope);
      }
   }
}