using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Ipc
{
   public interface IPeeringResult
   {
      PeeringState State { get; }
      INode Node { get; }
      Exception Exception { get; }
   }
}
