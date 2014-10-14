using System;
using Dargon.Ipc.Routing;

namespace Dargon.Ipc.Messaging
{
   public interface IEnvelopeV1
   {
      IDipIdentifier SenderId { get; }
      IDipIdentifier RecipientId { get; }
      Guid[] HopsToDestination { get; }
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
      public IDipIdentifier SenderId { get; private set; }
      public IDipIdentifier RecipientId { get; private set; }
      public Guid[] HopsToDestination { get; private set; }
      public DateTime TimeSent { get; private set; }
      public DateTime TimeReceived { get; private set; }
      public IMessage<T> Message { get; private set; }
      IMessage IEnvelopeV1.Message { get { return Message; } }

      public EnvelopeV1(IDipIdentifier senderGuid, IDipIdentifier recipientGuid, Guid[] hopsToDestintaion, DateTime timeSent, DateTime timeReceived, IMessage<T> message)
      {
         this.SenderId = senderGuid;
         this.RecipientId = recipientGuid;
         this.HopsToDestination = hopsToDestintaion;
         this.TimeSent = timeSent;
         this.TimeReceived = timeReceived;
         this.Message = message;
      }
   }
}
