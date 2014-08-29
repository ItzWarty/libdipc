using System;

namespace Dargon.Ipc.Routing
{
   public class LocalTerminalConfiguration : ILocalTerminalConfiguration
   {
      public string NodeIdentifier { get; set; }
      public Guid Guid { get; set; }
   }
}
