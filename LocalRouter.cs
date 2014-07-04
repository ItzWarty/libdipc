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

      protected override IPeeringResult Peer(IDipNode node)
      {
         if (node.Role == DipRole.RemoteTerminal)
            return PeeringFailure(node, new InvalidOperationException("Local nodes cannot directly peer with remote terminals"));

         if (node.Role == DipRole.LocalTerminal)
            return PeeringSuccess(node);

         throw new NotImplementedException("Not implemented: peering of " + Role + " and " + node.Role);
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
