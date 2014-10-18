using System;

namespace Dargon.Ipc.Components
{
   public class IdentityComponentImpl : IdentityComponent
   {
      private readonly Guid guid;

      public IdentityComponentImpl(Guid guid) {
         this.guid = guid;
      }

      public void Attach(ILocalNodeInternal node) { }

      public Guid Guid { get { return guid; } }
   }
}
