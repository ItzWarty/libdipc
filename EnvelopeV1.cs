using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Dargon.Ipc
{
   [ProtoContract]
   public interface IEnvelopeV1
   {
      [ProtoMember(1)]
      Guid SenderGuid { get; }

      [ProtoMember(2)]
      Guid RecipientGuid { get; }

      [ProtoMember(3)]
      Guid[] HopsToDestination { get; }

      [ProtoMember(4)]
      DateTime TimeSent { get; }

      [ProtoMember(5)]
      DateTime TimeReceived { get; }

      [ProtoMember(6)]
      IMessage Message { get; }
   }

   [ProtoContract]
   public interface IEnvelopeV1<out T> : IEnvelopeV1
   {
      [ProtoMember(6)]
      new IMessage<T> Message { get; }
   }

   [ProtoContract]
   public class EnvelopeV1<T> : IEnvelopeV1<T>
   {
      [ProtoMember(1)]
      public Guid SenderGuid { get; private set; }

      [ProtoMember(2)]
      public Guid RecipientGuid { get; private set; }

      [ProtoMember(3)]
      public Guid[] HopsToDestination { get; private set; }

      [ProtoMember(4)]
      public DateTime TimeSent { get; private set; }

      [ProtoMember(5)]
      public DateTime TimeReceived { get; private set; }

      [ProtoMember(6)]
      public IMessage<T> Message { get; private set; }
      IMessage IEnvelopeV1.Message { get { return Message; } }

      public EnvelopeV1(Guid senderGuid, Guid recipientGuid, Guid[] hopsToDestintaion, DateTime timeSent, DateTime timeReceived, IMessage<T> message)
      {
         this.SenderGuid = senderGuid;
         this.RecipientGuid = recipientGuid;
         this.HopsToDestination = hopsToDestintaion;
         this.TimeSent = timeSent;
         this.TimeReceived = timeReceived;
         this.Message = message;
      }
   }
}
