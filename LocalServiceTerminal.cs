using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dargon.Ipc.Messaging;
using ItzWarty;
using ProtoBuf;

namespace Dargon.Ipc
{
   public class LocalServiceTerminal<TService> : LocalTerminal
      where TService : class
   {
      private readonly TService m_service;
      private readonly Thread m_messageConsumerThread;
      private readonly CancellationTokenSource m_cancellationTokenSource = new CancellationTokenSource();

      public LocalServiceTerminal(TService service, ILocalTerminalConfiguration config) 
         : base(config)
      {
         if (!typeof(TService).IsInterface)
            throw new InvalidOperationException("Expected TService to be an interface!");

         m_service = service;
         m_messageConsumerThread = new Thread(MessageConsumerThreadStart) { IsBackground = true }.With((self) => self.Start());
      }

      public void Shutdown()
      {
         m_cancellationTokenSource.Cancel();
      }

      private void MessageConsumerThreadStart()
      {
         var cancellationToken = m_cancellationTokenSource.Token;
         try
         {
            while (!cancellationToken.IsCancellationRequested)
            {
               var message = DequeueMessage(cancellationToken);
            }
         }
         catch (OperationCanceledException e) { } // expected if shutdown()
      }
   }
}
