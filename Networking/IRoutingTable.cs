using System;

namespace Dargon.Ipc.Networking
{
   public interface IRoutingTable
   {
      void Add(Guid from, Guid to);
      Guid? FindNextHopOrNull(Guid destination);
   }
}
