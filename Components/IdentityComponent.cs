using System;

namespace Dargon.Ipc.Components
{
   public interface IdentityComponent : Component
   {
      Guid Guid { get; }
   }
}
