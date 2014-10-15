using System;

namespace Dargon.Ipc
{
   public interface ILocalNodeFactory
   {
      ILocalNode CreateTerminal(string identifier);
      ILocalNode CreateRouter(string identifier);
   }
}
