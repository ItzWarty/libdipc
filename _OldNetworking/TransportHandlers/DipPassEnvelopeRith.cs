using Dargon.Transport;

namespace Dargon.Ipc.OldNetworking.TransportHandlers
{
   public class DipPassEnvelopeRith : RemotelyInitializedTransactionHandler
   {
      public DipPassEnvelopeRith(uint transactionId) 
         : base(transactionId)
      {
      }

      public override void ProcessInitialMessage(IDSPExSession session, TransactionInitialMessage message)
      {
      }

      public override void ProcessMessage(IDSPExSession session, TransactionMessage message)
      {
         throw new System.NotImplementedException();
      }
   }
}