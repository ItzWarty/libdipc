namespace Dargon.Ipc.Networking
{
   public interface INetworkContext
   {
      INetworkBootstrapper Bootstrapper { get; }
      IDipNode HostNetwork { get; }
      IDipNode GlobalNetwork { get; }
   }
}