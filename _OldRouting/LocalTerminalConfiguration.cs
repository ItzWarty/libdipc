using System;

namespace Dargon.Ipc.OldRouting
{
   public class LocalTerminalConfiguration : ILocalTerminalConfiguration
   {
      public string NodeIdentifier { get; set; }
      public Guid Guid { get; set; }
   }
}
