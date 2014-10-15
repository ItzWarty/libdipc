using System;

namespace Dargon.Ipc.OldMessaging
{
   public interface IMessage
   {
      Object Content { get; }
   }

   public interface IMessage<out T> : IMessage
   {
      new T Content { get; }
   }

   public class Message<T> : IMessage<T>
   {
      public T Content { get; private set; }

      public Message(T content)
      {
         Content = content;
      }

      object IMessage.Content
      {
         get { return Content; }
      }
   }
}
