using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Ipc.Components
{
   public interface DiscoveryBehaviorComponent : Component
   {
      void HandleServiceDiscovered(INode node);
      void HandleServiceLost(INode node);
   }
}
