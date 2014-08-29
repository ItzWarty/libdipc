using Dargon.Ipc.Routing;
using Dargon.Transport;

namespace Dargon.Ipc.Networking
{
   public interface INetworkBootstrapper
   {
      INetworkContext Bootstrap(INetworkBootstrapConfiguration config, IDipNodeFactory dipFactory = null, IDtpNodeFactory dtpFactory = null);
   }
}