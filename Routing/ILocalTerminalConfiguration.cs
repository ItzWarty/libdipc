using System;

namespace Dargon.Ipc.Routing
{
   public interface ILocalTerminalConfiguration
   {
      string NodeIdentifier { get; set; }
      Guid Guid { get; set; }
   }
}