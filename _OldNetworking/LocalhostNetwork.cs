using System;
using Dargon.Ipc.Messaging;

namespace Dargon.Ipc.OldNetworking
{
   public class LocalhostNetwork : DipNodeBase
   {
      public LocalhostNetwork(string name, Guid? guid) 
         : base(DipRole.HostNetwork, guid ?? Guid.NewGuid(), name)
      {
      }

      protected override IPeeringResult PeerParent(IDipNode parent)
      {
         if (parent.Role == DipRole.GlobalNetwork)
         {
            var result = parent.PeerChildAsync(this).Result;
            return new PeeringResult(result.PeeringState, parent, result.Exception);
         }
         else
            return PeeringFailure(parent, new InvalidOperationException("Localhost Network may only child to Global Network."));
      }

      protected override IPeeringResult PeerChild(IDipNode child)
      {
         if (child.Role.HasFlag(DipRole.Local) || child.Role.HasFlag(DipRole.Host))
         {
            var result = child.PeerParentAsync(this).Result;
            return new PeeringResult(result.PeeringState, child, result.Exception);
         }
         else
            return PeeringFailure(child, new InvalidOperationException("Localhost Network may only parent local and host nodes."));
      }

      public override void ReceiveV1<T>(IEnvelopeV1<T> envelope)
      {
         throw new NotImplementedException();
//         if (envelope.Recipient.Guid == this.Guid)
//            throw new NotImplementedException("Message routing to Local Network");
//
//         this.RouteEnvelope(envelope);
      }
   }
}