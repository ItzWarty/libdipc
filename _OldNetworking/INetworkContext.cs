namespace Dargon.Ipc.OldNetworking
{
   public interface INetworkContext
   {
      INetworkBootstrapper Bootstrapper { get; }
      IDipNode HostNetwork { get; }
      IDipNode GlobalNetwork { get; }
   }
}