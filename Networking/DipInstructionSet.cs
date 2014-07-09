using System;
using Dargon.Transport;

namespace Dargon.Ipc.Networking
{
   public class DipInstructionSet : IInstructionSet
   {
      public bool UseConstructionContext { get; private set; }
      public object ConstructionContext { get; private set; }
      public Type GetRemotelyInitializedTransactionHandlerType(byte opcode)
      {

      }
   }
}