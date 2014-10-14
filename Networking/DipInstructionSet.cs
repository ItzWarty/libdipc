using System;
using Dargon.Ipc.Networking.TransportHandlers;
using Dargon.Transport;

namespace Dargon.Ipc.Networking
{
   public class DipInstructionSet : IInstructionSet
   {
      public bool UseConstructionContext { get; private set; }
      public object ConstructionContext { get; private set; }
      
      public bool TryCreateRemotelyInitializedTransactionHandler(byte opcode, uint transactionId, out RemotelyInitializedTransactionHandler handler)
      {
         handler = null;
         switch ((DTP_DIP)opcode)
         {
            case DTP_DIP.PASS_ENVELOPE:
               handler = new DipPassEnvelopeRith(transactionId);
               break;
         }
         return handler != null;
      }
   }
}