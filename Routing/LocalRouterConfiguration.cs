using System;

namespace Dargon.Ipc.Routing
{
   public class LocalRouterConfiguration : ILocalRouterConfiguration
   {
      public string NodeIdentifier { get; set; }
      public Guid Guid { get; set; }
   }
}
