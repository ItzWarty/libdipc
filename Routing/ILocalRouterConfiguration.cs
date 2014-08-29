using System;

namespace Dargon.Ipc.Routing
{
   public interface ILocalRouterConfiguration
   {
      string NodeIdentifier { get; set; }
      Guid Guid { get; set; }
   }
}