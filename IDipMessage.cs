using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Ipc
{
   public interface IDipMessage
   {
      IDipNode Sender { get; }
      IDipNode Recipient { get; }
      Object Content { get; }
   }

   public interface IDipMessage<T> : IDipMessage
   {
      new T Content { get; }
   }

   public class DipMessage<T> : IDipMessage<T>
   {
      public IDipNode Sender { get; private set; }
      public IDipNode Recipient { get; private set; }
      public T Content { get; private set; }

      public DipMessage(IDipNode sender, IDipNode recipient, T content)
      {
         Sender = sender;
         Recipient = recipient;
         Content = content;
      }

      object IDipMessage.Content
      {
         get { return Content; }
      }
   }
}
