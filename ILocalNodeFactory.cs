using System;

namespace Dargon.Ipc
{
   public interface ILocalNodeFactory
   {
      ILocalNode CreateServiceTerminal<TService>(TService service);
      ILocalNode CreateRouter(string identifier);
   }
}
