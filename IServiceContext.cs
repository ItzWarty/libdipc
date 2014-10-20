using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Ipc
{
   public interface IServiceContext
   {
      Guid RemoteNodeId { get; }
      Guid ServiceId { get; }
   }
}
