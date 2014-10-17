using System;

namespace Dargon.Ipc.Components
{
   public interface IIdentityComponent : IComponent
   {
      Guid Guid { get; }
   }
}
