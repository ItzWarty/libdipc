using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItzWarty;

namespace Dargon.Ipc
{
   public static class EnvelopeExtensions
   {
      public static bool HasNextHop(this IEnvelopeV1 envelope)
      {
         return envelope.HopsToDestination != null && envelope.HopsToDestination.Length > 1;
      }

      public static Guid GetNextHopGuid(this IEnvelopeV1 envelope)
      {
         return envelope.HopsToDestination[1];
      }

      public static IEnvelopeV1<T> GetEnvelopeForNextHop<T>(this IEnvelopeV1<T> envelope)
      {
         return envelope.GetReroutedEnvelope(envelope.HopsToDestination.SubArray(1));
      }

      public static IEnvelopeV1<T> GetReroutedEnvelope<T>(this IEnvelopeV1<T> envelope, Guid[] route)
      {
         return new EnvelopeV1<T>(
            envelope.SenderId,
            envelope.RecipientId,
            route,
            envelope.TimeSent,
            envelope.TimeReceived,
            envelope.Message
         );
      }
   }
}
