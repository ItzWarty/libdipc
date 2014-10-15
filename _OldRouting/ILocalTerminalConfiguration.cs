using System;

namespace Dargon.Ipc.OldRouting
{
   public interface ILocalTerminalConfiguration
   {
      string NodeIdentifier { get; set; }
      Guid Guid { get; set; }
   }
}