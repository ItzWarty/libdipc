using System;

namespace Dargon.Ipc.OldMessaging
{
   public interface IEnvelopeV1
   {
      Guid Sender { get; }
      Guid Recipient { get; }
      DateTime TimeSent { get; }
      DateTime TimeReceived { get; }
      IMessage Message { get; }
   }

   public interface IEnvelopeV1<out T> : IEnvelopeV1
   {
      new IMessage<T> Message { get; }
   }

   public class EnvelopeV1<T> : IEnvelopeV1<T>
   {
      public Guid Sender { get; private set; }
      public Guid Recipient { get; private set; }
      public DateTime TimeSent { get; private set; }
      public DateTime TimeReceived { get; private set; }
      public IMessage<T> Message { get; private set; }
      IMessage IEnvelopeV1.Message { get { return Message; } }

      public EnvelopeV1(Guid senderGuid, Guid recipientGuid, Guid[] hopsToDestintaion, DateTime timeSent, DateTime timeReceived, IMessage<T> message)
      {
         this.Sender = senderGuid;
         this.Recipient = recipientGuid;
         this.TimeSent = timeSent;
         this.TimeReceived = timeReceived;
         this.Message = message;
      }
   }
}
