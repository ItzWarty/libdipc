using System;

namespace Dargon.Ipc.Routing
{
   public interface IRoutingTable
   {
      void Add(Guid from, Guid to);
      Guid? FindNextHop(Guid destination);
   }
}
