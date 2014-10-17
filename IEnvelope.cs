using System;

namespace Dargon.Ipc
{
   public interface IEnvelope 
   {
      Guid Sender { get; }
      Guid Recipient { get; }
      PayloadType PayloadType { get; }
      byte[] Payload { get; }
   }
}
