using Dargon.Transport;

namespace Dargon.Ipc.OldNetworking
{
   public interface INetworkBootstrapper
   {
      INetworkContext Bootstrap(INetworkBootstrapConfiguration config, IDipNodeFactory dipFactory = null, IDtpNodeFactory dtpFactory = null);
   }
}