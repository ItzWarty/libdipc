namespace Dargon.Ipc.Networking
{
   public interface INetworkContextFactory
   {
      INetworkContext CreateNetworkContext();
   }

   public class NetworkContextFactoryImpl : INetworkContextFactory
   {
      public INetworkContext CreateNetworkContext() 
      { 
         var routingTable = (IRoutingTable)null; 
         return new NetworkContextImpl(routingTable);
      }
   }

   public class NetworkContextImpl : INetworkContext
   {
      private readonly IRoutingTable routingTable;

      public NetworkContextImpl(IRoutingTable routingTable) {
         this.routingTable = routingTable;
      }

      public void AddRemote(string host, int port) {
      }
   }
}
