using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Dargon.Ipc
{
   [ProtoContract]
   public interface IMessage
   {
      [ProtoMember(1)]
      Object Content { get; }
   }

   [ProtoContract]
   public interface IMessage<out T> : IMessage
   {
      [ProtoMember(1)]
      new T Content { get; }
   }

   [ProtoContract]
   public class Message<T> : IMessage<T>
   {
      [ProtoMember(1)]
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
