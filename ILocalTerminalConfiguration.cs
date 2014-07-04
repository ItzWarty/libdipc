using System;

namespace Dargon.Ipc
{
   public interface ILocalTerminalConfiguration
   {
      string NodeIdentifier { get; set; }
      Guid Guid { get; set; }
   }
}