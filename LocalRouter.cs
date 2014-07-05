using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ItzWarty;

namespace Dargon.Ipc
{
   public class LocalRouter : DipNodeBase
   {
      private readonly ILocalRouterConfiguration m_config;
      
      public LocalRouter(ILocalRouterConfiguration config)
         : base(DipRole.LocalRouter, config.Guid != Guid.Empty ? config.Guid : Guid.NewGuid(), config.NodeIdentifier)
      {
         m_config = config;
      }

      protected override IPeeringResult PeerParent(IDipNode parent)
      {
         var result = parent.PeerChildAsync(this).Result;
         return new PeeringResult(result.PeeringState, parent, result.Exception);
      }

      protected override IPeeringResult PeerChild(IDipNode child)
      {
         if (child.Role == DipRole.RemoteTerminal)
            return PeeringFailure(child, new InvalidOperationException("Local nodes cannot directly peer with remote terminals"));

         var result = child.PeerParentAsync(this).Result;
         return new PeeringResult(result.PeeringState, child, result.Exception);
      }

      public override void SendV1<T>(IEnvelopeV1<T> envelope)
      {
         this.RouteEnvelope(envelope);
      }

      public override void ReceiveV1<T>(IEnvelopeV1<T> envelope)
      {
         this.RouteEnvelope(envelope);
      }
   }
}
