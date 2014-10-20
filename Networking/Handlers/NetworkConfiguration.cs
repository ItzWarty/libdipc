using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Ipc.Networking.Handlers
{
   public interface INetworkConfiguration
   {
      int Port { get; }
   }

   public class NetworkNetworkConfiguration : INetworkConfiguration
   {
      public int Port { get { return 21337; } }
   }
}
