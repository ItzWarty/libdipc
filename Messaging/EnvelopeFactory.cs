using System;
using Dargon.Ipc.Routing;

namespace Dargon.Ipc.Messaging
{
   public static class EnvelopeFactory
   {

      public static IEnvelopeV1<TMessageContent> NewDirectEnvelopeFromContent<TMessageContent>(IDipNode sender, IDipNode receiver, TMessageContent content)
      {
         return NewDirectEnvelopeFromContentV1(sender, receiver, content);
      }

      public static IEnvelopeV1<TMessageContent> NewDirectEnvelopeFromContentV1<TMessageContent>(IDipNode sender, IDipNode receiver, TMessageContent content)
      {
         return NewDirectEnvelopeFromMessageV1(sender, receiver, new Message<TMessageContent>(content));
      }
      public static IEnvelopeV1<TMessageContent> NewDirectEnvelopeFromMessage<TMessageContent>(IDipNode sender, IDipNode receiver, IMessage<TMessageContent> message)
      {
         return NewDirectEnvelopeFromMessageV1(sender, receiver, message);
      }

      public static IEnvelopeV1<TMessageContent> NewDirectEnvelopeFromMessageV1<TMessageContent>(IDipNode sender, IDipNode receiver, IMessage<TMessageContent> message)
      {
         return new EnvelopeV1<TMessageContent>(sender.Identifier, receiver.Identifier, new Guid[] { sender.Guid, receiver.Guid }, DateTime.Now, DateTime.Now, message);
      }

      public static IEnvelopeV1<TMessageContent> NewUnroutedEnvelopeToRecipient<TMessageContent>(IDipNode sender, IDipNode recipient, IMessage<TMessageContent> message)
      {
         return new EnvelopeV1<TMessageContent>(sender.Identifier, recipient.Identifier, null, DateTime.Now, DateTime.Now, message);
      }
   }
}
