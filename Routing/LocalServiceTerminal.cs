using System;
using System.Threading;
using ItzWarty;

namespace Dargon.Ipc.Routing
{
   public class LocalServiceTerminal<TServiceInterface> : LocalTerminal
      where TServiceInterface : class
   {
      private readonly TServiceInterface m_service;
      private readonly Thread m_messageConsumerThread;
      private readonly CancellationTokenSource m_cancellationTokenSource = new CancellationTokenSource();

      public LocalServiceTerminal(TServiceInterface service, ILocalTerminalConfiguration config) 
         : base(config)
      {
         if (!typeof(TServiceInterface).IsInterface)
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
