using System;
using Dargon.Ipc.Networking.TransportHandlers;
using Dargon.Transport;

namespace Dargon.Ipc.Networking
{
   public class DipInstructionSet : IInstructionSet
   {
      public bool UseConstructionContext { get; private set; }
      public object ConstructionContext { get; private set; }
      public Type GetRemotelyInitializedTransactionHandlerType(byte opcode)
      {
         switch ((DTP_DIP)opcode)
         {
            case DTP_DIP.PASS_ENVELOPE:
               return typeof(DipPassEnvelopeRith);
         }
         return null;
      }
   }
}