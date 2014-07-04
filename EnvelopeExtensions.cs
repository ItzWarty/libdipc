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
         return envelope.HopsToDestination.Length > 1;
      }

      public static Guid GetNextHopGuid(this IEnvelopeV1 envelope)
      {
         return envelope.HopsToDestination[1];
      }

      public static IEnvelopeV1<T> GetEnvelopeForNextHop<T>(this IEnvelopeV1<T> envelope)
      {
         return new EnvelopeV1<T>(
            envelope.SenderGuid,
            envelope.RecipientGuid,
            envelope.HopsToDestination.SubArray(1),
            envelope.TimeSent,
            envelope.TimeReceived,
            envelope.Message);
      }
   }
}
