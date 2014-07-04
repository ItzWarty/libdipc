using System;

namespace Dargon.Ipc
{
   public interface ILocalRouterConfiguration
   {
      string NodeIdentifier { get; set; }
      Guid Guid { get; set; }
   }
}